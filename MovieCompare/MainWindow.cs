using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;

namespace MovieCompare
{
    public partial class mainWindow : Form
    {
        //current movie on the screen
        Movie currentMovie;
        // Instantiate a new client, all that's needed is an API key, then fetch config for the client
        TMDbClient client = new TMDbClient(Program.apiKey);
        
        public mainWindow()
        {
            //setup config for TMDb client
            FetchConfig(client);

            InitializeComponent();
        }

        private void mainWindow_Load(object sender, EventArgs e)
        {
            //add the list of available movies to the autocomplete list for the textbox
            SetupTextBoxAutoCompleteList(txtMovieSearch);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchMovies(client, txtMovieSearch.Text);
        }

        private void SetupTextBoxAutoCompleteList(TextBox tb)
        {
            string[] movieList;
            var autoCompleteSource = new AutoCompleteStringCollection();
            //set up list of movies
            movieList = GetListOfAllMovies();
            autoCompleteSource.AddRange(movieList);
            //configure text box with list
            tb.AutoCompleteCustomSource = autoCompleteSource;
            tb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            tb.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        private string[] GetListOfAllMovies()
        {
            StreamReader file;
            string line;
            string title;
            List<string> movieTitles = new List<string>();
            try
            {
                file = new StreamReader("..\\..\\..\\MovieList.json");
            }
            catch (FileNotFoundException e)
            {
                MessageBox.Show(e.ToString(), "Error in opening Movie List");
                return null;
            }

            while ((line = file.ReadLine()) != null)
            {
                title = ExtractTitleFromLine(line);
                movieTitles.Add(title);
            }

            return movieTitles.ToArray();
        }

        private string ExtractTitleFromLine(string line)
        {
            Dictionary<string, string> lineAttributes = JsonConvert.DeserializeObject<Dictionary<string, string>>(line);
            return lineAttributes["original_title"];
        }

        private void FetchConfig(TMDbClient client)
        {
            FileInfo configJson = new FileInfo("config.json");

            Console.WriteLine("Config file: " + configJson.FullName + ", Exists: " + configJson.Exists);

            if (configJson.Exists && configJson.LastWriteTimeUtc >= DateTime.UtcNow.AddHours(-1))
            {
                Console.WriteLine("Using stored config");
                string json = File.ReadAllText(configJson.FullName, Encoding.UTF8);

                client.SetConfig(JsonConvert.DeserializeObject<TMDbConfig>(json));
            }
            else
            {
                Console.WriteLine("Getting new config");
                client.GetConfig();

                Console.WriteLine("Storing config");
                string json = JsonConvert.SerializeObject(client.Config);
                File.WriteAllText(configJson.FullName, json, Encoding.UTF8);
            }
        }

        private static void SearchMovies(TMDbClient client, string searchQuery)
        {

            // This example shows the fetching of a movie.
            SearchContainer<SearchMovie> results = client.SearchMovieAsync(searchQuery).Result;

            // The results is a list, currently on page 1 because we didn't specify any page.
            Console.WriteLine("Searched for movies: '" + searchQuery + "', found " + results.TotalResults + " results in " +
                              results.TotalPages + " pages");

            // Let's iterate the first few hits
            foreach (SearchMovie result in results.Results.Take(3))
            {
                // Print out each hit
                Console.WriteLine(result.Id + ": " + result.Title);
                Console.WriteLine("\t Original Title: " + result.OriginalTitle);
                Console.WriteLine("\t Release date  : " + result.ReleaseDate);
                Console.WriteLine("\t Popularity    : " + result.Popularity);
                Console.WriteLine("\t Vote Average  : " + result.VoteAverage);
                Console.WriteLine("\t Vote Count    : " + result.VoteCount);
                Console.WriteLine();
                Console.WriteLine("\t Backdrop Path : " + result.BackdropPath);
                Console.WriteLine("\t Poster Path   : " + result.PosterPath);

                Console.WriteLine();
            }     
        }
    }
}
