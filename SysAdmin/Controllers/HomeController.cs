using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using SysAdmin.Models;

namespace SysAdmin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        private readonly IHostingEnvironment _hostingEnvironment;
        public HomeController(IHostingEnvironment e)
        {
            _hostingEnvironment = e;
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

        public IActionResult UploadFile(IFormFile FileUpload, string FileName, string FileDescription)
        {
            Console.WriteLine("--------------------------------RAN COMMAND");

            if (FileUpload != null)
            {
                Console.WriteLine("--------------------------------NOT NULL");

                if (!System.IO.File.Exists(Path.Combine(_hostingEnvironment.WebRootPath, User.Identity.Name + FileName + ".pdf")))
                {
                    Console.WriteLine("--------------------------------DOESNT EXIST");

                    var fileStream = Path.Combine(_hostingEnvironment.WebRootPath, User.Identity.Name + FileName + ".pdf");
                    FileUpload.CopyTo(new FileStream(fileStream, FileMode.Create));

                    Console.WriteLine("--------------------------------UPLOADED FILE");

                    ViewData["SavedName"] = User.Identity.Name + FileName + ".pdf";
                    ViewData["FileLocation"] = "/" + User.Identity.Name + FileName + ".pdf";
                    ViewData["SavedDescription"] = FileDescription;
                } else
                {
                    ModelState.AddModelError("FileName", "Woops, this file already exists!");

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
