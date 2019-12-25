/* 
*  Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. 
*  See LICENSE in the source repository root for complete license information. 
*/
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Identity.Client;

namespace CORTNE.Helpers
{


    public class GraphSdkHelper : IGraphSdkHelper
    {
        private readonly IGraphAuthProvider _authProvider;
        private GraphServiceClient _graphClient;

        public GraphSdkHelper(IGraphAuthProvider authProvider)
        {
            _authProvider = authProvider;
        }

        // Get an authenticated Microsoft Graph Service client.
        public GraphServiceClient GetAuthenticatedClient(ClaimsIdentity userIdentity)
        {
            _graphClient = new GraphServiceClient(new DelegateAuthenticationProvider(
                async requestMessage =>
                {
                        // Get user's id for token cache.
                        var identifier = userIdentity.FindFirst(Startup.ObjectIdentifierType)?.Value + "." + userIdentity.FindFirst(Startup.TenantIdType)?.Value;

                        // Passing tenant ID to the sample auth provider to use as a cache key
                        var accessToken = await _authProvider.GetUserAccessTokenAsync(identifier);

                        // Append the access token to the request
                        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                        // This header identifies the sample in the Microsoft Graph service. If extracting this code for your project please remove.
                        requestMessage.Headers.Add("CORTNE", "aspnetcore-webapp-openidconnect-v2");
                }));

            return _graphClient;
        }

        //public async Task<string> GetTokenForApplication()
        //{
        //    string tenantID = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
        //    string userObjectID = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

        //    ClientCredential clientcred = new ClientCredential(clientId, appKey);
        //    AuthenticationContext authenticationContext = new AuthenticationContext("" + tenantID, new NaiveSessionCache(userObjectID));
        //    AuthenticationResult authenticationResult = await authenticationContext.AcquireTokenAsync("https://graph.windows.net", clientcred);
        //    //AuthenticationResult authenticationResult = await authenticationContext.AcquireTokenSilentAsync(graphResourceID, clientcred, new UserIdentifier(userObjectID, UserIdentifierType.UniqueId));
        //    return authenticationResult.AccessToken;
        //}
    }
    public interface IGraphSdkHelper
    {
        GraphServiceClient GetAuthenticatedClient(ClaimsIdentity userIdentity);
    }
}
