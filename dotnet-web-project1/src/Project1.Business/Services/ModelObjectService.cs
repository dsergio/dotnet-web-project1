using AutoMapper;
using Project1.Business.Services.StorageServices;
using Project1.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Project1.Business.Services
{
    public class ModelObjectService : EntityService<Dto.ModelObject, Dto.ModelObjectInput, Data.ModelObject>, IModelObjectService
    {
        public ModelObjectService(ApplicationDbContext dbContext, IMapper mapper)
            : base(dbContext, mapper)
        { }

        public override async Task<bool> UploadAsync(int id, IFormFile file)
        {
            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            using Stream streamMem = new MemoryStream();
            file.CopyTo(streamMem);
            streamMem.Seek(0, SeekOrigin.Begin);

            IStorageService storageService = new StorageServiceS3("ded20b31-0bf4-4d39-8d1f-9b8aba09cb38");
            string link = await storageService.UploadStreamPublic(streamMem);

            
            storageService.GetClient().Dispose();

            return await UploadAsync(id, link, null);
        }

        public override async Task<bool> UploadAsync(int id, String link, DateTime? expiration)
        {

            Dto.ModelObject entity = await FetchByIdAsync(id);
            entity.LinkToFile = link;
            entity.Expiration = expiration;

            Dto.ModelObjectInput updateEntity = Mapper.Map<Dto.ModelObject, Dto.ModelObjectInput>(entity);
            await UpdateAsync(id, updateEntity);


            try
            {
                if (entity.EmailAddress != null)
                {
                    string senderAddress = "david@dsergio.co";
                    string receiverAddress = entity.EmailAddress;
                    string htmlBody = link;
                    string textBody = link;

                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("mail.dsergio.co");

                    mail.From = new MailAddress(senderAddress);
                    mail.To.Add(receiverAddress);
                    mail.Subject = "David Sergio Project1 - You uploaded a file...";
                    mail.Body = link;

                    string? user = System.Environment.GetEnvironmentVariable("email_user");
                    string? pass = System.Environment.GetEnvironmentVariable("email_pass");

                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential(user, pass);
                    SmtpServer.EnableSsl = true;

                    SmtpServer.Send(mail);
                    mail.Dispose();
                    SmtpServer.Dispose();
                }
            }
            catch (SmtpException ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Dto.ModelObjectInput newEntity = Mapper.Map<Dto.ModelObject, Dto.ModelObjectInput>(entity);
            await UpdateAsync(id, newEntity);

            return false;
        }
    }
}
