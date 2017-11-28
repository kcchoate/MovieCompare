using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MovieCompare
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        public static string apiKey = GetAPIKey();
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new mainWindow());
        }

        //returns the APIKey from a .txt file
        private static string GetAPIKey()
        {
            System.IO.StreamReader file;
            string line;

            try
            {
                file = new System.IO.StreamReader("..\\..\\..\\APIKey.txt");
            }
            catch (System.IO.FileNotFoundException e)
            {
                MessageBox.Show(e.ToString(), "APIKey Not Found");
                return null;
            }
            try
            {
                line = file.ReadLine() ?? throw new Exception("APIKey.txt does not contain data.");
                file.Close();
                return line;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error");
                file.Close();
                return null;
            }
        }
    }
}
