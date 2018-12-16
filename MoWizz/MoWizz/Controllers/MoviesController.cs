using MoWizz.Models.TVMazeApiModel;
using MoWizz.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MoWizz.Controllers
{
    [RoutePrefix("api/Movies")]
    public class MoviesController : ApiController
    {
        [HttpGet]
        [Route("GetMoviesFromTitle")]
        public HttpResponseMessage GetMoviesFromTitle(string title)
        {
            return Request.CreateResponse(HttpStatusCode.OK, MoviesRepository.GetMovies(title));
        }

        [HttpGet]
        [Route("GetMovieFromId")]
        public HttpResponseMessage GetMovieFromId(string imdbId)
        {
            return Request.CreateResponse(HttpStatusCode.OK, MoviesRepository.GetMovie(imdbId));
        }

        [HttpGet]
        [Route("GetActor")]
        public HttpResponseMessage GetActor(string name)
        {
            string apiKey = "http://api.tvmaze.com/search/people?q=";
            HttpWebRequest apiRequest = WebRequest.Create(apiKey + name) as HttpWebRequest;

            string apiResponse = "";
            using (HttpWebResponse response = apiRequest.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                apiResponse = reader.ReadToEnd();
            }

            Actor info = JsonConvert.DeserializeObject<List<Actor>>(apiResponse).FirstOrDefault();

            if (info == null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, info);
            }
            return Request.CreateResponse(HttpStatusCode.OK, info.person);
        }

        [HttpGet]
        [Route("GetWatchlist")]
        public HttpResponseMessage GetWatchlist(string user)
        {
            return Request.CreateResponse(HttpStatusCode.OK, MoviesRepository.GetWatchlist(user));
        }

        [HttpGet]
        [Route("Add")]
        public IHttpActionResult AddToWatchlist(string user, string imdbId)
        {
            MoviesRepository.AddToWatchlist(user, imdbId);
            return Ok();
        }
    }
}
