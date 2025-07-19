using Microsoft.AspNetCore.Mvc;
using Weblog.DataLayer.Context;
using Weblog.DataLayer.Entities;

namespace Weblog.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly WeblogContext _dbcontext;

        public HomeController(WeblogContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        [Route("/AboutUs")]
        public IActionResult AboutUs()
        {
            return View();
        }

        [HttpPost("[action]")]
        public IActionResult AddNewsletterSubscription(string email)
        {
            email = email.Trim().ToLower();
            if (_dbcontext.NewsLetterSubscriptions.Any(s => s.Email == email))
            {
                return BadRequest("Email already exists");
            }

            _dbcontext.NewsLetterSubscriptions.Add(new NewsLetterSubscription()
            {
                Email = email,
            });
            _dbcontext.SaveChanges();
            return Ok();
        }
    }
}
