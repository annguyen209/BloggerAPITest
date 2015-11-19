using Google.Apis.Auth.OAuth2;
using Google.Apis.Blogger.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace BloggerAPI.Helper
{
    public class BloggerRepository : IBloggerRepository
    {
        /// <summary>The blogger repository implementation which works the same for Windows and Windows Phone.</summary>

        public string clientId = BloggerAPI.Properties.Settings.Default.GAPIClientId;//From Google Developer console https://console.developers.google.com
        public string clientSecret = Properties.Settings.Default.GAPIClientSecret;//From Google Developer console https://console.developers.google.com
        public string userName = Properties.Settings.Default.Username;//  A string used to identify a user.
        public string blogId = Properties.Settings.Default.BlogId;
        public string[] scopes = new string[] { BloggerService.Scope.Blogger };
        public UserCredential credential;
        public BloggerService service;


        public void Authenticate()
        {
            // here is where we Request the user to give us access, or use the Refresh Token that was previously stored in %AppData%
            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets
            {
                ClientId = clientId,
                ClientSecret = clientSecret
            }, scopes, userName, CancellationToken.None).Result;
            var initializer = new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "BloggerApp",
            };

            service = new BloggerService(initializer);
        }

        public async Task<IEnumerable<Blog>> GetBlogsAsync()
        {
            var list = await service.Blogs.ListByUser("self").ExecuteAsync();
            return from blog in list.Items
                   select new Blog
                   {
                       Id = blog.Id,
                       Name = blog.Name
                   };
        }
        public async Task<string> AddPostToBlogAsync(Google.Apis.Blogger.v3.Data.Post post)
        {
            try
            {
                var insertReq = await service.Posts.Insert(post, blogId).ExecuteAsync();
                return insertReq.Url;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<string> UpdatePostToBlogAsync(Google.Apis.Blogger.v3.Data.Post post)
        {
            var updateReq = await service.Posts.Update(post, blogId, post.Id).ExecuteAsync();
            return updateReq.Url;
        }

        public async Task<IEnumerable<Google.Apis.Blogger.v3.Data.Post>> GetPostByLabel(string label)
        {
            try
            {
                var req = service.Posts.List(blogId);
                var reqRs = await req.ExecuteAsync();
                List<Google.Apis.Blogger.v3.Data.Post> list = new List<Google.Apis.Blogger.v3.Data.Post>();
                list = reqRs.Items.Where(p => p.Labels != null && p.Labels.FirstOrDefault().Equals(label)).ToList();
                while (reqRs.NextPageToken != null)
                {
                    req.PageToken = reqRs.NextPageToken;
                    reqRs = await req.ExecuteAsync();
                    var rs = reqRs.Items.Where(p => p.Labels != null && p.Labels.FirstOrDefault().Equals(label)).ToList();
                    if (rs != null)
                    {
                        list.AddRange(rs);
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<Google.Apis.Blogger.v3.Data.Post>> GetPostNoneLabel()
        {
            try
            {
                var req = service.Posts.List(blogId);
                var reqRs = await req.ExecuteAsync();
                List<Google.Apis.Blogger.v3.Data.Post> list = new List<Google.Apis.Blogger.v3.Data.Post>();
                list = reqRs.Items.Where(p => p.Labels == null).ToList();
                while (reqRs.NextPageToken != null)
                {
                    req.PageToken = reqRs.NextPageToken;
                    reqRs = await req.ExecuteAsync();
                    var rs = reqRs.Items.Where(p => p.Labels == null).ToList();
                    if (rs != null)
                    {
                        list.AddRange(rs);
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

}