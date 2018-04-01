using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
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

        public async Task<ActionResult> DownloadAsync(string FileName)
        {
            MailAddress emailAddress = new MailAddress(User.Identity.Name);
            string Username = emailAddress.User;
            string blobName = Username + FileName;

            if (await _blobsRepository.DoesBlobExistAsync(blobName))
            {

                await _blobsRepository.DownloadBlobAsync(blobName);

                TempData["DownloadSuccess"] = "Success! You've downloaded the file. Check your documents folder";
            } else
            {
                TempData["DownloadFail"] = "There was trouble downloading the file. Sorry we tried!";
            }

            return Redirect(Request.Headers["Referer"].ToString());

        }


    }
}