using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Stellarium.Models;
using System.Web;

namespace Stellarium.Pages
{
    public class AuthModel : PageModel
    {
        ApplicationContext Context;
        public string Username = "Гость";
        public AuthModel(ApplicationContext db)
        {
            Context = db;
        }
        public async void OnPostAuthorise()
        {
            var username = HttpUtility.UrlDecode(Request.Cookies["username"]);
            var userimg = Request.Cookies["userimg"];
            var useremail = Request.Cookies["useremail"];
            try
            {
                var user1 = Context.Users.FirstOrDefault(u => u.Email == useremail);
                if (user1 == null)
                {
                    User user = new User(0, username, useremail, userimg, 0, DateTime.Now, DateTime.Now, false);
                    Context.Users.Add(user);
                    Context.SaveChanges();
                    var userid = Context.Users.FirstOrDefault(u => u.Email == useremail).Id.ToString();
                    Response.Cookies.Append("userid", userid);
                }
                else
                {
                    User olduser = Context.Users.FirstOrDefault(u => u.Email == useremail);
                    olduser.Email = useremail;
                    olduser.Name = username;
                    olduser.Icon = userimg;
                    Context.Update(olduser);
                    Context.SaveChanges();
                    var userid = Context.Users.FirstOrDefault(u => u.Email == useremail).Id.ToString();
                    Response.Cookies.Append("userid", userid);
                }
            }
            catch
            {
            }

        }
    }
}
