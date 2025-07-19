using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Weblog.DataLayer.Context;
using Weblog.DataLayer.Entities;
using Weblog.Web.DTOs;

namespace Weblog.Web.Controllers;

[AdminAuthorize]
public class AdminController : Controller
{
    private readonly WeblogContext _context;

    public AdminController(WeblogContext context)
    {
        _context = context;
    }

    public IActionResult ShowPanel()
    {
        ViewBag.PostsCount = _context.Blogs.Count();
        ViewBag.Users = _context.Users.ToList();
        ViewBag.Categories = _context.Categories.ToList();
        return View();
    }

    [HttpPost]
    public IActionResult CreateBlog(CreateBlogDto createBlogDto)
    {
        if (!ModelState.IsValid)
        {
            TempData["Message"] = "اطلاعات ارسالی مشکل دارد";
            return RedirectToAction(nameof(ShowPanel));
        }

        var newBlog = new Blog()
        {
            Title = createBlogDto.Title,
            Description = createBlogDto.Description,
            CreateDate = DateTime.Now,
            SubCategoryId = createBlogDto.SubCategoryId ?? 2
        };
        _context.Add(newBlog);
        _context.SaveChanges();

        if (createBlogDto.Images != null)
        {
            List<BlogImage> blogImages = new List<BlogImage>();
            var dir = Directory.GetCurrentDirectory();
            foreach (var img in createBlogDto.Images)
            {
                var fileExtension = Path.GetExtension(img.FileName);
                string[] imgFormats = { ".jpg", ".jpeg", ".png", ".svg", ".webp", ".gif" };
                if (!imgFormats.Contains(fileExtension))
                {
                    TempData["Message"] = "فرمت عکس پشتیبانی نشده";
                    return RedirectToAction(nameof(ShowPanel));
                }

                var filename = Path.GetFileNameWithoutExtension(img.FileName) + "-" + DateTime.Now.Ticks +
                               fileExtension;
                try
                {
                    using (var stream = new FileStream(path: dir + @"\\wwwroot\\BlogsImages\\" + filename,
                               FileMode.Create))
                    {
                        img.CopyTo(stream);
                    }

                    blogImages.Add(new BlogImage() { ImageName = filename, BlogId = newBlog.BlogId });
                }
                catch (Exception ex)
                {
                    TempData["Message"] = "خطا در ذخیره تصویر: " + ex.Message;
                    return RedirectToAction(nameof(ShowPanel));
                }
            }

            _context.AddRange(blogImages);
        }

        _context.SaveChanges();
        TempData["Message"] = "پست با موفقیت ایجاد شد";
        return RedirectToAction("ShowAllBlogs", "Blogs");
    }

    [HttpPost]
    public async Task<IActionResult> AddOutboundBlogs([FromForm] List<CreateBlogDtoOutbound> blogs)
    {
        if (blogs.Count == 0 || !ModelState.IsValid) return BadRequest("Bad Data");

        foreach (var blogDto in blogs)
        {
            var blog = new Blog
            {
                Title = blogDto.Title,
                Description = blogDto.Description,
                CreateDate = DateTime.Now
            };
            _context.Add(blog);
            await _context.SaveChangesAsync();

            if (!string.IsNullOrWhiteSpace(blogDto.ImageUrl))
            {
                // 1) Download
                using var http = new HttpClient();
                HttpResponseMessage resp;
                try
                {
                    resp = await http.GetAsync(blogDto.ImageUrl);
                    resp.EnsureSuccessStatusCode();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    continue;
                    //return BadRequest("Failed to download image.");
                }

                // 2) Validate content‐type / extension
                var contentType = resp.Content.Headers.ContentType.MediaType; // e.g. "image/jpeg"
                var ext = contentType switch
                {
                    "image/jpeg" => ".jpg",
                    "image/png" => ".png",
                    "image/gif" => ".gif",
                    "image/webp" => ".webp",
                    _ => null
                };
                if (ext == null)
                    return BadRequest("Unsupported image format.");

                // 3) Save to disk
                var filename = Path.GetFileNameWithoutExtension(Path.GetRandomFileName())
                               + "_" + DateTime.Now.Ticks + ext;
                var savePath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot", "BlogsImages", filename);
                await using var fileStream = new FileStream(savePath, FileMode.Create);
                await resp.Content.CopyToAsync(fileStream);

                // 4) Record in DB
                _context.BlogsImages.Add(new BlogImage
                {
                    ImageName = filename,
                    BlogId = blog.BlogId
                });
                await _context.SaveChangesAsync();
            }
        }

        return Ok();
    }

