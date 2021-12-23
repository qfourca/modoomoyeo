using Microsoft.AspNetCore.Mvc;
using modoomoyeo.Database;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
        public IActionResult SigninProc()
        {
            UserQurey db = HttpContext.RequestServices.GetService(typeof(UserQurey)) as UserQurey;
            Userdata userdata = new Userdata(Request.Form["email"], Request.Form["pw"], null);
            return Redirect($"/Sign/result?msg={HttpUtility.UrlEncode(db.signin(userdata)? "true" : "false")}" +
                $"&begin=login");
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
    }
}