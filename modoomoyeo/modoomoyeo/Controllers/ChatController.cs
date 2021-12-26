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

<<<<<<< HEAD
=======
        [HttpPost]
        [Route("/CHat/Chatting")]
        public IActionResult ChattingRequest()
        {            
            return View("Chating");
        }
>>>>>>> 0ce342a128676485c13ab9183074185d0bd18c1f
    }
}
