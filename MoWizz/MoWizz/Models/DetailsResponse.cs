using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoWizz.Models
{
    public class DetailsResponse
    {
        public string backdrop_path { get; set; }
        public string budget { get; set; }
        public Genre[] genres { get; set; }
        public string homepage { get; set; }
        public int id { get; set; }
        public string imdb_id { get; set; }
        public string original_language { get; set; }
        public string original_title { get; set; }
        public string overview { get; set; }
        public string popularity { get; set; }
        public string posterPath { get; set; }
        public Company[] production_companies { get; set; }
        public Country[] production_countries { get; set; }
        public string release_date { get; set; }
        public string revenue { get; set; }
        public Country[] spoken_languages { get; set; }  
        public string status { get; set; }
        public string tagline { get; set; }
        public string title { get; set; }
        public string vote_average { get; set; }
        public int vote_count { get; set; }
    }

    public class Genre
    {
        public int id { get; set; }
        public string name { get; set; }

        public override bool Equals(object obj)
        {
            var genre = obj as Genre;
            return genre != null &&
                   name == genre.name;
        }

        public override int GetHashCode()
        {
            return 363513814 + EqualityComparer<string>.Default.GetHashCode(name);
        }
    }

    public class Company
    {
        public int id { get; set; }
        public string logo_path { get; set; }
        public string name { get; set; }
        public string origin_country { get; set; }
    }

    public class Country
    {
        public string iso_3166_1 { get; set; }
        public string name { get; set; }
    }

}