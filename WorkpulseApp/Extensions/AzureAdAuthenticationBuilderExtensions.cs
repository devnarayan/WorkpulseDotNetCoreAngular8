using CORTNE.Extensions;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using TestGraphApi.Models;

namespace Microsoft.AspNetCore.Authentication
{
    public static class AzureAdAuthenticationBuilderExtensions
    {
        private static readonly HttpClient ClientSingle = new HttpClient();
        private const string EndPoint = "https://graph.microsoft.com/v1.0";

        public static AuthenticationBuilder AddAzureAd(this AuthenticationBuilder builder)
            => builder.AddAzureAd(_ => { });

        public static AuthenticationBuilder AddAzureAd(this AuthenticationBuilder builder, Action<AzureAdOptions> configureOptions)
        {
            builder.Services.Configure(configureOptions);
            builder.Services.AddSingleton<IConfigureOptions<OpenIdConnectOptions>, ConfigureAzureOptions>();
            builder.AddOpenIdConnect();
            return builder;
        }

        private class ConfigureAzureOptions : IConfigureNamedOptions<OpenIdConnectOptions>
        {
            private readonly AzureAdOptions _azureOptions;

            public ConfigureAzureOptions(IOptions<AzureAdOptions> azureOptions)
            {
                _azureOptions = azureOptions.Value;
            }

            public void Configure(string name, OpenIdConnectOptions options)
            {
                // Set which roles/groups that we wish to check for
                var roles = new List<string> { "MMBGS" };
                GetRoleClaims(name, options, _azureOptions, roles);
            }

            public void Configure(OpenIdConnectOptions options)
            {
                Configure(Options.DefaultName, options);
            }

            private void GetRoleClaims(string name, OpenIdConnectOptions options, AzureAdOptions azureOptions, List<string> roles)
            {
                options.ClientId = azureOptions.ClientId;
                options.ClientSecret = azureOptions.ClientSecret;
                options.Resource = "https://graph.microsoft.com/";
                options.Authority = $"{azureOptions.Instance}{azureOptions.TenantId}";
                options.UseTokenLifetime = true;
                options.CallbackPath = azureOptions.CallbackPath;
                options.RequireHttpsMetadata = false;
                options.ResponseType = OpenIdConnectResponseType.CodeIdToken;

                options.Events = new OpenIdConnectEvents
                {
                    OnAuthorizationCodeReceived = async context =>
                    {
                        var request = context.HttpContext.Request;
                        var currentUri = UriHelper.BuildAbsolute(request.Scheme, request.Host, request.PathBase, request.Path);
                        var credential = new ClientCredential(context.Options.ClientId, context.Options.ClientSecret);

                        var distributedCache = context.HttpContext.RequestServices.GetRequiredService<IDistributedCache>();

                        var userId = context.Principal.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

                        var cache = new AdalDistributedTokenCache(distributedCache, userId);

                        var authContext = new AuthenticationContext(context.Options.Authority, cache);

                        var result = await authContext.AcquireTokenByAuthorizationCodeAsync(
                            context.ProtocolMessage.Code, new Uri(currentUri), credential, context.Options.Resource);

                        //Add AccessToken to claim
                        var claim = new Claim(ClaimTypes.MobilePhone, result.AccessToken);
                        ((ClaimsIdentity)context.Principal.Identity).AddClaim(claim);

                        var claim2 = new Claim("accesstoken", result.AccessToken);
                        // ((ClaimsIdentity)context.Principal.Identity).AddClaim(claim2);

                        SetRoleClaims(result.AccessToken, context, roles);

                        context.HandleCodeRedemption(result.AccessToken, result.IdToken);

                    }
                };
            }

            private static void SetRoleClaims(string accessToken, AuthorizationCodeReceivedContext ctx, ICollection<string> roles)
            {
                var res = QueryGraph("/me/memberOf", accessToken);
                var data = res.Content.ReadAsStringAsync().Result;
                var json = JsonConvert.DeserializeObject<MemberOf>(data);

                //MyProfile(accessToken, "");
                //UserProfile(accessToken, "kiran.dusi@flhealth.gov");
                //UserProfile(accessToken, "Tom.Mitas@flhealth.com");
                foreach (var item in json.Value)
                {
                    //// If found add it to our claims
                    //if (roles.Contains(item.))
                    //{
                    //    var claim = new Claim(ClaimTypes.Role, item.DisplayName);
                    //    ((ClaimsIdentity)ctx.Principal.Identity).AddClaim(claim);
                    //}

                    //// Add any groups that our group is part of
                    //var parentGroups = GetGroupMemberOf(accessToken, item.Id);
                    //if (parentGroups?.Value == null) continue;
                    //foreach (var val in parentGroups.Value)
                    //{
                    //    if (!roles.Contains(val.DisplayName)) continue;
                    //    var innerClaim = new Claim(ClaimTypes.Role, val.DisplayName);
                    //    ((ClaimsIdentity)ctx.Principal.Identity).AddClaim(innerClaim);
                    //}
                }
            }

            private static AllGroups GetGroupMemberOf(string accessToken, string id)
            {
                var res = QueryGraph($"/groups/{id}/memberOf", accessToken);

                var data = res.Content.ReadAsStringAsync().Result;
                var json = JsonConvert.DeserializeObject<AllGroups>(data);

                return json;
            }
            private static AllGroups MyProfile(string accessToken, string id)
            {
                var res = QueryGraph($"/me", accessToken);

                var data = res.Content.ReadAsStringAsync().Result;
                var json = JsonConvert.DeserializeObject<AllGroups>(data);

                return json;
            }
            private static AllGroups UserProfile(string accessToken, string id)
            {
                var res = QueryGraph($"/users/{id}", accessToken);

                var data = res.Content.ReadAsStringAsync().Result;
                var json = JsonConvert.DeserializeObject<AllGroups>(data);

                return json;
            }

            private static HttpResponseMessage QueryGraph(string relativeUrl, string accessToken)
            {
                var req = new HttpRequestMessage(HttpMethod.Get, EndPoint + relativeUrl);
                req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                return ClientSingle.SendAsync(req).Result;
            }

        }
    }
}
