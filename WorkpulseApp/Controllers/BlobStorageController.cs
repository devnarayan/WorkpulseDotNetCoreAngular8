using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
using System.Linq;
using CORTNE.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using CORTNE.EFModelCORTNEDB;
using CORTNE.ViewModel;
using log4net;
using CORTNE.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Net.Mime;

namespace CORTNE.Controllers
{
    //[Authorize]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class BlobStorageController : ControllerBase
    {
        private readonly IOptions<BlobConfig> config;
        private readonly CommonHelper _commonHelper;
        private static readonly ILog _log = LogManager.GetLogger(typeof(BlobStorageController));
        private readonly CORTNEDEVContext _context;
        private readonly IHostingEnvironment _env;

        public BlobStorageController(IOptions<BlobConfig> config, CommonHelper commonHelper, CORTNEDEVContext context, IHostingEnvironment env)
        {
            _context = context;
            this.config = config;
            _commonHelper = commonHelper;
            _env = env;
        }

        [HttpGet("ListFiles")]
        public async Task<List<string>> ListFiles()
        {
            List<string> blobs = new List<string>();
            try
            {
                if (CloudStorageAccount.TryParse(config.Value.StorageConnection, out CloudStorageAccount storageAccount))
                {
                    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                    CloudBlobContainer container = blobClient.GetContainerReference(config.Value.Container);

                    BlobResultSegment resultSegment = await container.ListBlobsSegmentedAsync(null);
                    foreach (IListBlobItem item in resultSegment.Results)
                    {
                        if (item.GetType() == typeof(CloudBlockBlob))
                        {
                            CloudBlockBlob blob = (CloudBlockBlob)item;
                            blobs.Add(blob.Name);
                        }
                        else if (item.GetType() == typeof(CloudPageBlob))
                        {
                            CloudPageBlob blob = (CloudPageBlob)item;
                            blobs.Add(blob.Name);
                        }
                        else if (item.GetType() == typeof(CloudBlobDirectory))
                        {
                            CloudBlobDirectory dir = (CloudBlobDirectory)item;
                            blobs.Add(dir.Uri.ToString());
                        }
                    }
                }
            }
            catch
            {
            }
            return blobs;
        }

        [HttpPost("InsertFile")]
        public async Task<IActionResult> InsertFile(IFormFile asset)
        {
            return Ok(await UploadFile(asset, config.Value.Container));
        }

        [HttpPost("InsertCashLog")]
        public async Task<IActionResult> InsertCashLog(IFormFile asset)
        {
            return Ok(await UploadFile(asset, config.Value.CashLogContainer));
        }
        [HttpPost("InsertCashReceipt")]
        public async Task<IActionResult> InsertCashReceipt(IFormFile asset)
        {
            return Ok(await UploadFile(asset, config.Value.CashReceiptContainer));
        }
        [HttpPost("InsertDeposit")]
        public async Task<IActionResult> InsertDeposit(IFormFile asset)
        {
            return Ok(await UploadFile(asset, config.Value.DepositContainer));
        }
        [HttpPost("InsertTransfer")]
        public async Task<IActionResult> InsertTransfer(IFormFile asset)
        {
            return Ok(await UploadFile(asset, config.Value.TransferContainer));
        }
        [HttpPost("InsertAccountCode")]
        public async Task<IActionResult> InsertAccountCode(IFormFile asset)
        {
            return Ok(await UploadFile(asset, config.Value.AccountCodeContainer));
        }

        private async Task<string> UploadFile(IFormFile asset, string containerName)
        {
            string filurl = string.Empty;
            try
            {
                if (CloudStorageAccount.TryParse(config.Value.StorageConnection, out CloudStorageAccount storageAccount))
                {
                    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                    CloudBlobContainer container = blobClient.GetContainerReference(containerName);

                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(asset.FileName);

                    await blockBlob.UploadFromStreamAsync(asset.OpenReadStream());

                    filurl = blockBlob.StorageUri.PrimaryUri.ToString();
                    return filurl;
                }
                else
                {
                    return filurl;
                }
            }
            catch
            {
                return filurl;
            }
        }


        [HttpGet("DownloadFile/{fileName}")]
        public async Task<IActionResult> DownloadFile(string fileName, ContainerEnum container = ContainerEnum.Container)
        {
            if (container == ContainerEnum.Container)
            {
                return await DownloadFileContainer(fileName, config.Value.Container);
            }
            else if (container == ContainerEnum.CashLogContainer)
            {
                return await DownloadFileContainer(fileName, config.Value.CashLogContainer);
            }
            else if (container == ContainerEnum.CashReceiptContainer)
            {
                return await DownloadFileContainer(fileName, config.Value.CashReceiptContainer);
            }
            else if (container == ContainerEnum.DepositContainer)
            {
                return await DownloadFileContainer(fileName, config.Value.DepositContainer);
            }
            else if (container == ContainerEnum.TransferContainer)
            {
                return await DownloadFileContainer(fileName, config.Value.TransferContainer);
            }
            else if (container == ContainerEnum.AccountCodeContainer)
            {
                return await DownloadFileContainer(fileName, config.Value.AccountCodeContainer);
            }
            else
            {
                return await DownloadFileContainer(fileName, config.Value.Container);
            }
        }

        private async Task<IActionResult> DownloadFileContainer(string fileName, string containerName)
        {
            MemoryStream ms = new MemoryStream();
            if (CloudStorageAccount.TryParse(config.Value.StorageConnection, out CloudStorageAccount storageAccount))
            {
                CloudBlobClient BlobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = BlobClient.GetContainerReference(containerName);

                if (await container.ExistsAsync())
                {
                    CloudBlob file = container.GetBlobReference(fileName);

                    if (await file.ExistsAsync())
                    {
                        await file.DownloadToStreamAsync(ms);
                        Stream blobStream = file.OpenReadAsync().Result;
                        return File(blobStream, file.Properties.ContentType, file.Name);
                    }
                    else
                    {
                        return Content("File does not exist");
                    }
                }
                else
                {
                    return Content("Container does not exist");
                }
            }
            else
            {
                return Content("Error opening storage");
            }
        }


        [Route("DeleteFile/{fileName}")]
        [HttpGet]
        public async Task<bool> DeleteFile(string fileName, ContainerEnum container = ContainerEnum.Container)
        {
            if(container == ContainerEnum.Container)
            {
                return await DeleteContainerFile(fileName, config.Value.Container);

            }
            else if(container == ContainerEnum.CashLogContainer)
            {
                return await DeleteContainerFile(fileName, config.Value.CashLogContainer);

            }
            else if (container == ContainerEnum.CashReceiptContainer)
            {
                return await DeleteContainerFile(fileName, config.Value.CashReceiptContainer);

            }
            else if (container == ContainerEnum.DepositContainer)
            {
                return await DeleteContainerFile(fileName, config.Value.DepositContainer);

            }
            else if (container == ContainerEnum.TransferContainer)
            {
                return await DeleteContainerFile(fileName, config.Value.TransferContainer);
            }
            else if (container == ContainerEnum.AccountCodeContainer)
            {
                return await DeleteContainerFile(fileName, config.Value.AccountCodeContainer);
            }
            else
            {
                return await DeleteContainerFile(fileName, config.Value.Container);
            }
        }

        private async Task<bool> DeleteContainerFile(string fileName, string containerName)
        {
            try
            {
                if (CloudStorageAccount.TryParse(config.Value.StorageConnection, out CloudStorageAccount storageAccount))
                {
                    CloudBlobClient BlobClient = storageAccount.CreateCloudBlobClient();
                    CloudBlobContainer container = BlobClient.GetContainerReference(containerName);

                    if (await container.ExistsAsync())
                    {
                        CloudBlob file = container.GetBlobReference(fileName);

                        if (await file.ExistsAsync())
                        {
                            await file.DeleteAsync();
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}