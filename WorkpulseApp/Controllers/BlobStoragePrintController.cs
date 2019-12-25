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

namespace CORTNE.Controllers
{
    //[Authorize]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class BlobStoragePrintController : ControllerBase
    {
        private readonly IOptions<BlobPrintConfig> config;


        public BlobStoragePrintController(IOptions<BlobPrintConfig> config)
        {
            this.config = config;
        }

       
        [HttpPost("InsertFile")]
        public async Task<IActionResult> InsertFile(IFormFile asset)
        {
            string filurl = string.Empty;
            try
            {
                if (CloudStorageAccount.TryParse(config.Value.StorageConnection, out CloudStorageAccount storageAccount))
                {
                    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                    CloudBlobContainer container = blobClient.GetContainerReference(config.Value.Container);

                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(asset.FileName);

                    await blockBlob.UploadFromStreamAsync(asset.OpenReadStream());

                    filurl = blockBlob.StorageUri.PrimaryUri.ToString();
                    return Ok(filurl);
                }
                else
                {
                    return Ok(filurl);
                }
            }
            catch
            {
                return Ok(filurl);
            }
        }


    }
}