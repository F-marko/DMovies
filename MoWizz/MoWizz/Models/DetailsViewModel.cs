using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoWizz.Models
{
    public class DetailsViewModel
    {
        public DetailsResponse Details { get; set; }
        public CreditsResponse Credits { get; set; }
        public SimilarMoviesResponse Similar { get; set; }
        public MovieImages Images { get; set; }
    }
}