using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoWizz.Models
{
    public class ResultModel
    {
        public string status { get; set; }
        public string copyright { get; set; }
        public string has_more { get; set; }
        public string num_results { get; set; }
        public ReviewModel[] results { get; set; }
    }
}