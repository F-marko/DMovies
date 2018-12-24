using Model;
using MoWizz.Models;
using MoWizz.Models.TVMazeApiModel;
using MoWizz.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace MoWizz.Controllers
{
    [RoutePrefix("api/movies")]
    public class MoviesController : ApiController
    {
        MovieRepository _movieRepository;

        public MoviesController()
        {
            _movieRepository = new MovieRepository();
        }

        [HttpGet]
        [Route("")]
        public HttpResponseMessage GetMovies([FromUri]PagingParameterModel pagingParameterModel, [FromUri]SearchParameterModel searchParameterModel)
        {
            if (pagingParameterModel == null)
            {
                pagingParameterModel = new PagingParameterModel();
            }

            if (searchParameterModel == null)
            {
                searchParameterModel = new SearchParameterModel();
            }

            // list of all movies
            var source = _movieRepository.GetMovies(searchParameterModel.SearchString).AsQueryable();

            // number of movies in db
            int count = source.Count();

            int CurrentPage = pagingParameterModel.PageNumber;
            int PageSize = pagingParameterModel.PageSize;

            int TotalCount = count;

            int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

            var items = source
                        .Skip((CurrentPage - 1) * PageSize)
                        .Take(PageSize)
                        .ToList();

            var movies = ToMovieListViewModel(items);

            var previousPage = CurrentPage > 1 ? "Yes" : "No";

            var nextPage = CurrentPage < TotalPages ? "Yes" : "No";

            var paginationMetadata = new
            {
                totalCount = TotalCount,
                pageSize = PageSize,
                currentPage = CurrentPage,
                totalPages = TotalPages,
                previousPage,
                nextPage
            };

            // Setting Header
            HttpContext.Current.Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(paginationMetadata));

            return Request.CreateResponse(HttpStatusCode.OK, movies);
        }

        [HttpGet]
        [Route("movie")]
        public HttpResponseMessage GetMovieFromId([FromUri]int id)
        {
            var item = _movieRepository.GetMovie(id);

            if (item == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, new HttpResponseException(HttpStatusCode.NotFound));
            }
            return Request.CreateResponse(HttpStatusCode.OK, item);
        }

        public HttpResponseMessage UpdateRating([FromBody]int movieId, [FromBody]int rating)
        {
            throw new NotImplementedException();
        }

        //[HttpGet]
        //[Route("GetActor")]
        //public HttpResponseMessage GetActor(string name)
        //{
        //    string apiKey = "http://api.tvmaze.com/search/people?q=";
        //    HttpWebRequest apiRequest = WebRequest.Create(apiKey + name) as HttpWebRequest;

        //    string apiResponse = "";
        //    using (HttpWebResponse response = apiRequest.GetResponse() as HttpWebResponse)
        //    {
        //        StreamReader reader = new StreamReader(response.GetResponseStream());
        //        apiResponse = reader.ReadToEnd();
        //    }

        //    Actor info = JsonConvert.DeserializeObject<List<Actor>>(apiResponse).FirstOrDefault();

        //    if (info == null)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.OK, info);
        //    }
        //    return Request.CreateResponse(HttpStatusCode.OK, info.person);
        //}

        #region Helper methods
        private List<MovieListViewModel> ToMovieListViewModel(List<Movie> items)
        {
            var movies = new List<MovieListViewModel>();

            items.ForEach(i => movies.Add(new MovieListViewModel
            {
                Id = i.id,
                Image = i.poster_path,
                Title = i.title
            }));

            return movies;
        }

        #endregion
    }
}
