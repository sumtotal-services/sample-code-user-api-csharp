using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AuthUserSample
{
    public class UserOperations
    {
        string env = ConfigurationManager.AppSettings.Get("Environmnet");

        public void CreateUser(string accessToken)
        {
            try
            {
                Console.WriteLine("Creating User");

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(env + "/apis/api/v1/users");

                httpWebRequest.Headers[HttpRequestHeader.Authorization] = "Bearer " + accessToken;
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                UserObject user = ReadUserObject();

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = JsonConvert.SerializeObject(user);

                    streamWriter.Write(json);
                    streamWriter.Flush();
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var responseText = streamReader.ReadToEnd();
                    Console.WriteLine(responseText);
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void GetUsers(string accessToken)
        {
            try
            {
                Console.WriteLine("Get Users");

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(env + "/apis/api/v1/users");
                httpWebRequest.Headers[HttpRequestHeader.Authorization] = "Bearer " + accessToken;
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.Method = "GET";

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var responseText = streamReader.ReadToEnd();
                    Console.WriteLine(responseText); 
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public UserObject ReadUserObject()
        {
            UserObject user = new UserObject()
            {
                loginName = ConfigurationManager.AppSettings.Get("loginName"),
                userId = ConfigurationManager.AppSettings.Get("userId"),
                password = ConfigurationManager.AppSettings.Get("password"),
                domainId = int.Parse(ConfigurationManager.AppSettings.Get("domainId")),
                userTypeId = int.Parse(ConfigurationManager.AppSettings.Get("userTypeId")),
                usersecurityRoleId = int.Parse(ConfigurationManager.AppSettings.Get("usersecurityRoleId")),
                firstName = ConfigurationManager.AppSettings.Get("firstName"),
                lastName = ConfigurationManager.AppSettings.Get("lastName"),
                email = ConfigurationManager.AppSettings.Get("email"),
                languageId = int.Parse(ConfigurationManager.AppSettings.Get("languageId")),
                timeZoneId = int.Parse(ConfigurationManager.AppSettings.Get("timeZoneId")),
                status = ConfigurationManager.AppSettings.Get("status")
            };

            return user;
        }
    }
}
