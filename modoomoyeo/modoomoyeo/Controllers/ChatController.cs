using Microsoft.AspNetCore.Mvc;
using modoomoyeo.Database;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System;
using System.IO;

namespace modoomoyeo.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("/Sign/Signin");

            UserQurey db = HttpContext.RequestServices.GetService(typeof(UserQurey)) as UserQurey;
            return View(db.findUserAll());
        }
        public IActionResult Mentors()
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("/Sign/Signin");

            UserQurey db = HttpContext.RequestServices.GetService(typeof(UserQurey)) as UserQurey;
            return View(db.findMentors());
        }
        [HttpPost]
        [Route("/Chat/Chatting")]
        public IActionResult ChattingRequest()
        {
            Console.WriteLine("debug");
            ViewData["Target"] = Request.Form["id"];
            return View("Chating");
        }

        public IActionResult Sum()
        {
            return View();
        }
    }
}
