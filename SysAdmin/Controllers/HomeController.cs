using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SysAdmin.Models;

namespace SysAdmin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        private readonly IHostingEnvironment hostingEnv;
        public HomeController(IHostingEnvironment e)
        {
            hostingEnv = e;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult FAQ()
        {
            return View();
        }

        public IActionResult UploadFile(string SavedName, IFormFile pic)
        {
            ViewData["SavedName"] = SavedName;

            if(pic != null)
            {

                if (!System.IO.File.Exists(Path.Combine(hostingEnv.WebRootPath, User.Identity.Name + ".pdf")))
                {
                    var FileName = Path.Combine(hostingEnv.WebRootPath, User.Identity.Name + ".pdf");
                    pic.CopyTo(new FileStream(FileName, FileMode.Create));

                    ViewData["FileLocation"] = "/" + User.Identity.Name + ".pdf";
                    ViewData["Error"] = "";
                } else
                {
                    ViewData["Error"] = "Woops, this file alrady exists!";
                }
            }

            return View();
        }

        [AllowAnonymous]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
