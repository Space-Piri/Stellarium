using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stellarium.Models;

namespace Stellarium.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        ApplicationContext Context;
        public List<PublicationV2> Publications = new List<PublicationV2>();
        public User CurrentUser;
        public List<BookMark> UserBookmarks = new List<BookMark>();
        public string Section;
        public string Search;
        public int I = 0;
        public string From;
        public string To;
        public string Category;
        public string Sort;
        public int Show;
        public IndexModel(ILogger<IndexModel> logger, ApplicationContext db)
        {
            _logger = logger;
            Context = db;
        }

        public async void OnGet(string? section, string? search, DateTime? from, DateTime? to, string? category, string? sort, int? show)
        {
            //Context.Publications.Remove(Context.Publications.First(p => p.Id == 1));
            //Context.SaveChanges();
            Section = section;
            Search = search;
            if (from != null)
            {
                From = from.Value.ToString("yyyy-MM-dd");
            }
            if (to != null)
            {
                To = to.Value.ToString("yyyy-MM-dd");
            }
            if (category != null)
            {
                Category = category;
            }
            if (sort != null)
            {
                Sort = sort;
            }
            if (show != null)
            {
                Show = show.Value;
            }
            await GetPublications();
        }

        async Task GetPublications()
        {
            string useremail = Request.Cookies["useremail"];
            if (useremail != null)
            {
                CurrentUser = Context.Users.FirstOrDefault(x => x.Email == useremail);
                CurrentUser.LastOnline = DateTime.Now;
                Context.Users.Update(CurrentUser);
                Context.SaveChanges();
                var userid = Context.Users.FirstOrDefault(u => u.Email == useremail).Id.ToString();
                Response.Cookies.Append("userid", userid);
                if (CurrentUser.IsModerator == true)
                {
                    Response.Cookies.Append("ismoderator", "true");
                }
                else
                {
                    Response.Cookies.Append("ismoderator", "false");
                }
            }

            var visitorId = Request.Cookies["visitorId"];
            if (visitorId == "null" || visitorId == null)
            {
                Context.Visitors.Add(new Visitor());
                Context.SaveChanges();
                visitorId = Context.Visitors.OrderBy(v => v.Id).Last().Id + "";
                Response.Cookies.Append("visitorId", visitorId);
            }

            if (CurrentUser != null)
            {
                UserBookmarks = Context.Bookmarks.Where(b => b.UserId == CurrentUser.Id).ToList();
            }
            
            try
            {
                var username = System.Web.HttpUtility.UrlDecode(Request.Cookies["username"]);
                User olduser = Context.Users.FirstOrDefault(u => u.Email == useremail);
                olduser.Name = username;
                Context.Update(olduser);
                Context.SaveChanges();
            }
            catch
            {

            }
            RefreshOutput();
        }

        public void RefreshOutput()
        {
            List<Category> Categories = new List<Category>();
            Publications.Clear();
            var users = Context.Users.ToList();
            if (Search != null)
            {
                foreach (var publication in Context.Publications.Where(p => p.Title.Contains(Search) && p.IsPublicated == true).OrderByDescending(p => p.DateTime))
                {
                    var user = users.FirstOrDefault(u => u.Id == publication.UserId);
                    var views = Context.Views.Where(v => v.PublicationId == publication.Id).Count();
                    var comments = Context.Comments.Where(c => c.PublicationId == publication.Id).Count();
                    var publicationCategories = Context.PublicationCategories.Where(c => c.PublicationId == publication.Id).ToList();
                    foreach (var publicationCategory in publicationCategories)
                    {
                        Categories.Add(Context.Categories.FirstOrDefault(c => c.Id == publicationCategory.CategoryId));
                    }
                    if (Convert.ToDateTime(From) != DateTime.MinValue)
                    {
                        if (publication.DateTime < Convert.ToDateTime(From))
                        {
                            continue;
                        }
                    }
                    if (Convert.ToDateTime(To) != DateTime.MinValue)
                    {
                        if (publication.DateTime > Convert.ToDateTime(To).AddDays(1))
                        {
                            continue;
                        }
                    }
                    if (Category != null)
                    {
                        if (Categories.FirstOrDefault(c => c.Name == Category) == null)
                        {
                            continue;
                        }
                    }
                    Publications.Add(new PublicationV2(publication, user, views, comments, Categories.ToList()));
                    Categories.Clear();
                }
            }
            else
            {
                switch (Section)
                {
                    case null:
                        foreach (var publication in Context.Publications.Where(p => p.IsPublicated == true).OrderByDescending(p => p.DateTime))
                        {
                            var user = users.FirstOrDefault(u => u.Id == publication.UserId);
                            var views = Context.Views.Where(v => v.PublicationId == publication.Id).Count();
                            var comments = Context.Comments.Where(c => c.PublicationId == publication.Id).Count();
                            var publicationCategories = Context.PublicationCategories.Where(c => c.PublicationId == publication.Id).ToList();
                            foreach (var publicationCategory in publicationCategories)
                            {
                                Categories.Add(Context.Categories.FirstOrDefault(c => c.Id == publicationCategory.CategoryId));
                            }
                            Publications.Add(new PublicationV2(publication, user, views, comments, Categories.ToList()));
                            Categories.Clear();
                        }
                        break;

                    case "Bookmarks":
                        if (UserBookmarks == null)
                        {
                            break;
                        }
                        foreach (var publication in Context.Publications.Where(p => p.IsPublicated == true).OrderByDescending(p => p.DateTime))
                        {
                            if (UserBookmarks.FirstOrDefault(b => b.PublicationId == publication.Id) != null)
                            {
                                var user = users.FirstOrDefault(u => u.Id == publication.UserId);
                                var views = Context.Views.Where(v => v.PublicationId == publication.Id).Count();
                                var comments = Context.Comments.Where(c => c.PublicationId == publication.Id).Count();
                                var publicationCategories = Context.PublicationCategories.Where(c => c.PublicationId == publication.Id).ToList();
                                foreach (var publicationCategory in publicationCategories)
                                {
                                    Categories.Add(Context.Categories.FirstOrDefault(c => c.Id == publicationCategory.CategoryId));
                                }
                                Publications.Add(new PublicationV2(publication, user, views, comments, Categories.ToList()));
                                Categories.Clear();
                            }
                        }
                        break;

                    case "Custom":
                        foreach (var publication in Context.Publications.Where(p => p.IsPublicated == true).OrderByDescending(p => p.DateTime))
                        {
                            var user = users.FirstOrDefault(u => u.Id == publication.UserId);
                            var views = Context.Views.Where(v => v.PublicationId == publication.Id).Count();
                            var comments = Context.Comments.Where(c => c.PublicationId == publication.Id).Count();
                            var publicationCategories = Context.PublicationCategories.Where(c => c.PublicationId == publication.Id).ToList();
                            foreach (var publicationCategory in publicationCategories)
                            {
                                Categories.Add(Context.Categories.FirstOrDefault(c => c.Id == publicationCategory.CategoryId));
                            }
                            if (Category != null)
                            {
                                if (Categories.FirstOrDefault(c => c.Name == Category) == null)
                                {
                                    continue;
                                }
                            }
                            Publications.Add(new PublicationV2(publication, user, views, comments, Categories.ToList()));
                            Categories.Clear();
                        }

                        switch (Sort)
                        {
                            case "datedec":
                                break;
                            case "dateasc":
                                Publications = Publications.OrderBy(p => p.Publication.DateTime).ToList();
                                break;
                            case "viewsdes":
                                Publications = Publications.OrderByDescending(p => p.Views).ToList();
                                break;
                            case "viewsasc":
                                Publications = Publications.OrderBy(p => p.Views).ToList();
                                break;
                        }
                        break;

                    case "Новости": case "Статьи": case "Экзопланеты": case "Тест": case "Наука": case "Технологии":
                        foreach (var publication in Context.Publications.Where(p => p.IsPublicated == true).OrderByDescending(p => p.DateTime))
                        {
                            var user = users.FirstOrDefault(u => u.Id == publication.UserId);
                            var views = Context.Views.Where(v => v.PublicationId == publication.Id).Count();
                            var comments = Context.Comments.Where(c => c.PublicationId == publication.Id).Count();
                            var publicationCategories = Context.PublicationCategories.Where(c => c.PublicationId == publication.Id).ToList();
                            foreach (var publicationCategory in publicationCategories)
                            {
                                Categories.Add(Context.Categories.FirstOrDefault(c => c.Id == publicationCategory.CategoryId));
                            }
                            if (Categories.FirstOrDefault(c => c.Name == Section) != null)
                            {
                                Publications.Add(new PublicationV2(publication, user, views, comments, Categories.ToList()));
                            }
                            Categories.Clear();
                        }
                        break;
                }
            }
        }

        public async void OnPostAddBookMark(int id, string? section, string? search, DateTime? from, DateTime? to, string? category, string? sort, int? show)
        {
            Section = section;
            Search = search;
            if (from != null)
            {
                From = from.Value.ToString("yyyy-MM-dd");
            }
            if (to != null)
            {
                To = to.Value.ToString("yyyy-MM-dd");
            }
            if (category != null)
            {
                Category = category;
            }
            if (sort != null)
            {
                Sort = sort;
            }
            if (show != null)
            {
                Show = show.Value;
            }
            try
            {
                await GetPublications();
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

        public async void OnPostDeleteBookMark(int id, string? section, string? search, DateTime? from, DateTime? to, string? category, string? sort, int? show)
        {
            Section = section;
            Search = search;
            if (from != null)
            {
                From = from.Value.ToString("yyyy-MM-dd");
            }
            if (to != null)
            {
                To = to.Value.ToString("yyyy-MM-dd");
            }
            if (category != null)
            {
                Category = category;
            }
            if (sort != null)
            {
                Sort = sort;
            }
            if (show != null)
            {
                Show = show.Value;
            }
            try
            {
                await GetPublications();
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