using Microsoft.AspNetCore.Mvc;
using modoomoyeo.Models;
using System.Diagnostics;
using modoomoyeo.Database;

namespace modoomoyeo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            Console.WriteLine("debug2");
            return View();
        }
        [HttpPost]
        [Route("/Schedule/Post")]
        public IActionResult scheduleInsert()
        {
            if (User.Identity.IsAuthenticated)
            {
                ScheduleData schedule = new ScheduleData(Request.Form["title"],
                                                          User.Identity.Name,
                                                          Request.Form["contents"],
                                                          DateTime.Now,
                                                          10
                                                         );
                ScheduleQurey db = HttpContext.RequestServices.GetService(typeof(ScheduleQurey)) as ScheduleQurey;
                db.insertSchedule(schedule);
            }
            return Redirect(User.Identity.IsAuthenticated ? "/Home/Schedule" : "/Sign/Signin");
        }

        public IActionResult Schedule()
        {
            ScheduleQurey db = HttpContext.RequestServices.GetService(typeof(ScheduleQurey)) as ScheduleQurey;
            return View(db.scheduleDatas());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}