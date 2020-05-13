using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;


namespace AuthUserSample
{
    class Program
    {
        // The application ID that you were given when you
        // registered your application. This is for a 
        // desktop app so there's not client secret.

        private static string _clientId = ConfigurationManager.AppSettings.Get("ClientId");

        // If _storedRefreshToken is null, CodeGrantFlow goes
        // through the entire process of getting the user credentials
        // and permissions. If _storedRefreshToken contains the refresh
        // token, CodeGrantFlow returns the new access and refresh tokens.

        private static string _storedRefreshToken = null;
        private static CodeGrantOauth _tokens = null;
        private static B2BGrantOauth _b2bTokens = null;
        private static DateTime _tokenExpiration;

        static void Main(string[] args)
        {
            try
            {
                // TODO: Add logic to get the logged on user's refresh token 
                // from secured storage. 

                bool b2b = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("b2b"));

                UserOperations userOperations = new UserOperations();

                if (!b2b)
                {
                    _tokens = GetOauthTokens_AuthCode(_storedRefreshToken, _clientId);

                    PrintTokens(_tokens.AccessToken, _tokens.RefreshToken, _tokens.Expiration);
                    userOperations.CreateUser(_tokens.AccessToken);
                    userOperations.GetUsers(_tokens.AccessToken);
                }
                else
                {
                    _b2bTokens = GetToken(_storedRefreshToken, _clientId);

                    PrintTokens(_b2bTokens.AccessToken, _b2bTokens.RefreshToken, _b2bTokens.Expiration);
                    userOperations.GetUsers(_b2bTokens.AccessToken);
                }
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.Message);
            }
        }

        public static void PrintTokens(string accessToken, string refreshToken, int tokenExpiry)
        {
            Console.BackgroundColor = ConsoleColor.Green;
            Console.WriteLine("access token:");
            Console.ResetColor();
            Console.WriteLine(accessToken);
            if (!String.IsNullOrEmpty(refreshToken))
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.WriteLine("refresh token:");
                Console.ResetColor();
                Console.WriteLine(refreshToken);
            }
            Console.BackgroundColor = ConsoleColor.Green;
            Console.WriteLine("token expires");
            Console.ResetColor();
            Console.WriteLine(tokenExpiry);
        }

        private static B2BGrantOauth GetToken(string refreshToken, string clientId)
        {
            B2BGrantOauth auth = new B2BGrantOauth(clientId);

            auth.GetAccessToken();

            return auth;
        } 

        private static CodeGrantOauth GetOauthTokens_AuthCode(string refreshToken, string clientId)
        {
            CodeGrantOauth auth = new CodeGrantOauth(clientId);

            if (string.IsNullOrEmpty(refreshToken))
            {
                auth.GetAccessToken();
            }
            else
            {
                auth.RefreshAccessToken(refreshToken);

                // Refresh tokens can become invalid for several reasons
                // such as the user's password changed.

                if (!string.IsNullOrEmpty(auth.Error))
                {
                    auth = GetOauthTokens_AuthCode(null, clientId);
                }
            }

            // TODO: Store the new refresh token in secured storage
            // for the logged on user.

            if (!string.IsNullOrEmpty(auth.Error))
            {
                throw new Exception(auth.Error);
            }
            else
            {
                _storedRefreshToken = auth.RefreshToken;
                _tokenExpiration = DateTime.Now.AddSeconds(auth.Expiration);
            }

            return auth;
        }
    }
}
