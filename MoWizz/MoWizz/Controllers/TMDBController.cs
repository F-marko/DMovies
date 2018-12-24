using MongoDB.Driver;
using MoWizz.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace MoWizz.Controllers {

    //[Authorize]
    public class TMDBController : Controller {

        private MongoDatabase GetMongoDB() {
            MongoClient mongoClient = new MongoClient(ConfigurationManager.ConnectionStrings["Mongo"].ConnectionString);
            MongoServer server = mongoClient.GetServer();
            return server.GetDatabase("MoWizz");
        }

        public ActionResult MostPopular() {
            var client = new RestClient("https://api.themoviedb.org/3/movie/popular?page=1&language=en-US&api_key=4c769c0e240a8d828da99a1019da67a8");
            var request = new RestRequest(Method.GET);
            request.AddParameter("undefined", "{}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            var model = JsonConvert.DeserializeObject<TMDBPopularMoviesResponse>(response.Content);
            return View("Popular", model.results.ToList());
        }

        public ActionResult Details(int movieId) {
            try {
                //get details
                var model = new DetailsViewModel();
                var detailsClient = new RestClient($"https://api.themoviedb.org/3/movie/"
                + movieId.ToString() + "?language=en-US&api_key=4c769c0e240a8d828da99a1019da67a8");
                var detailsRequest = new RestRequest(Method.GET);
                detailsRequest.AddParameter("undefined", "{}", ParameterType.RequestBody);
                IRestResponse detailsResponse = detailsClient.Execute(detailsRequest);
                model.Details = JsonConvert.DeserializeObject<DetailsResponse>(detailsResponse.Content);

                //get credits
                var creditsClient = new RestClient("https://api.themoviedb.org/3/movie/" + movieId.ToString() + "/credits?api_key=4c769c0e240a8d828da99a1019da67a8");
                var creditsRequest = new RestRequest(Method.GET);
                creditsRequest.AddParameter("undefined", "{}", ParameterType.RequestBody);
                IRestResponse creditsResponse = creditsClient.Execute(creditsRequest);
                model.Credits = JsonConvert.DeserializeObject<CreditsResponse>(creditsResponse.Content);

                var similarClient = new RestClient("https://api.themoviedb.org/3/movie/" + movieId + "/similar?page=1&language=en-US&api_key=4c769c0e240a8d828da99a1019da67a8");
                var similarReq = new RestRequest(Method.GET);
                similarReq.AddParameter("undefined", "{}", ParameterType.RequestBody);
                IRestResponse response = similarClient.Execute(similarReq);
                model.Similar = JsonConvert.DeserializeObject<SimilarMoviesResponse>(response.Content);

                var client = new RestClient("https://api.themoviedb.org/3/movie/" + movieId + "/images?language=en-US&api_key=4c769c0e240a8d828da99a1019da67a8");
                var request = new RestRequest(Method.GET);
                request.AddParameter("undefined", "{}", ParameterType.RequestBody);
                IRestResponse responsePictures = client.Execute(request);
                model.Images = JsonConvert.DeserializeObject<MovieImages>(responsePictures.Content);


                return View(model);
            } catch (Exception e) {
                return View("Error");
            }
        }

        public ActionResult GetGenres() {
            var client = new RestClient("https://api.themoviedb.org/3/genre/movie/list?language=en-US&api_key=4c769c0e240a8d828da99a1019da67a8");
            var request = new RestRequest(Method.GET);
            request.AddParameter("undefined", "{}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {

                var collection = GetMongoDB().GetCollection("Genres");

                var data = JsonConvert.DeserializeObject<GenreResponse>(response.Content);

                foreach (var x in data.genres) {
                    collection.Insert(x);
                }

            } else {
                return View("Error");
            }
            return null;
        }

        public ActionResult SearchMovie() {
            return View("SearchMovie");
        }

        public ActionResult Search(string query, int? page, int? year) {
            try {
                var baseUrl = "https://api.themoviedb.org/3/search/movie?";
                var pageParam = page.HasValue ? $"&page={page.Value}" : "";
                var yearParam = year.HasValue ? $"&year={year.Value}" : "";

                var client = new RestClient($"{baseUrl}query={query}{page}{year}&api_key=4c769c0e240a8d828da99a1019da67a8");
                var request = new RestRequest(Method.GET);
                request.AddParameter("undefined", "{}", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                var model = JsonConvert.DeserializeObject<TMDBPopularMoviesResponse>(response.Content);

                return View("SearchItem", model);
            } catch (Exception e) {
                return View("Error");
            }
        }

    }
}