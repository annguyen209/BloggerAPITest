using BloggerAPI.Helper;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Web;
using Google.Apis.Blogger.v3;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace BloggerAPI.Helper
{
    public class APISupporter : IAPISupporter
    {
        protected static string
                    CLIENT_ID = BloggerAPI.Properties.Settings.Default.GAPIClientId,
                    CLIENT_SECRET = Properties.Settings.Default.GAPIClientSecret,
                    API_KEY = Properties.Settings.Default.GAPIAPIKey,
                    APPLICATION_NAME = Properties.Settings.Default.GAPIApplicationName,
                     BLOG_ID = Properties.Settings.Default.BlogId;

        protected BloggerService BloggerService;
        protected static GoogleAuthorizationCodeFlow
            CodeFlow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = CLIENT_ID,
                    ClientSecret = CLIENT_SECRET
                },
                Scopes = new[] { BloggerService.Scope.Blogger },
            });
        public object Authorize(HttpRequest Request, string accountName)
        {
            try
            {
                var uri = Request.Url.ToString();
                var code = Request["code"];
                var error = Request["error"];

                if (!string.IsNullOrEmpty(error))
                    if (error == "access_denied")
                        return new UserRejectException();
                    else
                        return new UnknownException(error);

                BloggerService = null;

                if (code != null)
                {
                    string redirectUri = uri.Substring(0, uri.IndexOf("?"));
                    var token = CodeFlow.ExchangeCodeForTokenAsync(accountName, code, redirectUri, CancellationToken.None).Result;
                    string state = Request["state"];
                    var result = AuthWebUtility.ExtracRedirectFromState(CodeFlow.DataStore, accountName, state);                    
                    return result;
                }
                else
                {
                    string redirectUri = uri;
                    string state = "ostate_";// Guid.NewGuid().ToString("N");
                    var result = new AuthorizationCodeWebApp(CodeFlow, redirectUri, state).AuthorizeAsync(accountName, CancellationToken.None).Result;
                    if (result.RedirectUri != null)
                    {
                        return result;
                    }
                    else
                    {
                        BloggerService = new BloggerService(new BaseClientService.Initializer()
                        {
                            HttpClientInitializer = result.Credential,
                            ApplicationName = APPLICATION_NAME
                        });
                        // alright
                        return "OK";
                    }
                }
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
        public object GetBlog()
        {
            try
            {
                BlogsResource.GetRequest req = BloggerService.Blogs.Get(BLOG_ID);
                return req.Execute();
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }

}