using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MoWizz.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace MoWizz.Controllers
{
    [Authorize]
    public class NYTController : Controller {
        private readonly string NYTUri = "https://api.nytimes.com/";
        private readonly string moviesAPIKey = "81901124af3c43c5a481e667e5ee2761";

        private MongoDatabase GetMongoDB() {
            MongoClient mongoClient = new MongoClient(ConfigurationManager.ConnectionStrings["Mongo"].ConnectionString);
            MongoServer server = mongoClient.GetServer();
            return server.GetDatabase("MoWizz");
        }

        public ActionResult FillDBReviews() {
            //27960 puta tj 1398 puta

            for (var i = 1; i < 1398; ++i) {

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(NYTUri);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync($"/svc/movies/v2/reviews/all.json/?api-key={moviesAPIKey}&offset={i * 20}").Result;
                if (response.IsSuccessStatusCode) {

                    var collection = GetMongoDB().GetCollection("NYTMovieCritics");

                    var data = response.Content.ReadAsAsync<ResultModel>().Result;

                    foreach (var x in data.results) {
                        collection.Insert(x);
                    }
                } else {
                    //Something has gone wrong, handle it here
                }
            }
            return null;
        }

        public ActionResult FindAReview(string name) {
            var mongoDatabase = GetMongoDB();
            var collection = GetMongoDB().GetCollection("NYTMovieCritics");
            var query = Query<ReviewModel>.Matches(c => c.headline, $".*{name}.*");
            var result = collection.FindAs<ReviewModel>(query).ToList();

            return PartialView("FindReview", result);
        }

        public ActionResult Last10Reviews() {
            var mongoDatabase = GetMongoDB();
            var collection = GetMongoDB().GetCollection("NYTMovieCritics");
            var query = Query<ReviewModel>.NE(c => c.display_title, "");

            var result = collection.FindAs<ReviewModel>(query).SetSortOrder(SortBy.Descending("publication_date")).SetLimit(10).ToList();
            return View("Last10Reviews", result);
        }

        public ActionResult Review(string url) {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(NYTUri);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync($"/svc/news/v3/content/content.json?api-key={moviesAPIKey}&url={url}").Result;
            if (response.IsSuccessStatusCode) {

                //var collection = GetMongoDB().GetCollection("NYTMovieCritics");

                var data = response.Content.ReadAsAsync<ArticleResponseModel>().Result;
                return null;
                //foreach (var x in data.results) {
                //    collection.Insert(x);
                //}
            } else {
                //Something has gone wrong, handle it here
            }
            return null;
        }
    }
}