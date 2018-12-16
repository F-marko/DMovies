using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoWizz.Models {
    public class ReviewModel {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string display_title { get; set; }
        public string mpaa_rating { get; set; }
        public string critics_pick { get; set; }
        public string byline { get; set; }
        public string headline { get; set; }
        public string summary_short { get; set; }
        public string publication_date { get; set; }
        public Link link { get; set; }
        public Multimedia multimedia { get; set; }
    }

    public class Link {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public string suggested_link_text { get; set; }
    }

    public class Multimedia {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string type { get; set; }
        public string src { get; set; }
        public string width { get; set; }
        public string height { get; set; }
    }
}