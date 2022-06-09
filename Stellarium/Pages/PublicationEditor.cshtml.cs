using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stellarium.Models;
using System.ComponentModel.DataAnnotations;

namespace Stellarium.Pages
{
    [RequestFormLimits(MultipartBodyLengthLimit = 209715200)]
    [RequestSizeLimit(209715200)]
    public class PublicationEditorModel : PageModel
    {
        public ApplicationContext Context;
        private readonly IWebHostEnvironment webHostEnvironment;

        [BindProperty]
        public IFormFile TitleImg { get; set; }
        public User CurrentUser;
        public PublicationEditorModel(ApplicationContext db, IWebHostEnvironment webHostEnvironment)
        {
            Context = db;
            this.webHostEnvironment = webHostEnvironment;
        }

        public void OnGet()
        {
            string useremail = Request.Cookies["useremail"];
            if (useremail != null)
            {
                CurrentUser = Context.Users.FirstOrDefault(x => x.Email == useremail);
            }
        }

        [RequestFormLimits(MultipartBodyLengthLimit = 209715200)]
        [RequestSizeLimit(209715200)]
        public void OnPostWrite(string title, string titletext, string firstcategory, string secondcategory, string text, IFormFile titleimg, IFormFile? testfile)
        {
            if (title == null || titletext == null || firstcategory == null || text == null || FileUpload == null)
            {
                return;
            }

            OnGet();

            TitleImg = FileUpload.FormFile;

            string imgName = "";

            string uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "Images");
            imgName = Guid.NewGuid().ToString() + "_" + TitleImg.FileName;
            string path = Path.Combine(uploadFolder, imgName);
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                TitleImg.CopyTo(fileStream);
            }

            var date = DateTime.Now;
            var publication = new Publication(0, CurrentUser.Id, date, title, titletext, "/Images/"+imgName, text, false);
            Context.Publications.Add(publication);
            Context.SaveChanges();
            publication = Context.Publications.FirstOrDefault(p => p.UserId == CurrentUser.Id && p.DateTime.Minute == date.Minute && p.DateTime.Hour == date.Hour && p.DateTime.Day == date.Day && p.DateTime.Month == date.Month && p.DateTime.Year == date.Year && p.DateTime.Second == date.Second);
            Context.PublicationCategories.Add(new PublicationCategory(0, publication.Id, Context.Categories.FirstOrDefault(c => c.Name == firstcategory).Id));
            if (secondcategory != "Без категории")
            {
                Context.PublicationCategories.Add(new PublicationCategory(0, publication.Id, Context.Categories.FirstOrDefault(c => c.Name == secondcategory).Id));
            }
            Context.SaveChanges();
        }

        [BindProperty]
        public BufferedSingleFileUploadDb FileUpload { get; set; }

        public class BufferedSingleFileUploadDb
        {
            [Required]
            [Display(Name = "File")]
            public IFormFile FormFile { get; set; }
        }
    }
}