    public IActionResult EditPost(int id)
    {
        var blog = _context.Blogs
            .Include(b => b.BlogImages)
            .FirstOrDefault(b => b.BlogId == id);

        if (blog == null)
        {
            TempData["MessageWarning"] = "پست پیدا نشد!";
            return RedirectToAction("ShowAllBlogs", "Blogs");
        }

        ViewBag.Categories = _context.Categories.ToList();

        var blogDto = new EditBlogDto()
        {
            BlogId = blog.BlogId,
            Description = blog.Description,
            SubCategoryId = blog.SubCategoryId,
            Title = blog.Title
        };

        return View(blogDto);
    }

    [HttpPost]
    public IActionResult EditPost(EditBlogDto editBlogDto)
    {
        var blog = _context.Blogs.FirstOrDefault(b => b.BlogId == editBlogDto.BlogId);
        if (blog == null)
        {
            TempData["MessageWarning"] = "پست پیدا نشد!";
            return RedirectToAction("ShowAllBlogs", "Blogs");
        }

        if (!string.IsNullOrEmpty(editBlogDto.Title))
        {
            blog.Title = editBlogDto.Title;
        }

        if (!string.IsNullOrEmpty(editBlogDto.Description))
        {
            blog.Description = editBlogDto.Description;
        }

        if (editBlogDto.SubCategoryId > 1)
        {
            blog.SubCategoryId = editBlogDto.SubCategoryId;
        }

        _context.Update(blog);
        _context.SaveChanges();

        if (editBlogDto.Images != null)
        {
            var oldImages = _context.BlogsImages
                .Where(i => i.BlogId == editBlogDto.BlogId).ToList();

            var dir = Directory.GetCurrentDirectory();

            if (oldImages.Count > 0)
            {
                foreach (var img in oldImages)
                {
                    try
                    {
                        var filePath = Path.Combine(dir, "wwwroot", "BlogsImages", img.ImageName);

                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                    catch (Exception ex)
                    {
                        TempData["Message"] = "خطا در حذف تصویر: " + ex.Message;
                        return RedirectToAction(nameof(ShowPanel));
                    }
                }

                _context.RemoveRange(oldImages);
            }

            List<BlogImage> blogImages = new List<BlogImage>();
            foreach (var img in editBlogDto.Images)
            {
                var fileExtension = Path.GetExtension(img.FileName);
                string[] imgFormats = { ".jpg", ".jpeg", ".png", ".svg", ".webp", ".gif" };
                if (!imgFormats.Contains(fileExtension))
                {
                    TempData["Message"] = "فرمت عکس پشتیبانی نشده";
                    return RedirectToAction(nameof(ShowPanel));
                }

                var filename = Path.GetFileNameWithoutExtension(img.FileName) + "-" + DateTime.Now.Ticks +
                               fileExtension;
                try
                {
                    using (var stream = new FileStream(path: dir + @"\\wwwroot\\BlogsImages\\" + filename,
                               FileMode.Create))
                    {
                        img.CopyTo(stream);
                    }

                    blogImages.Add(new BlogImage() { ImageName = filename, BlogId = editBlogDto.BlogId });
                }
                catch (Exception ex)
                {
                    TempData["Message"] = "خطا در ذخیره تصویر: " + ex.Message;
                    return RedirectToAction(nameof(ShowPanel));
                }
            }

            _context.AddRange(blogImages);
            _context.SaveChanges();
        }

        return RedirectToAction("ShowAllBlogs", "Blogs");
    }

    public IActionResult DeletePost(int id)
    {
        if (id <= 0)
        {
            TempData["Message"] = "اطلاعات ارسالی مشکل دارد";
            return RedirectToAction("ShowAllBlogs", "Blogs");
        }

        var blog = _context.Blogs.FirstOrDefault(b => b.BlogId == id);
        if (blog == null)
        {
            TempData["Message"] = "پست مورد نظر یافت نشد";
            return RedirectToAction("ShowAllBlogs", "Blogs");
        }

        _context.Remove(blog);
        _context.SaveChanges();

        TempData["Message"] = "حذف با موفقیت انجام شد";
        return RedirectToAction("ShowAllBlogs", "Blogs");
    }

    public IActionResult DeleteUser(int id)
    {
        if (id <= 0)
        {
            TempData["Message"] = "اطلاعات ارسالی مشکل دارد";
            return RedirectToAction(nameof(ShowPanel));
        }

        var user = _context.Users.FirstOrDefault(u => u.UserId == id);
        if (user == null)
        {
            TempData["Message"] = "کاربر مورد نظر یافت نشد";
            return RedirectToAction(nameof(ShowPanel));
        }

        _context.Remove(user);
        _context.SaveChanges();

        TempData["Message"] = "حذف با موفقیت انجام شد";
        return RedirectToAction(nameof(ShowPanel));
    }
}