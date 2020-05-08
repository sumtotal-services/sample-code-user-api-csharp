using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AuthUserSample
{
    public class B2BGrantOauth
    {
        private string _accessToken = null;
        private string _refreshToken = null;
        private string _authorizationCode = null;
        private int _expiration;
        private string _error = null;

        // Production OAuth server endpoints.
        public static string env = ConfigurationManager.AppSettings.Get("Environmnet");
        public static string redirect = ConfigurationManager.AppSettings.Get("RedirectUri");
        public static string clientId = ConfigurationManager.AppSettings.Get("ClientId");
        public static string clientSecret = ConfigurationManager.AppSettings.Get("ClientSecret");

        private string AuthorizationUri = env + "/apisecurity/connect/authorize";  // Authorization code endpoint
        private string RefreshUri = env + "/apisecurity/connect/token";  // Get tokens endpoint

        private string CodeQueryString = "?client_id={0}&client_secret={1}&scope=allapis&response_type=code";
        private string AccessBody = "client_id={0}&client_secret={1}&grant_type=client_credentials";

        private string _clientId = null;
        private string _clientSecret = null;
        private string _uri = null;

        public string AccessToken { get { return this._accessToken; } }
        public string RefreshToken { get { return this._refreshToken; } }
        public int Expiration { get { return this._expiration; } }
        public string Error { get { return this._error; } }

        public B2BGrantOauth(string clientId)
        {

            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentException("The client ID is missing.");
            }

            this._clientId = clientId;
            this._clientSecret = clientSecret;
            this._uri = string.Format(this.AuthorizationUri + this.CodeQueryString, this._clientId, this._clientSecret);
        }

        public string GetAccessToken()
        {
            try
            {
                var accessTokenRequestBody = string.Format(this.AccessBody, this._clientId, this._clientSecret);
                AccessTokens tokens = GetTokens(this.RefreshUri, accessTokenRequestBody);
                this._accessToken = tokens.AccessToken;
                this._refreshToken = tokens.RefreshToken;
                this._expiration = tokens.Expiration;
            }
            catch (WebException ex)
            {
                this._error = "GetAccessToken failed likely due to an invalid client ID, client secret, or authorization code";
            }

            return this._accessToken;
        }

        private static AccessTokens GetTokens(string uri, string body)
        {

            try
            {
                AccessTokens tokens = null;
                var request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.Accept = "application/json";
                request.ContentType = "application/x-www-form-urlencoded";

                request.ContentLength = body.Length;

                using (Stream requestStream = request.GetRequestStream())
                {
                    StreamWriter writer = new StreamWriter(requestStream);
                    writer.Write(body);
                    writer.Close();
                }

                var response = (HttpWebResponse)request.GetResponse();

                using (Stream responseStream = response.GetResponseStream())
                {
                    var reader = new StreamReader(responseStream);
                    string json = reader.ReadToEnd();
                    reader.Close();
                    tokens = JsonConvert.DeserializeObject(json, typeof(AccessTokens)) as AccessTokens;
                }

                return tokens;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}
