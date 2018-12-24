using Model;
using MoWizz.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoWizz.Services
{
    public class MoviesService
    {
        MovieRepository _movieRepository;

        public MoviesService()
        {
            _movieRepository = new MovieRepository();
        }

        public List<Movie> GetMovies(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }
    }
}