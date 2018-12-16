using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoWizz.Models
{
    public class HomeViewModel
    {
        public List<ReviewModel> recentReviews; //two
        public int numOfRecentReviews = 2;

        public TMDBPopularMoviesResponse trending;

        //TODO add other stuff
    }
}