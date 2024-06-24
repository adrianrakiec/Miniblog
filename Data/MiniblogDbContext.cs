using Microsoft.EntityFrameworkCore;
using Miniblog.Models.Domain;

namespace Miniblog.Data
{
    public class MiniblogDbContext : DbContext
    {
        public MiniblogDbContext(DbContextOptions<MiniblogDbContext> options) : base(options)
        {
        }

        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }
}
