using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CORTNE.Service.PDF;
using Microsoft.AspNetCore.Mvc;

namespace CORTNE.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PDFController : Controller
    {
        private readonly IStampService _stampService;

        public PDFController(IStampService stampService)
        {
            _stampService = stampService;
        }

        [HttpPost]
        [Route("stamp")]
        [Produces(contentType: "application/pdf")]
        public FileStreamResult Stamp([FromForm] StampRequestForm stampRequestForm)
        {
            Stream outStream = _stampService.ApplyStamp(stampRequestForm);
            outStream.Position = 0;
            return new FileStreamResult(outStream, "application/pdf");
        }
    }
}