using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SysAdmin.Repositories
{
    public class BlobsRepository
    {
        string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=dmsysadmin;AccountKey=rLOkasSvakqO5lGqKdbqMrq1k8gBl5QpfB4HFJJDx99WOxmOgvkbE1MXNA5p8+7vTesQBRR9DPL7Xe/K/kNoVw==;EndpointSuffix=core.windows.net";


        private CloudBlobContainer GetCloudBlobContainer()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("socialfiles");
            return container;
        }

        public List<string> ListBlobsNames()
        {
            List<string> blobNames = new List<string>();
            CloudBlobContainer container = GetCloudBlobContainer();
            var blobs = container.GetDirectoryReference("").ListBlobsSegmentedAsync(false, BlobListingDetails.Metadata, 100, null, null, null).Result;


            foreach (IListBlobItem item in blobs.Results)
            {
                if (item.GetType() == typeof(CloudBlockBlob))
                {
                    CloudBlockBlob blob = (CloudBlockBlob)item;
                    blobNames.Add(blob.Name);
                }
                else if (item.GetType() == typeof(CloudPageBlob))
                {
                    CloudPageBlob blob = (CloudPageBlob)item;
                    blobNames.Add(blob.Name);
                }
                else if (item.GetType() == typeof(CloudBlobDirectory))
                {
                    CloudBlobDirectory dir = (CloudBlobDirectory)item;
                    blobNames.Add(dir.Uri.ToString());
                }

            }

            return blobNames;
        }

        public async Task DownloadBlobAsync(string FileName)
        {
            CloudBlobContainer container = GetCloudBlobContainer();
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(FileName);

            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), FileName);
            using (var fileStream = System.IO.File.OpenWrite(path))
            {
                await blockBlob.DownloadToStreamAsync(fileStream);
            }
        }

        public async Task<bool> UploadBlobAsync(IFormFile File, String Name)
        {

            if (File == null)
            {
                return false;
            }

            CloudBlobContainer container = GetCloudBlobContainer();
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(Name);

            using (var fileStream = File.OpenReadStream())
            {
                await blockBlob.UploadFromStreamAsync(fileStream);
            }

            return true;
        }

        public async Task<bool> DoesBlobExistAsync(string Name)
        {
            CloudBlobContainer container = GetCloudBlobContainer();
            CloudBlockBlob existingBlob = container.GetBlockBlobReference(Name);

            return await existingBlob.ExistsAsync();
        }

    }
}
