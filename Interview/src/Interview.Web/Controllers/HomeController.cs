using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Interview.Business.Services.StorageServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Interview.Web.Controllers
{
    public class HomeController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> ProcessForm(string emailAddress, [FromForm] IFormFile fileInput)
        {
            //IStorageService storageService = new StorageServiceS3("ded20b31-0bf4-4d39-8d1f-9b8aba09cb38");
            //MemoryStream streamMem = new MemoryStream();
            //fileInput.CopyTo(streamMem);
            //streamMem.Seek(0, SeekOrigin.Begin);
            //(string urlPut, string urlGet, DateTime expiration) accessCredentials = await storageService.UploadStreamLimited(streamMem, fileInput.ContentType, fileInput);

            return Ok("email: " + emailAddress + " file: " + fileInput.FileName);
        }

    }
}