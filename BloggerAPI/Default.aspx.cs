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
namespace BloggerAPI
{
    public partial class _Default : Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            BloggerRepository repo = new BloggerRepository();
            //Task<IEnumerable<Blog>> list = repo.GetBlogsAsync();
            var list = await repo.GetBlogsAsync();
            foreach (var b in list)
            {
                Result.Controls.Add(new HtmlGenericControl() { InnerText = b.Id + " " + b.Name });
            }
            //var code = Request["code"];
            //var error = Request["error"];
            //var action = Request["action"];
            //var newUserId = "annguyen209@gmail.com";
            //if (newUserId != null)
            //{
            //    Session["GapiUserId"] = newUserId;
            //}
            //if (newUserId != null || code != null || error != null || action != null)
            //{
            //    APISupporter sup = new APISupporter();
            //    var result = sup.Authorize(Request, Session["GapiUserId"] as string);
            //    if (result is AuthorizationCodeWebApp.AuthResult)
            //    {
            //        var authResult = result as AuthorizationCodeWebApp.AuthResult;
            //        Response.Redirect(authResult.RedirectUri);
            //    }
            //    else if (result is string && (result as string) == "OK")
            //    {
            //        Session["APISupporter"] = sup;
            //        //Response.Redirect("Page2.aspx");
                    
            //    }
            //    else if (result is Task<string>)
            //    {
            //        //var task = result as Task<string>;
            //        // Authorization was, now just need to refresh the page to log in again
            //        Response.Redirect(Request.Url.AbsolutePath + "?action=check");
            //    }
            //    else if (result is UserRejectException)
            //    {
            //        // User rejected the invitation
            //        RenderException(new Exception("User rejected the invitation", result as UserRejectException));
            //    }
            //    else if (result is Exception)
            //    {
            //        // Other error occurred when accessing the service Google Api
            //        RenderException(result as Exception);
            //    }
            //    else
            //    {
            //        // unexpected happened
            //        RenderException(new Exception("unexpected happened", new Exception("Authorize unexpected happened")));
            //    }
            //}
        }

        protected async void btnRun_Click(object sender, EventArgs e)
        {
            //APISupporter sup = (APISupporter) Session["APISupporter"];
            //var rs = sup.GetBlog();
            //Result.Controls.Add(new HtmlGenericControl() { InnerText = "• " + rs.ToString() });

            BloggerRepository repo = new BloggerRepository();
            //Task<IEnumerable<Blog>> list = repo.GetBlogsAsync();
            var list = await repo.GetBlogsAsync();
            foreach (var b in list)
            {
                Result.Controls.Add(new HtmlGenericControl() { InnerText = b.Id + " " + b.Name });
            }
            
        }

        private void RenderException(Exception ex)
        {
            while (ex != null)
            {
                ErrorPan.Controls.Add(new HtmlGenericControl() { InnerText = "• " + ex.Message });
                ex = ex.InnerException;
            }
        }
       
    }
}