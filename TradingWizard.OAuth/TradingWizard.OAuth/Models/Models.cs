using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infoveave.OAuth.Models
{
    public class AuthorisationRequest
    {
        public string Tenant { get; set; }
        public long UserId { get; set; }
        public string RequestDomain { get; set; }
        public string RedirectUrl { get; set; }
    }
    public class UserDetails
    {
        public string avatarUrl { get; set; }
        public string displayName { get; set; }
        public string id { get; set; }
        public DateTime created { get; set; }
        public DateTime updated { get; set; }
    }
    public class AuthenticationResult
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }

    public class RedirectData
    {
        public string RedirectUrl { get; set; }
        public Dictionary<string, dynamic> Data { get; set; }
    }

    public class FacebookProfileResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class ApplicationConfiguration
    {
        public Application Application { get; set; }
    }

    public class Application
    {
        public string BaseUrl { get; set; }
    }
}
