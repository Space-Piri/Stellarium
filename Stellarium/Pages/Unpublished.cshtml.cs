using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stellarium.Models;

namespace Stellarium.Pages
{
    public class UnpublishedModel : PageModel
    {
        ApplicationContext Context;
        public List<PublicationV2> Publications = new List<PublicationV2>();

        public UnpublishedModel(ApplicationContext db)
        {
            Context = db;
        }

        public void OnGet()
        {
            Publications.Clear();
            var users = Context.Users.ToList();
            List<Category> Categories = new List<Category>();
            foreach (var publication in Context.Publications.Where(p => p.IsPublicated == false).OrderByDescending(p => p.DateTime))
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
    }
}
