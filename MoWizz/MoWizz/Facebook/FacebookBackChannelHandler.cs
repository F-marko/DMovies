using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeService.Facebook
{
    public class FacebookBackChannelHandler : HttpClientHandler
    {
        protected override async Task<HttpResponseMessage>
            SendAsync(HttpRequestMessage request,
                      CancellationToken cancellationToken)
        {
            System.Diagnostics.Debug.WriteLine("FacebookBackChannelHandler");

            if (!request.RequestUri.AbsolutePath.Contains("/oauth"))
            {
                string url1 = request.RequestUri.AbsolutePath;
            //    string url2 = url1.Substring(url1.IndexOf("access_token") + 13);
            //    string accessToken = url2.Substring(0, url2.IndexOf("&"));
                System.Diagnostics.Debug.WriteLine("Access token = " + url1);
                request.RequestUri = new Uri(
                    request.RequestUri.AbsoluteUri.Replace("?access_token", "&access_token"));
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}