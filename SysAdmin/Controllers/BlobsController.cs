using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage.Blob;
using SysAdmin.Repositories;

namespace SysAdmin.Controllers
{
    [Authorize]
    public class BlobsController : Controller
    {
        IHttpContextAccessor _httpAccessor = new HttpContextAccessor();
        BlobsRepository _blobsRepository = new BlobsRepository();


        public IActionResult Index()
        {
            return View();
        }

        public ActionResult ListBlobs()
        {
            return View(_blobsRepository.ListBlobsNames());
        }

        public string GetUsername()
        {
            MailAddress emailAddress = new MailAddress(User.Identity.Name);
            string Username = emailAddress.User;

            return Username;
        }

        public async Task<ActionResult> DownloadAsync(string FileName)
        {
            MailAddress emailAddress = new MailAddress(User.Identity.Name);
            string Username = emailAddress.User;
            string blobName = Username + FileName;

            if (await _blobsRepository.DoesBlobExistAsync(blobName))
            {

                //await _blobsRepository.DownloadBlobAsync(blobName);
            
                var fileContent = new System.Net.WebClient().DownloadData("https://dmsysadmin.blob.core.windows.net/socialfiles/" + blobName);
                TempData["DownloadSuccess"] = "Success! You've downloaded the file. Yay exciting!";

                return File(fileContent, "application/octet-stream", blobName);

            }
            else
            {
                TempData["DownloadFail"] = "There was trouble downloading the file. Sorry we tried!";
            }

            return Redirect(Request.Headers["Referer"].ToString());

        }


    }
}