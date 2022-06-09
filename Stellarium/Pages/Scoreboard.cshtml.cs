using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Stellarium.Models;

namespace Stellarium.Pages
{
    public class ScoreboardModel : PageModel
    {
        ApplicationContext Context;
        public List<User> Users { get; set; }
        public ScoreboardModel(ApplicationContext db)
        {
            Context = db;
        }
        public void OnGet()
        {
            Users = Context.Users.Where(u => u.Score > 0).OrderByDescending(u => u.Score).ToList();
        }
    }
}
