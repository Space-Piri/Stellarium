namespace Stellarium
{
    using Microsoft.EntityFrameworkCore;
    using Stellarium.Models;

    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Publication> Publications { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;
        public DbSet<View> Views { get; set; } = null!;
        public DbSet<Visitor> Visitors { get; set; } = null!;
        public DbSet<BookMark> Bookmarks { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<PublicationCategory> PublicationCategories { get; set; } = null!;

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();   
        }
    }
}
