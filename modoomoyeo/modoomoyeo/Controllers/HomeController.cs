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
            PostQurey db = HttpContext.RequestServices.GetService(typeof(PostQurey)) as PostQurey;
            UserQurey name = HttpContext.RequestServices.GetService(typeof(UserQurey)) as UserQurey;
            List<PostData> postDatas = new List<PostData>();
            postDatas = db.findPosts(10);
            foreach (var postData in postDatas)
            {
                postData.name = name.idToName(postData.ownerid);
                
            }
            return View(postDatas);
        }

        public IActionResult Wiki()
        {
            return View();
        }
        public IActionResult Insert()
        {
            return View();
        }
        public IActionResult test()
        {
            return View();
        }
        [HttpPost]
        [Route("/Schedule/Post")]
        public async Task<IActionResult> OnPostUploadAsync(List<IFormFile> image_file)
        {
            long size = image_file.Sum(f => f.Length);
            Console.WriteLine(size);
            if (User.Identity.IsAuthenticated)
            {
                PostData post = new PostData(0,
                                                 Request.Form["title"],
                                                 null,
                                                 Int32.Parse(User.FindFirst("userid").Value),
                                                 Request.Form["contents"],
                                                 DateTime.Now,
                                                 10);
                PostQurey db = HttpContext.RequestServices.GetService(typeof(PostQurey)) as PostQurey;
                int code = db.insertPost(post);

                foreach (var formFile in image_file)
                {
                    if (formFile.Length > 0)
                    {
                        var filePath = $"D:\\modoomoyeo\\modoomoyeo\\modoomoyeo\\wwwroot\\img\\posts\\{code}.png";
                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await formFile.CopyToAsync(stream);
                        }
                    }
                }
                return Redirect("/Home/Index");
            }
            return Redirect("/Sign/Signin");
        }
        [HttpPost]
        [Route("/Schedule/Schedule")]
        public IActionResult OnScheduleUploadAsync()
        {
            Console.WriteLine(Request.Form["title"]);
            Console.WriteLine(Request.Form["contents"]);
            Console.WriteLine(Request.Form["birthday"]);

                                                 
            ScheduleData post = new ScheduleData(Request.Form["title"],
                                                 Int32.Parse(User.FindFirst("userid").Value),
                                                 Request.Form["contents"],
                                                 Convert.ToDateTime(Request.Form["startdate"]),
                                                 Convert.ToDateTime(Request.Form["enddate"]),
                                                 10);
            ScheduleQurey db = HttpContext.RequestServices.GetService(typeof(ScheduleQurey)) as ScheduleQurey;
            db.insertSchedule(post);
            return Redirect("/Home/Schedule");
        }
        public IActionResult Insertschedule()
        {
            return View();
        }
        public IActionResult Schedule()
        {
            PostQurey db = HttpContext.RequestServices.GetService(typeof(PostQurey)) as PostQurey;
            return View(db.findPosts(10));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}