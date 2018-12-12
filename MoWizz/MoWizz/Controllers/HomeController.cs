using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MoWizz.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoWizz.Controllers
{
    public class HomeController : Controller
    {
        private readonly string NYTUri = "https://api.nytimes.com/";
        private readonly string moviesAPIKey = "81901124af3c43c5a481e667e5ee2761";

        private MongoDatabase GetMongoDB()
        {
            MongoClient mongoClient = new MongoClient(ConfigurationManager.ConnectionStrings["Mongo"].ConnectionString);
            MongoServer server = mongoClient.GetServer();
            return server.GetDatabase("MoWizz");
        }

        public ActionResult Index()
        {
            if (!Request.IsAuthenticated)
            {
                return View();
            }

            HomeViewModel model = new HomeViewModel();

            //get last review
            var mongoDatabase = GetMongoDB();
            var collection = GetMongoDB().GetCollection("MovieCritics");
            var query = Query<ReviewModel>.NE(c => c.display_title, "");

            var result = collection.FindAs<ReviewModel>(query).SetSortOrder(SortBy.Descending("publication_date")).SetLimit(2).ToList();
            model.recentReviews = result;

            var client = new RestClient("https://api.themoviedb.org/3/trending/movie/week?api_key=4c769c0e240a8d828da99a1019da67a8");
            var request = new RestRequest(Method.GET);
            request.AddParameter("undefined", "{}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
           model.trending = JsonConvert.DeserializeObject<TMDBPopularMoviesResponse>(response.Content);

            return View("HomePage", model);
        }
    }
}
