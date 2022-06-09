using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stellarium.Models;

namespace Stellarium.Pages
{
    public class ProfileModel : PageModel
    {
        public ApplicationContext Context;
        public User User;
        public string RegDate;
        public string LastOnline;
        public List<PublicationV2> Publications = new List<PublicationV2>();
        public User CurrentUser;
        public List<BookMark> UserBookmarks = new List<BookMark>();
        public ProfileModel(ApplicationContext db)
        {
            Context = db;
        }

        public void OnGet(int id)
        {
            try
            {
                User = Context.Users.FirstOrDefault(u => u.Id == id);
                RegDate = User.RegistrationDate.ToString("dd MMMM yyyy");
                LastOnline = Entity.OverDay(User.LastOnline);
            }
            catch 
            {
                return;
            }


            string useremail = Request.Cookies["useremail"];
            if (useremail != null)
            {
                CurrentUser = Context.Users.FirstOrDefault(x => x.Email == useremail);
                UserBookmarks = Context.Bookmarks.Where(b => b.UserId == CurrentUser.Id).ToList();
            }
            RefreshOutput();
        }

        void RefreshOutput()
        {
            Publications.Clear();
            foreach (var publication in Context.Publications.Where(p => p.UserId == User.Id && p.IsPublicated == true).OrderByDescending(p => p.DateTime))
            {
                var views = Context.Views.Where(v => v.PublicationId == publication.Id).Count();
                var comments = Context.Comments.Where(c => c.PublicationId == publication.Id).Count();
                var publicationCategories = Context.PublicationCategories.Where(c => c.PublicationId == publication.Id).ToList();
                List<Category> Categories = new List<Category>();
                foreach (var publicationCategory in publicationCategories)
                {
                    Categories.Add(Context.Categories.FirstOrDefault(c => c.Id == publicationCategory.CategoryId));
                }
                Publications.Add(new PublicationV2(publication, User, views, comments, Categories.ToList()));
            }
        }

        public async void OnPostAddBookMark(int userid, int id)
        {
            try
            {
                OnGet(userid);
                var newBookmark = new BookMark(0, CurrentUser.Id, id);
                if (!Context.Bookmarks.Contains(newBookmark))
                {
                    Context.Bookmarks.Add(newBookmark);
                    Context.SaveChanges();
                    UserBookmarks.Add(newBookmark);
                    RefreshOutput();
                }
            }
            catch
            {
            }
        }

        public async void OnPostDeleteBookMark(int userid, int id)
        {
            try
            {
                OnGet(userid);
                var bookmark = Context.Bookmarks.FirstOrDefault(b => b.UserId == CurrentUser.Id && b.PublicationId == id);
                Context.Bookmarks.Remove(bookmark);
                Context.SaveChanges();
                UserBookmarks.Remove(bookmark);
                RefreshOutput();
            }
            catch
            {
            }
        }
    }
}
