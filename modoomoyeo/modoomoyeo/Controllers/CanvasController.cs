using Microsoft.AspNetCore.Mvc;
using modoomoyeo.Models;
using System.Diagnostics;
using modoomoyeo.Database;
namespace modoomoyeo.Controllers
{
    public class CanvasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
