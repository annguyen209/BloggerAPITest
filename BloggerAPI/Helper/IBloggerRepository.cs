using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BloggerAPI.Helper
{
        /// <summary>Blog data contact.</summary>
        public class Blog
        {
            /// <summary>Gets or sets the blog id.</summary>
            public string Id { get; set; }
            /// <summary>Gets or sets the blog name.</summary>
            public string Name { get; set; }
        }

        /// <summary>Post data contact.</summary>
        public class Post
        {
            /// <summary>Gets or sets the post title.</summary>
            public string Title { get; set; }
            /// <summary>Gets or sets the post content.</summary>
            public string Content { get; set; }
        }

        /// <summary>Blogger repository for retrieving blogs and posts.</summary>
        public interface IBloggerRepository
        {
            /// <summary>Gets all post for the specified blog.</summary>
            Task<IEnumerable<Post>> GetPostsAsync(string blogId);

            /// <summary>Get all user's blogs.</summary>
            Task<IEnumerable<Blog>> GetBlogsAsync();
        }
}