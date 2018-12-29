using MoWizz.Models;
using MoWizz.Models.TVMazeApiModel;
using MoWizz.Repositories;
using MoWizz.Services.Sorters;
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
            //System.Diagnostics.Debug.WriteLine("Session " + System.Web.HttpContext.Current.Session!=null);

            //foreach (string key in HttpContext.Current.Session.Contents)
            //{
            //    string value = "Key: " + key + ", Value: " + HttpContext.Current.Session[key].ToString();

            //    System.Diagnostics.Debug.WriteLine(value);
            //}

            if (pagingParameterModel == null)
            {
                pagingParameterModel = new PagingParameterModel();
            }

            if (searchParameterModel == null)
            {
                searchParameterModel = new SearchParameterModel();
            }

            // list of all movies
            var source = _movieRepository.GetMovies(searchParameterModel.SearchString);

            var filtered = Filter(source, searchParameterModel.Year, searchParameterModel.Genre);

            var sorted = new SorterFactory().Get(searchParameterModel.Order).Sorted(filtered).AsQueryable();

            // number of movies in db
            int count = source.Count();

            int CurrentPage = pagingParameterModel.PageNumber;
            int PageSize = pagingParameterModel.PageSize;

            int TotalCount = count;

            int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

            var items = sorted
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

        #region Helper methods

        private List<Movie> Filter(List<Movie> source, string year, string genreName)
        {
            if (genreName.Equals("All") && year.Equals("All"))
            {
                return source;
            }

            if (!genreName.Equals("All") && !year.Equals("All"))
            {
                var genre = new Genre
                {
                    name = genreName
                };
                return source.FindAll(m => m.genres.Contains(genre) && DateTime.Parse(m.release_date).Year.Equals(DateTime.Parse(year + "-01-01").Year));
            }

            if (!genreName.Equals("All"))
            {
                var genre = new Genre
                {
                    name = genreName
                };
                return source.FindAll(m => m.genres.Contains(genre));
            }

            return source.FindAll(m => DateTime.Parse(m.release_date).Year.Equals(DateTime.Parse(year + "-01-01").Year));
        }

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
