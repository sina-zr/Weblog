using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Weblog.DataLayer.Context;
using Weblog.DataLayer.Entities;
using Weblog.Web.DTOs;

namespace Weblog.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly WeblogContext _context;

        public AuthController(WeblogContext context)
        {
            _context = context;
        }


        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("Register")]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "اطلاعات به درستی وارد نشده!";
                return RedirectToAction(nameof(Register));
            }

            registerDto.Username.Trim().ToLower();
            registerDto.Email.Trim().ToLower();

            var user = _context.Users.FirstOrDefault(u => u.Username == registerDto.Username);
            if (user != null)
            {
                TempData["Message"] = "نام کاربری قبلا استفاده شده";
                return RedirectToAction(nameof(Register));
            }

            var newUser = new User()
            {
                Username = registerDto.Username,
                Password = registerDto.Password,
                Email = registerDto.Email,
                IsMale = registerDto.IsMale
            };
            _context.Users.Add(newUser);
            _context.SaveChanges();

            TempData["Message"] = "ثبت نام با موفقیت انجام شد.";
            return RedirectToAction(nameof(Login));
        }

        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("Login")]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginDto loginDto, string ReturnUrl = "")
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "اطلاعات به درستی وارد نشده!";
                return RedirectToAction(nameof(Login));
            }

            var user = _context.Users.FirstOrDefault(u => u.Username == loginDto.Username);
            if (user != null)
            {
                if (user.Password == loginDto.Password)
                {
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                        new Claim(ClaimTypes.Name, user.Username)
                    };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    var properties = new AuthenticationProperties
                    {
                        IsPersistent = loginDto.RememberMe.Value
                    };
                    HttpContext.SignInAsync(principal, properties);

                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }

                    TempData["Message"] = "ورود موفقیت آمیز بود";
                    return RedirectToAction("Index", "Home");
                }
                TempData["Message"] = "رمز عبور نادرست است!";
                return RedirectToAction(nameof(Login));
            }
            TempData["Message"] = "نام کاربری پیدا نشد!";
            return RedirectToAction(nameof(Login));
        }

        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }
    }
}
