using Infoveave.OAuth.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TradingWizard.OAuth.Controllers
{
    [Route("UpstoxOAuth")]
    public class UpStoxController : Controller
    {

        private ApplicationConfiguration Configuration { get; }
        public UpStoxController(IOptionsSnapshot<ApplicationConfiguration> configuration)
        {
            Configuration = configuration.Value;
        }


        [HttpGet("BeginAuth")]
        public async Task<IActionResult> BeginAuthAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await Task.Delay(0, cancellationToken);
            return Redirect("/UpstoxOAuth/StartOAuth");
        }

        [HttpGet("StartOAuth")]
        public async Task<IActionResult> StartOAuthAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await Task.Delay(0, cancellationToken);
            string apikey = "yourApiKey";
            return Redirect(
                $"https://api.upstox.com/index/dialog/authorize?apiKey={apikey}&redirect_uri={Configuration.Application.BaseUrl}/UpstoxOAuth/OAuthResponse&" +
                $"response_type=code");
        }



        [HttpGet("OAuthResponse")]
        public async Task<IActionResult> OAuthResponseAsync([FromQuery]string code)
        {

            var client = new HttpClient();
            string apikey = "yourApiKey";
            string redirectUri = $"{Configuration.Application.BaseUrl}/UpstoxOAuth/OAuthResponse";
            Content contents = new Content();
            //abc.apiKey = apikey;
            contents.code = code;
            contents.grant_type = "authorization_code";
            contents.redirect_uri = redirectUri;
            client.DefaultRequestHeaders.Add("apiKey", apikey);
            var result = await client.PostAsync($"https://api.upstox.com/index/oauth/token",
                new StringContent(JsonConvert.SerializeObject(contents), Encoding.UTF8, "application/json"));

            var content = await result.Content.ReadAsStringAsync();
            if (!result.IsSuccessStatusCode)
            {
                return Ok("Oops! Some thing Went Wrong with Authentication : " + content);
            }
            var tokenRespose = JsonConvert.DeserializeObject<AuthenticationResult>(content);
            //Get User Details
            client.DefaultRequestHeaders.Add("authorization", $"Bearer {tokenRespose.AccessToken}");
            var userDetails = await client.GetAsync($"https://api.upstox.com/index/profile");
            var response = await userDetails.Content.ReadAsStringAsync();
            var userResponse = JsonConvert.DeserializeObject<UserDetails>(response);


            return View("Redirect", new RedirectData()
            {
                Data = new Dictionary<string, dynamic>()
                {

                    { "AccessToken", tokenRespose.AccessToken },
                    { "RefreshToken", tokenRespose.RefreshToken },
                    { "RemoteUserName", userResponse.displayName },
                    { "RemoteUserId", userResponse.id },
                }
            });
        }
    }
    public class Content
    {
        //public string apiKey { get; set; }
        public string redirect_uri { get; set; }
        public string code { get; set; }
        public string grant_type { get; set; }

    }
}