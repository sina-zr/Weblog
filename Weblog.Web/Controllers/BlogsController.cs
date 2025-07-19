using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Weblog.DataLayer.Context;

namespace Weblog.Web.Controllers
{
    [Authorize]
    public class BlogsController : Controller
    {
        private readonly WeblogContext _context;

        public BlogsController(WeblogContext context)
        {
            _context = context;
        }

        public IActionResult ShowAllBlogs(int pageId = 1, string? filterTitle = null, List<int>? filterCategory = null)
        {
            var query = _context.Blogs.Include(b => b.BlogImages).AsQueryable();

            if (!string.IsNullOrEmpty(filterTitle))
            {
                query = query.Where(b => b.Title.Contains(filterTitle));
            }

            if (filterCategory != null && filterCategory.Any(c => c > 0))
            {
                query = query.Where(b => b.SubCategoryId.HasValue && filterCategory.Contains(b.SubCategoryId.Value));
            }

            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.SelectedFilterCategory = filterCategory;
            ViewBag.FilterTitle = filterTitle;

            //Pagination
            int take = 5;
            int pageCount = (int)(Math.Ceiling((decimal)query.Count() / take));
            if (pageId < 1)
            {
                pageId = 1;
            }
            if (pageId > pageCount)
            {
                pageId = pageCount;
            }
            int skip = (pageId - 1) * take;
            if (skip < 0)
            {
                skip = 0;
            }
            
            var blogs = query.OrderByDescending(b => b.CreateDate).Skip(skip).Take(take).ToList();

            ViewBag.CurrentPage = pageId;
            ViewBag.PagesCount = pageCount;
            
            return View(blogs);
        }

        public IActionResult ShowBlogDetails(int id)
        {
            if (id <= 0)
            {
                TempData["Message"] = "آیدی بلاگ مشکل دارد";
                return RedirectToAction(nameof(ShowAllBlogs));
            }

            var blog = _context.Blogs
                .Include(b => b.BlogImages)
                .FirstOrDefault(b => b.BlogId == id);
            if (blog == null)
            {
                TempData["Message"] = "پست مورد نظر یافت نشد";
                return RedirectToAction(nameof(ShowAllBlogs));
            }

            return View(blog);
        }

        [HttpGet]
        public IActionResult SearchInBlogs(string text)
        {
            var titles = _context.Blogs.Where(b => b.Title.Contains(text))
                .Select(b => b.Title)
                .ToList();

            if (titles.Any(t => !string.IsNullOrEmpty(t)))
            {
                return Json(Ok(titles));
            }
            else
            {
                return NotFound();
            }
        }
    }
}
