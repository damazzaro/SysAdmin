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
        string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=dmsysadmin;AccountKey=rLOkasSvakqO5lGqKdbqMrq1k8gBl5QpfB4HFJJDx99WOxmOgvkbE1MXNA5p8+7vTesQBRR9DPL7Xe/K/kNoVw==;EndpointSuffix=core.windows.net";

        public IActionResult Index()
        {
            return View();
        }

        private CloudBlobContainer GetCloudBlobContainer()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("socialfiles");
            return container;
        }

        public ActionResult ListBlobs()
        {
            return View(_blobsRepository.ListBlobsNames());
        }


    }
}