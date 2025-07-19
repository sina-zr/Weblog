using Microsoft.EntityFrameworkCore;
using Weblog.DataLayer.Context;
using Weblog.DataLayer.Entities;

namespace Weblog.DataLayer;

public class DbInitializer
{
    public static void Seed(WeblogContext context)
    {
        context.Database.Migrate();

        if (!context.Users.Any())
        {
            var admin = new User()
            {
                Username = "admin",
                Email = "admin@admin.com",
                Password = "admin",
                IsMale = true,
                Age = 32,
                Biography = "This is Admin",
                AvatarName = "300-15.jpg"
            };
            
            context.Users.Add(admin);

            if (!context.Categories.Any())
            {
                var news = new Category()
                {
                    Title = "News"
                };
                context.Categories.Add(news);
                context.SaveChanges();

                var newsSub = new List<Category>()
                {
                    new Category() { Title = "Politics", ParentId = news.CategoryId },
                    new Category() { Title = "Local", ParentId = news.CategoryId },
                    new Category() { Title = "Sports", ParentId = news.CategoryId },
                    new Category() { Title = "Tech", ParentId = news.CategoryId },
                };
                context.Categories.AddRange(newsSub);

                var edu = new Category()
                {
                    Title = "Education"
                };
                
                context.Categories.Add(edu);
                context.SaveChanges();

                var eduSub = new List<Category>()
                {
                    new Category() { Title = "Science", ParentId = edu.CategoryId },
                    new Category() { Title = "Mobile", ParentId = edu.CategoryId },
                    new Category() { Title = "Photography", ParentId = edu.CategoryId },
                };
                context.Categories.AddRange(eduSub);
                context.SaveChanges();
            }
        }
    }
}