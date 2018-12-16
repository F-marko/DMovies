using MoWizz.Models;
using MoWizz.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MoWizz
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            MoviesRepository.Initialize(GetMovies());
        }

        private ICollection<MovieInfo> GetMovies()
        {
            string apiKey = ConfigurationManager.ConnectionStrings["OMDBApiKey"].ConnectionString;

            ICollection<MovieInfo> movies = new List<MovieInfo>();
            string[] titles = { "game", "home", "house", "king", "ring", "field", "school" };
            
            foreach (string title in titles)
            {
                foreach (SearchResult result in GetMoviesForTitle(title, apiKey))
                {
                    movies.Add(GetMovieInfo(result.imdbID, apiKey));
                }
            }

            return movies;
        }

        private ICollection<SearchResult> GetMoviesForTitle(string title, string apiKey)
        {
            HttpWebRequest apiRequest = WebRequest.Create("http://www.omdbapi.com/?apikey=" + apiKey + "&s=" + title) as HttpWebRequest;

            string apiResponse = "";
            using (HttpWebResponse response = apiRequest.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                apiResponse = reader.ReadToEnd();
            }

            List<SearchResult> list = new List<SearchResult>();
            SearchResults results = JsonConvert.DeserializeObject<SearchResults>(apiResponse);
            if (results.Search != null)
            {
                foreach (SearchResult m in results.Search)
                {
                    list.Add(m);
                }
            }

            return list;
        }

        private MovieInfo GetMovieInfo(string imdbId, string apiKey)
        {
            HttpWebRequest apiRequest = WebRequest.Create("http://www.omdbapi.com/?apikey=" + apiKey + "&i=" + imdbId) as HttpWebRequest;

            string apiResponse = "";
            using (HttpWebResponse response = apiRequest.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                apiResponse = reader.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<MovieInfo>(apiResponse);
        }
    }
}
