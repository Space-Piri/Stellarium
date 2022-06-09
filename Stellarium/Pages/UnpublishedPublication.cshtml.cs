using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stellarium.Models;

namespace Stellarium.Pages
{
    public class UnpublishedPublicationModel : PageModel
    {
        public ApplicationContext Context;
        public PublicationV2 Publication;
        public List<CommentV2> Comments = new List<CommentV2>();
        public User User;
        public UnpublishedPublicationModel(ApplicationContext db)
        {
            Context = db;
        }

        public async void OnGet(int id)
        {
            GetPublication(id);
        }

        public void OnPostPublicate(int id)
        {
            GetPublication(id);
            Publication publication = Context.Publications.FirstOrDefault(p => p.Id == id);
            publication.IsPublicated = true;
            publication.DateTime = DateTime.Now;
            Context.Publications.Update(publication);
            User.Score += 25;
            Context.SaveChanges();
            RedirectToPage("/Unpublished");
        }

        public void GetPublication(int id)
        {
            var publication = Context.Publications.FirstOrDefault(p => p.Id == id);
            var users = Context.Users.ToList();
            var user = User = users.FirstOrDefault(u => u.Id == publication.UserId);
            var views = Context.Views.Where(v => v.PublicationId == publication.Id).Count();
            foreach (var comment in Context.Comments.Where(c => c.PublicationId == publication.Id).ToList())
            {
                Comments.Add(new CommentV2(comment, Context.Users.FirstOrDefault(u => u.Id == comment.UserId), comment.Date, Context.Comments.FirstOrDefault(c => c.Id == comment.AnswerOnId)));
            }
            List<Category> Categories = new List<Category>();
            var publicationCategories = Context.PublicationCategories.Where(c => c.PublicationId == publication.Id).ToList();
            foreach (var publicationCategory in publicationCategories)
            {
                Categories.Add(Context.Categories.FirstOrDefault(c => c.Id == publicationCategory.CategoryId));
            }
            Publication = new PublicationV2(publication, user, views, Comments.Count(), Categories);
        }

        public void OnPostDelete(int id)
        {
            GetPublication(id);
            var publication = Context.Publications.FirstOrDefault(p => p.Id == id);
            try
            {
                for (int i = 0; i < 100; i++)
                {
                    Context.PublicationCategories.Remove(Context.PublicationCategories.FirstOrDefault(pc => pc.PublicationId == id));
                }
                for (int i = 0; i < 100; i++)
                {
                    Context.Views.Remove(Context.Views.FirstOrDefault(v => v.PublicationId == id));
                }
                for (int i = 0; i < 100; i++)
                {
                    Context.Comments.Remove(Context.Comments.FirstOrDefault(c => c.PublicationId == id));
                }
            }
            catch 
            {
            }
            try
            {

                Context.SaveChanges();
                Context.Publications.Remove(publication);
                Context.SaveChanges();
            }
            catch
            {
            }
            RedirectToPage("/Unpublished");
        }
    }
}
