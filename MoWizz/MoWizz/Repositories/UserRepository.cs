using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MoWizz.Models;
using MoWizz.Models.AppModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoWizz.Repositories
{
    public class UserRepository
    {
        //private static MongoClient _client = new MongoClient("mongodb://localhost:27017");
        //private static readonly MongoDatabase _database = _client.GetServer().GetDatabase("dm-mowizz");
        private readonly MongoCollection<ApplicationUser> _usersCollection;

        public UserRepository()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetServer().GetDatabase("dm-mowizz");

            _usersCollection = database.GetCollection<ApplicationUser>("AspNetUsers");
        }

        public void AddToWatchlist(string userId, int movieId)
        {
            //var usersCollection = _database.GetCollection<ApplicationUser>("AspNetUsers");

            var query = Query<ApplicationUser>.EQ(user => user.Id, userId);
            var update = Update<ApplicationUser>.AddToSet(user => user.Watchlist, movieId);

            _usersCollection.Update(query, update);
        }

        public void Update(ApplicationUser user)
        {
            //var usersCollection = _database.GetCollection<ApplicationUser>("AspNetUsers");

            var query = Query<ApplicationUser>.EQ(u => u.Id, user.Id);
            var update = Update<ApplicationUser>.Replace(user);

            _usersCollection.Update(query, update);
        }
    }
}