using BloggerAPI.Helper;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Web;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AngleSharp;


namespace BloggerAPI
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected async void RunImportFromCSAuthor(List<string> label)
        {
            BloggerRepository repo = new BloggerRepository();
            try
            {
                repo.Authenticate();
                var list = await AnalyzeCSAuthor(label);
                List<string> listURL = new List<string>();
                foreach (var p in list)
                {
                    p.Updated = DateTime.Now.AddMinutes(new Random().Next(10));
                    p.Published = p.Updated.Value.AddMinutes(new Random().Next(10));
                    string rs = await repo.AddPostToBlogAsync(p);
                    listURL.Add(rs);
                }
                Result.Controls.Add(new HtmlGenericControl() { InnerText = "Post URL : <br/>" + string.Join("<br />", listURL) });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected async void RunImportFromCSAuthorWithCheckExist(List<string> label)
        {
            BloggerRepository repo = new BloggerRepository();
            try
            {
                repo.Authenticate();
                var list = await AnalyzeCSAuthor(label);
                List<string> listURL = new List<string>();
                var listPost = await repo.GetPostByLabel(label.FirstOrDefault());
                foreach (var p in list)
                {
                    var exist = listPost.Where(pa => pa.Title.Equals(p.Title)).FirstOrDefault();
                    if (exist == null)
                    {
                        p.Updated = DateTime.Now.AddMinutes(new Random().Next(10));
                        p.Published = p.Updated.Value.AddMinutes(new Random().Next(10));
                        string rs = await repo.AddPostToBlogAsync(p);
                        listURL.Add(rs);
                    }
                }
                Result.Controls.Add(new HtmlGenericControl() { InnerText = "Post URL : <br/>" + string.Join("<br />", listURL) });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected async Task<IEnumerable<Google.Apis.Blogger.v3.Data.Post>> AnalyzeCSAuthor(List<string> label)
        {
            try
            {
                List<Google.Apis.Blogger.v3.Data.Post> listPosts = new List<Google.Apis.Blogger.v3.Data.Post>();
                var url = "http://www.cssauthor.com/blogger-templates-2015/";
                var config = Configuration.Default.WithDefaultLoader();
                var address = url;
                var document = await BrowsingContext.New(config).OpenAsync(address);
                var root = document.QuerySelector("div.post-content");
                foreach (var item in root.QuerySelectorAll("h3.new"))
                {
                    Google.Apis.Blogger.v3.Data.Post post = new Google.Apis.Blogger.v3.Data.Post();
                    post.Title = item.TextContent;

                    post.Labels = label;

                    var nextSib = item.NextSibling; //<1st p>
                    while (nextSib != null && nextSib.NodeName.ToLower().Equals("p"))
                    {
                        var html = nextSib.ToHtml();

                        if (nextSib.NextSibling != null && nextSib.NextSibling.NodeName.ToLower().Equals("h3"))
                        {
                            html = "<div class='button-blogger-theme'>" + html + "</div>";
                        }
                        post.Content += html;
                        if (nextSib.NextSibling.NodeName.ToLower().Equals("ul"))
                        {
                            break;
                        }
                        nextSib = nextSib.NextSibling;
                    }
                    //post.Content = item.NextSibling.ToHtml() + item.NextSibling.NextSibling.ToHtml()
                    //               + "<div class='button-blogger-theme'>" + item.NextSibling.NextSibling.NextSibling.ToHtml() + "</div>";
                    listPosts.Add(post);
                }

                return listPosts;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected async void UpdateTimeOfAllPostByLabel(string label)
        {
            BloggerRepository repo = new BloggerRepository();
            repo.Authenticate();
            var listPost = await repo.GetPostByLabel(label);
            List<string> listURL = new List<string>();
            foreach (var p in listPost)
            {
                p.Updated = DateTime.Now.AddMinutes(new Random().Next(10));
                p.Published = p.Updated.Value.AddMinutes(new Random().Next(10));
                var rs = await repo.UpdatePostToBlogAsync(p);
                listURL.Add(rs);
            }
            Result.Controls.Add(new HtmlGenericControl() { InnerText = "Post URL : <br/>" + string.Join("<br />", listURL) });
                        
        }

        protected async void AddLabelForNonLabelPost(string label)
        {
            var repo = new BloggerRepository();
            repo.Authenticate();
            var listPost = await repo.GetPostNoneLabel();
            var listURL = new List<string>();
            foreach (var p in listPost)
            {
                p.Labels = new List<string> { label };
                var rs = await repo.UpdatePostToBlogAsync(p);
                listURL.Add(rs);
            }
            Result.Controls.Add(new HtmlGenericControl() { InnerText = "Post URL : <br/>" + string.Join("<br />", listURL) });
                        
        }

        protected async void GSharePublishedPostsByTime(DateTime from, DateTime to)
        {
            var repo = new BloggerRepository();
            repo.Authenticate();
            var listPost = await repo.GetPostByPublishedTime(from, to);
            var listURL = new List<string>();
            string gurl = "https://plus.google.com/share?url=";
            foreach (var p in listPost)
            {
                string script = string.Format("window.open('{0}');", gurl + p.Url);
                Page.ClientScript.RegisterStartupScript(this.GetType(),
                                        "newPage" + Guid.NewGuid(), script, true);
                
            }
        }

        protected async Task<Google.Apis.Blogger.v3.Data.Post> AnalyzeGeneralSite(List<string> label, string url, string rootNodeID, string titleNode,  string bodyNode)
        {
            try
            {
                var config = Configuration.Default.WithDefaultLoader();
                var address = url;
                var document = await BrowsingContext.New(config).OpenAsync(address);
                var root = document.QuerySelector(rootNodeID);
                var title = root.QuerySelector(titleNode);
                var postContent = root.QuerySelector(bodyNode);
                if (title != null && postContent != null)
                {
                    Google.Apis.Blogger.v3.Data.Post post = new Google.Apis.Blogger.v3.Data.Post();
                    post.Title = title.TextContent;
                    post.Content = postContent.InnerHtml;
                    post.Updated = DateTime.Now;
                    post.Published = post.Updated.Value.AddMinutes(1);
                    post.Labels = label;
                    return post;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected async void RunImportFromGeneralSIte(List<string> label, string url, string rootNodeID, string titleNode, string bodyNode)
        {
            BloggerRepository repo = new BloggerRepository();
            try
            {
                repo.Authenticate();
                var post = await AnalyzeGeneralSite(label,url, rootNodeID,titleNode,bodyNode);
                if (post != null)
                {
                    string rs = await repo.AddPostToBlogAsync(post);
                    Result.Controls.Add(new HtmlGenericControl() { InnerText = "Post URL : <br/>" + rs });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected async void btnRunAction_Click(object sender, EventArgs e)
        {
            try
            {
                string action = ddlAction.SelectedValue;

                switch (action)
                {
                    case "Generate Post General Site":
                        var label = new List<string>();
                        label.Add("Blogger Theme");
                        string url = "http://helplogger.blogspot.com/2014/10/6-custom-search-boxes--for-blogger.html";
                        string rootNodeID = "div#main-content";
                        string titleNode = "div.post h1"; 
                        string bodyNode = "div.post-body";
                        RunImportFromGeneralSIte(label, url, rootNodeID, titleNode, bodyNode);
                        break;
                    case "Generate Post CSAuthor":
                        label = new List<string>();
                        label.Add("Blogger Theme");
                        RunImportFromCSAuthor(label);
                        break;
                    case "Generate Post CSAuthor With Checking Existence":
                        label = new List<string>();
                        label.Add("Blogger Theme");
                        RunImportFromCSAuthorWithCheckExist(label);
                        break;
                    case "Update Time All Post By Label":
                        UpdateTimeOfAllPostByLabel("Blogger Theme");
                        break;
                    case "Add Label For Non Label Post":
                        AddLabelForNonLabelPost("Technical Sharing");
                        break;
                    case "Gplus Share All Posts Today":
                        GSharePublishedPostsByTime(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1));
                        break;
                    default: break;
                }
            }
            catch (Exception ex)
            {
                Result.Controls.Add(new HtmlGenericControl() { InnerText = "Exception: " + "<h3>" + ex.Message + "</h3><br/>" + ex.StackTrace });
            }
        }
    }
}