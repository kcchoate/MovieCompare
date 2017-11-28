using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCompare
{
    class Movie
    {
        public string title { get; set; }
        public int id { get; set; }
        public string averageVote { get; set; }
        public double popularity { get; set; }
        public List<string> cast { get; set; }

        //constructors
        public Movie(string _title)
        {
            title = _title;
        }

        public Movie(int _id)
        {
            id = _id;
        }
    }
}
