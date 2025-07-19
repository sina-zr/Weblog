using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weblog.DataLayer.Entities;

namespace Weblog.DataLayer.Context
{
    public class WeblogContext : DbContext
    {
        public WeblogContext(DbContextOptions<WeblogContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogImage> BlogsImages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<ChatGroup> ChatGroups { get; set; }
        public DbSet<UserGroup> UsersGroups { get; set; }
        public DbSet<NewsLetterSubscription> NewsLetterSubscriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NewsLetterSubscription>()
                .HasAlternateKey(x => x.Email);
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
