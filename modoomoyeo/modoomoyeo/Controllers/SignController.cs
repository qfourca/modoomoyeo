using Microsoft.AspNetCore.Mvc;
using modoomoyeo.Database;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace Ducademy.Controllers
{
    public class SignController : Controller
    {
        //public SignController(ILogger<HomeController> logger)
        //{

        //}
        public IActionResult Index() { return Redirect("/Sign/Signin"); }
        public IActionResult Signin()
        {
            return View();
        }

        [HttpPost]
        [Route("/Sign/Signin/Post")]
        public async Task<IActionResult> SigninProc()
        {
            UserQurey db = HttpContext.RequestServices.GetService(typeof(UserQurey)) as UserQurey;
            Userdata userdata = new Userdata(Request.Form["email"], Request.Form["pw"], null);
            bool result = db.signin(userdata);
            if(result)
            { 
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme,
                    ClaimTypes.Name, ClaimTypes.Role);  
                identity.AddClaim(new Claim(ClaimTypes.Email, db.findData(userdata.Email, "Email")));
                identity.AddClaim(new Claim(ClaimTypes.Name, db.findData(userdata.Email, "Name")));
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, db.findData(userdata.Email, "Email")));
                //identity.AddClaim(new Claim("LastCheckDateTime", DateTime.UtcNow.ToString("yyyyMMddHHmmss")));
                Console.WriteLine("Login Sucess" + db.findData(userdata.Email, "Name"));
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
                {
                    IsPersistent = false,
                    ExpiresUtc = DateTime.UtcNow.AddHours(4),
                    AllowRefresh = true
                });
                return Redirect("/");
            }
            else
            {
                return Redirect($"/Sign/result?msg={HttpUtility.UrlEncode("false")}" +
                $"&begin=login");
            }
        }

        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        [Route("/Sign/Signup/Post")]
        public IActionResult SignupProc()
        {
            if (Request.Form["pw"] != Request.Form["repw"])
            {
                return Redirect($"/Sign/result?msg={HttpUtility.UrlEncode("비밀번호가 다릅니다")}" +
                $"&begin=signup");
            }
            else
            {
                UserQurey db = HttpContext.RequestServices.GetService(typeof(UserQurey)) as UserQurey;
                Userdata userdata = new Userdata(Request.Form["email"], Request.Form["pw"], Request.Form["name"]);
                return Redirect($"/Sign/result?msg={HttpUtility.UrlEncode(db.signup(userdata))}" +
                                $"&begin=signup");
            }
        }
        public IActionResult result(string msg, string begin)
        {
            if (begin == "login")
                ViewData["result"] = msg == "true" ? "login success" : "login fail";
            else
                ViewData["result"] = msg;
            return View();
        }

        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }
    }
}