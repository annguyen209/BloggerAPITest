using Google.Apis.Auth.OAuth2;
using Google.Apis.Blogger.v3;
using Google.Apis.Services;
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


        private UserCredential credential;
        private BloggerService service;


        public static byte[] ReadFile(string filePath)
        {
            byte[] buffer;
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            try
            {
                int length = (int)fileStream.Length;  // get file length
                buffer = new byte[length];            // create buffer
                int count;                            // actual number of bytes read
                int sum = 0;                          // total number of bytes read

                // read until Read method returns 0 (end of the stream has been reached)
                while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                    sum += count;  // sum is a buffer offset for next reading
            }
            finally
            {
                fileStream.Close();
            }
            return buffer;
        }

        private async Task AuthenticateAsync()
        {
            if (service != null)
                return;
            Stream stream = new MemoryStream(ReadFile("/App_Data/Blogger-b6c0eedf2b38.json"));
            credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                stream,
                new[] { BloggerService.Scope.Blogger },
                "user",
                CancellationToken.None);

            var initializer = new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "BloggerApp",
            };

            service = new BloggerService(initializer);
        }

        #region IBloggerRepository members

        public async Task<IEnumerable<Blog>> GetBlogsAsync()
        {
            await AuthenticateAsync();

            var list = await service.Blogs.ListByUser("self").ExecuteAsync();
            return from blog in list.Items
                   select new Blog
                   {
                       Id = blog.Id,
                       Name = blog.Name
                   };
        }

        public async Task<IEnumerable<Post>> GetPostsAsync(string blogId)
        {
            await AuthenticateAsync();
            var list = await service.Posts.List(blogId).ExecuteAsync();
            return from post in list.Items
                   select new Post
                   {
                       Title = post.Title,
                       Content = post.Content
                   };
        }

        #endregion
    }

}