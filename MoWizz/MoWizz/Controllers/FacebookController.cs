using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace MoWizz.Controllers
{
    [Authorize]
    public class FacebookController : ApiController
    {
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public async Task<IHttpActionResult> GetRecommendationsAsync()
        {
            var currentClaims = await UserManager.GetClaimsAsync(HttpContext.Current.User.Identity.GetUserId());

            var accessToken = currentClaims.FirstOrDefault();

            throw new NotImplementedException();
        }

    }
}
