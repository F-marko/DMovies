using Model;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MoWizz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MoWizz.Repositories
{
    public class MovieRepository
    {
        private static MongoClient _client = new MongoClient("mongodb://localhost:27017");
        private static readonly MongoDatabase _database = _client.GetServer().GetDatabase("dm-mowizz");

        public Movie GetMovie(int id)
        {
            var moviesCollection = _database.GetCollection<Movie>("movies");

            var query = Query<Movie>.EQ(m => m.id, id);

            return moviesCollection.FindOne(query);
        }

        public List<Movie> GetMovies(string searchString)
        {
            var moviesCollection = _database.GetCollection<Movie>("movies");


            if (String.IsNullOrEmpty(searchString))
            {
                return moviesCollection.FindAll().ToList();
            }
            else
            {
                var query = Query<Movie>.Matches(m =>
                    m.title,
                    new BsonRegularExpression(new Regex(searchString, RegexOptions.IgnoreCase))
                );

                return moviesCollection.Find(query).ToList();
            }
        }

        public void UpdateRating(int movieId, int oldUserRating, int newUserRating, bool updateCount)
        {
            var moviesCollection = _database.GetCollection<Movie>("movies");

            var query = Query<Movie>.EQ(movie => movie.id, movieId);
            //var update = Update<Movie>.

            //moviesCollection.Update(query, update);
        }

        public List<Movie> GetFilteredMovies(string search)
        {
            throw new NotImplementedException();
        }
    }
}