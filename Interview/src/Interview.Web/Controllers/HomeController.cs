using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
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
            IStorageService storageService = new StorageServiceS3("ded20b31-0bf4-4d39-8d1f-9b8aba09cb38");
            MemoryStream streamMem = new MemoryStream();
            fileInput.CopyTo(streamMem);
            streamMem.Seek(0, SeekOrigin.Begin);
            (string urlPut, string urlGet, DateTime expiration) accessCredentials = await storageService.UploadStreamLimited(streamMem, fileInput.ContentType, fileInput);

            try
            {
                if (emailAddress != null)
                {
                    string senderAddress = "david@dsergio.co";
                    string receiverAddress = emailAddress;
                    string htmlBody = accessCredentials.urlGet;
                    string textBody = accessCredentials.urlGet;

                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("mail.dsergio.co");

                    mail.From = new MailAddress(senderAddress);
                    mail.To.Add(receiverAddress);
                    mail.Subject = "David Sergio Interview - You uploaded a file...";
                    mail.Body = accessCredentials.urlGet;

                    string? user = System.Environment.GetEnvironmentVariable("email_user");
                    string? pass = System.Environment.GetEnvironmentVariable("email_pass");

                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential(user, pass);
                    SmtpServer.EnableSsl = true;

                    SmtpServer.Send(mail);
                    Console.WriteLine("mail Send");

                    mail.Dispose();
                    SmtpServer.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            ViewBag.fileName = fileInput.FileName;
            ViewBag.emailAddress = emailAddress;
            ViewBag.url = accessCredentials.urlGet;
            ViewBag.expiration = accessCredentials.expiration;

            return View();
        }

    }
}