﻿using Microsoft.AspNetCore.Mvc;
using modoomoyeo.Database;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

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

        [HttpPost]
        [Route("/Chat/Chatting")]

    }
        
}
