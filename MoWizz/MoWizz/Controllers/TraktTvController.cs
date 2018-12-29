using MoWizz.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace MoWizz.Controllers
{
    [Authorize]
    public class TraktTvController : Controller
    {
        private readonly Uri baseAddress = new Uri("https://api.trakt.tv/");

        public ActionResult MostPopular()
        {
            IEnumerable<PopularMovieViewModel> model;

            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("trakt-api-version", "2");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("trakt-api-key", "[client_id]");

                HttpResponseMessage response = httpClient.GetAsync("movies/popular").Result;
                if (response.IsSuccessStatusCode)
                {
                    IEnumerable<PopularMovieViewModel> responseData = response.Content.ReadAsAsync<IEnumerable<PopularMovieViewModel>>().Result;
                    return View("PopularMovies", responseData);
                }
            }

            return View();
        }
    }
}