using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using MongoDB.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using MoWizz.Models.AppModels;

namespace MoWizz.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string Email { get; set; }

        public string Gender { get; set; }

        public List<int> Watched { get; set; }

        public List<int> Watchlist { get; set; }

        public List<MovieRating> Ratings { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, string> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }
}