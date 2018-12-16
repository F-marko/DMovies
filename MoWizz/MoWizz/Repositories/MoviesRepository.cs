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
    public class MoviesRepository
    {
        private static MongoClient _client = new MongoClient("mongodb://localhost:27017");
        private static readonly MongoDatabase _database = _client.GetServer().GetDatabase("MoWizz");

        public static MongoDatabase GetDatabase()
        {
            return _database;
        }

        public static void Initialize(ICollection<MovieInfo> movies)
        {
            var mongoMovies = _database.GetCollection<MovieInfo>("movies");
            mongoMovies.Drop();
            mongoMovies.InsertBatch(movies);
        }

        public static IList<MovieInfo> GetMovies(string title)
        {
            if (title == null)
            {
                return null;
            }
            var mongoMovies = _database.GetCollection<MovieInfo>("movies");
            var query = Query<MovieInfo>.Matches(m => 
                m.Title, 
                new BsonRegularExpression(new Regex(title, RegexOptions.IgnoreCase))
            );


            return mongoMovies.Find(query).ToList();
        }

        public static MovieInfo GetMovie(string imdbId)
        {
            var mongoMovies = _database.GetCollection<MovieInfo>("movies");
            var query = Query<MovieInfo>.EQ(m => m.imdbID, imdbId);
            return mongoMovies.Find(query).FirstOrDefault();
        }

        public static void AddToWatchlist(string user, string imdbId)
        {
            var users = _database.GetCollection<ApplicationUser>("AspNetUsers");
            var query = Query<ApplicationUser>.EQ(u => u.UserName, user);
            ApplicationUser us = users.Find(query).First();
            if (us.Watchlist == null)
            {
                users.Update(query, Update<ApplicationUser>.Set(usr => usr.Watchlist, new List<string>()));
            }
            var update = Update<ApplicationUser>.AddToSet(usr => usr.Watchlist, imdbId);
            users.Update(query, update);
        }

        public static IList<MovieInfo> GetWatchlist(string user)
        {
            var users = _database.GetCollection<ApplicationUser>("AspNetUsers");
            var query = Query<ApplicationUser>.EQ(u => u.UserName, user);
            ApplicationUser us = users.Find(query).First();

            List<MovieInfo> watchlist = new List<MovieInfo>();
            foreach (string id in us.Watchlist)
            {
                watchlist.Add(GetMovie(id));
            }

            return watchlist;
        }
    }
}