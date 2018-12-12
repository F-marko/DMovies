using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoWizz.Models
{
    public class PopularMovieViewModel
    {
        public string title { get; set; }
        public string year { get; set; }
        public Ids ids { get; set; }
    }

    public class Ids
    {
        public string trakt { get; set; }
        public string slug { get; set; }
        public string imdb { get; set; }
        public string tmdb { get; set; }
    }
}