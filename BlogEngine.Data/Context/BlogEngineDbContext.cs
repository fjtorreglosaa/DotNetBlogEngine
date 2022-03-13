using BlogEngine.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BlogEngine.Data.Context
{
    public class BlogEngineDbContext : DbContext
    {
        public BlogEngineDbContext(DbContextOptions<BlogEngineDbContext> options) : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
