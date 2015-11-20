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

        

        protected async void RunImportFromSite(string url, List<string> label)
        {
            BloggerRepository repo = new BloggerRepository();
            try
            {
                repo.Authenticate();
                var list = await AnalyzeWeb(url, label);
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
                Result.Controls.Add(new HtmlGenericControl() { InnerText = "Exception :)" + ex.StackTrace });
            }
        }

        protected async void RunImportFromSiteWithCheckExist(string url, List<string> label)
        {
            BloggerRepository repo = new BloggerRepository();
            try
            {
                repo.Authenticate();
                var list = await AnalyzeWeb(url, label);
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
                Result.Controls.Add(new HtmlGenericControl() { InnerText = "Exception: " + "<h3>" + ex.Message + "</h3><br/>" + ex.StackTrace });
            }
        }
        protected async Task<IEnumerable<Google.Apis.Blogger.v3.Data.Post>> AnalyzeWeb(string url, List<string> label)
        {
            try
            {
                List<Google.Apis.Blogger.v3.Data.Post> listPosts = new List<Google.Apis.Blogger.v3.Data.Post>();

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

        

        protected async void btnRunAction_Click(object sender, EventArgs e)
        {
            try
            {
                string action = ddlAction.SelectedValue;

                switch (action)
                {
                    case "Generate Post":
                        var address = "http://www.cssauthor.com/blogger-templates-2015/";
                        var label = new List<string>();
                        label.Add("Blogger Theme");
                        RunImportFromSite(address, label);
                        break;
                    case "Generate Post With Checking Existence":
                        address = "http://www.cssauthor.com/blogger-templates-2015/";
                        label = new List<string>();
                        label.Add("Blogger Theme");
                        RunImportFromSiteWithCheckExist(address, label);
                        break;
                    case "Edit Time Post":

                        BloggerRepository repo = new BloggerRepository();
                        repo.Authenticate();
                        var listPost = await repo.GetPostByLabel("Blogger Theme");
                        List<string> listURL = new List<string>();
                        foreach (var p in listPost)
                        {
                            p.Updated = DateTime.Now.AddMinutes(new Random().Next(30));
                            p.Published = p.Updated.Value.AddMinutes(new Random().Next(30));
                            var rs = await repo.UpdatePostToBlogAsync(p);
                            listURL.Add(rs);
                        }
                        Result.Controls.Add(new HtmlGenericControl() { InnerText = "Post URL : <br/>" + string.Join("<br />", listURL) });
                        break;
                    case "Add Label For Non Label Post":
                        repo = new BloggerRepository();
                        repo.Authenticate();
                        listPost = await repo.GetPostNoneLabel();
                        listURL = new List<string>();
                        foreach (var p in listPost)
                        {
                            p.Labels = new List<string> { "Technical Sharing" };
                            var rs = await repo.UpdatePostToBlogAsync(p);
                            listURL.Add(rs);
                        }
                        Result.Controls.Add(new HtmlGenericControl() { InnerText = "Post URL : <br/>" + string.Join("<br />", listURL) });
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