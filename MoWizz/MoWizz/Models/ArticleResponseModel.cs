using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoWizz.Models {
    public class ArticleResponseModel {
        public Article[] results { get; set; }
    }

    public class Article {
        public string section { get; set; }
        public string subsection { get; set; }
        public string title { get; set; }
        //public string abstract { get; set; }
        public string url { get; set; }
        public string short_url { get; set; }
        public string byline { get; set; }
        public string published_date { get; set; }
        public string thumbnail_standard { get; set; }
        public string kicker { get; set; }
        public string headline { get; set; }
        public SuggestedURL[] related_urls { get; set; }
        public Media[] multimedia;
    }

    public class SuggestedURL {
        public string suggested_link_text { get; set; }
        public string url { get; set; }
    }

    public class Media {
        public string url { get; set; }
        public string type { get; set; }
    }
}