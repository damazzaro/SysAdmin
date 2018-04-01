using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using SysAdmin.Repositories;

namespace SysAdmin.Controllers
{
    public class BlobsController : Controller
    {
        BlobsRepository _blobsRepository = new BlobsRepository();

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult ListBlobs()
        {
            return View(_blobsRepository.ListBlobsNames());
        }


    }
}