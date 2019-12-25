using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Identity.Client;
using Microsoft.Graph;
using Microsoft.Extensions.Configuration;


namespace CORTNE
{
    public class GraphAuthenticationProvider
    {
        private static GraphServiceClient _graphServiceClient;
        private static HttpClient _httpClient;

        private static IConfigurationRoot LoadAppSettings()
        {
            try
            {
                var config = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .Build();

                // Validate required settings
                if (string.IsNullOrEmpty(config["AzureAd:ClientId"]) ||
                string.IsNullOrEmpty(config["AzureAd:ClientSecret"]) ||
                string.IsNullOrEmpty(config["AzureAd:BaseUrl"]) ||
                string.IsNullOrEmpty(config["AzureAd:TenantId"]))
                {
                    return null;
                }

                return config;
            }
            catch (System.IO.FileNotFoundException)
            {
                return null;
            }
        }

        private static IAuthenticationProvider CreateAuthorizationProvider(IConfigurationRoot config)
        {
            var clientId = config["AzureAd:ClientId"];
            var clientSecret = config["AzureAd:ClientSecret"];
            var redirectUri = config["AzureAd:BaseUrl"];
            var authority = $"https://login.microsoftonline.com/{config["AzureAd:TenantId"]}/v2.0";

            List<string> scopes = new List<string>();
            scopes.Add("https://graph.microsoft.com/.default");

            var cca = new ConfidentialClientApplication(clientId, authority, redirectUri, new ClientCredential(clientSecret), null, null);
            return new MsalAuthenticationProvider(cca, scopes.ToArray());
        }
        private static GraphServiceClient GetAuthenticatedGraphClient(IConfigurationRoot config)
        {
            var authenticationProvider = CreateAuthorizationProvider(config);
            _graphServiceClient = new GraphServiceClient(authenticationProvider);
            return _graphServiceClient;
        }
        private static HttpClient GetAuthenticatedHTTPClient(IConfigurationRoot config)
        {
            var authenticationProvider = CreateAuthorizationProvider(config);
            _httpClient = new HttpClient(new AuthHandler(authenticationProvider, new HttpClientHandler()));
            return _httpClient;
        }

        public static void Initlize()
        {
            var config = LoadAppSettings();
            if (null == config)
            {
                Console.WriteLine("Missing or invalid appsettings.json file. Please see README.md for configuration instructions.");
                return;
            }

            //Query using Graph SDK (preferred when possible)
            GraphServiceClient graphClient = GetAuthenticatedGraphClient(config);
            List<QueryOption> options = new List<QueryOption>
            {
                new QueryOption("$top", "1")
            };

            var graphResult = graphClient.Users.Request(options).GetAsync().Result;
            Console.WriteLine("Graph SDK Result");
            Console.WriteLine(graphResult[0].DisplayName);

            var myInfo = graphClient.Me.Request().GetAsync().Result;


            //Direct query using HTTPClient (for beta endpoint calls or not available in Graph SDK)
            HttpClient httpClient = GetAuthenticatedHTTPClient(config);
            Uri Uri = new Uri("https://graph.microsoft.com/v1.0/kiran.dusi@flhealth.gov");
            var httpResult = httpClient.GetStringAsync(Uri).Result;

            Uri Uri2 = new Uri("https://graph.microsoft.com/v1.0/users/Sangeetha.Karthiraj@flhealth.gov");
            var httpResult2 = httpClient.GetStringAsync(Uri2).Result;

            Console.WriteLine("HTTP Result");
            Console.WriteLine(httpResult);
        }
    }
}
