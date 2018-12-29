using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoWizz.Models
{
    public class SearchParameterModel
    {
        public string SearchString { get; set; } = "";

        public string Order { get; set; } = "Latest";

        public string Year { get; set; } = "All";

        public string Genre { get; set; } = "All";
    }
}