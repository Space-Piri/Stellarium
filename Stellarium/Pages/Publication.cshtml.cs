using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stellarium.Models;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Stellarium.Pages
{
    public class PublicationModel : PageModel
    {
        public ApplicationContext Context;
        public PublicationV2 Publication;
        public List<CommentV2> Comments = new List<CommentV2>();
        public User CurrentUser;
        public BookMark UserBookmark;
        public PublicationModel(ApplicationContext db)
        {
            Context = db;
        }

        public async void OnGet(int id)
        {
            await GetPublication(id);
        }

        public async void OnPostSendComment(int id, string text)
        {
            if (text == null)
            {
                return;
            }
            await GetPublication(id);
            var newComment = new Comment(0, CurrentUser.Id, Publication.Publication.Id, text, null, DateTime.Now);
            Context.Comments.Add(newComment);
            var newCommentV2 = new CommentV2(newComment, Context.Users.FirstOrDefault(u => u.Id == newComment.UserId), newComment.Date, Context.Comments.FirstOrDefault(c => c.Id == newComment.AnswerOnId));
            if (Comments.FirstOrDefault(c => c.Comment.Text == text && c.User == CurrentUser) == null)
            {
                Comments.Add(newCommentV2);
                Publication.Comments++;
                CurrentUser.Score += 3;
                Context.SaveChanges();
            }
            else
            {
                ViewData["Message"] = "true";
            }
        }

        public async void OnPostDeleteComment(int id, int commentId)
        {
            await GetPublication(id);
            var comment = Context.Comments.FirstOrDefault(c => c.Id == commentId);
            if (comment != null)
            {
                var answers = Context.Comments.Where(c => c.AnswerOnId == commentId);
                if (answers != null)
                {
                    foreach (var answer in answers)
                    {
                        Context.Comments.Remove(answer);
                    }
                }
                Context.Comments.Remove(comment);
                CurrentUser.Score -= 3;
                Context.SaveChanges();
                Comments.Remove(Comments.FirstOrDefault(c => c.Comment == comment));
                Publication.Comments--;
            }
        }

        async Task GetPublication(int id)
        {
            var publication = Context.Publications.FirstOrDefault(p => p.Id == id);
            var vid = HttpUtility.UrlDecode(Request.Cookies["visitorId"]);
            int visitorId = Convert.ToInt32(vid);
            if (Context.Views.FirstOrDefault(v => v.VisitorId == visitorId && v.PublicationId == publication.Id) == null)
            {
                Context.Views.Add(new View(0, visitorId, publication.Id));
                Context.SaveChanges();
            }

            var users = Context.Users.ToList();
            var user = users.FirstOrDefault(u => u.Id == publication.UserId);
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

            string useremail = Request.Cookies["useremail"];
            if (useremail != null)
            {
                CurrentUser = Context.Users.FirstOrDefault(x => x.Email == useremail);
                UserBookmark = Context.Bookmarks.FirstOrDefault(b => b.UserId == CurrentUser.Id && b.PublicationId == Publication.Publication.Id);
            }

        }
        public async void OnPostAddBookMark(int id)
        {
            try
            {
                await GetPublication(id);
                UserBookmark = new BookMark(0, CurrentUser.Id, Publication.Publication.Id);
                if (!Context.Bookmarks.Contains(UserBookmark))
                {
                    Context.Bookmarks.Add(UserBookmark);
                    Context.SaveChanges();
                }
            }
            catch 
            {
            }
        }

        public async void OnPostDeleteBookMark(int id)
        {
            try
            {
                await GetPublication(id);
                Context.Bookmarks.Remove(UserBookmark);
                Context.SaveChanges();
                UserBookmark = null;
            }
            catch
            {
            }
        }

        [BindProperty]
        public BufferedMessage Message { get; set; }

        public class BufferedMessage
        {
            [Required]
            [Display(Name = "Message")]
            public string MessageText { get; set; }
        }

        public async void OnPostAnswer(int id, int commentId, string answerText2)
        {
            await GetPublication(id);
            if (answerText2 == null || answerText2 == "null")
            {
                return;
            }
            var newComment = new Comment(0, CurrentUser.Id, Publication.Publication.Id, answerText2, commentId, DateTime.Now);
            Context.Comments.Add(newComment);
            var newCommentV2 = new CommentV2(newComment, Context.Users.FirstOrDefault(u => u.Id == newComment.UserId), newComment.Date, Context.Comments.FirstOrDefault(c => c.Id == newComment.AnswerOnId));
            if (Comments.FirstOrDefault(c => c.Comment.Text == answerText2 && c.User == CurrentUser) == null)
            {
                Comments.Add(newCommentV2);
                Publication.Comments++;
                Context.SaveChanges();
            }
            else
            {
                ViewData["Message"] = "true";
            }
        }
    }
}
