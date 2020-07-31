using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Project1.Web.Api;
using Project1.Business.Services.StorageServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using RestSharp;

namespace Project1.Web.Controllers
{
    public class ModelObjectsController : Controller
    {
        public ModelObjectsController(IHttpClientFactory clientFactory)
        {
            //HttpClient httpClient = clientFactory?.CreateClient("Project1Api") ?? throw new ArgumentNullException(nameof(clientFactory));
            //Client = new ModelObjectClient(httpClient);


            ClientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
        }

        public IHttpClientFactory ClientFactory { get; }

        //private ModelObjectClient Client { get; }

        public async Task<ActionResult> Index()
        {
            //ICollection<ModelObject> modelObjects = await Client.GetAllAsync();
            //return View(modelObjects);

            HttpClient httpClient = ClientFactory.CreateClient("Project1Api");

            var client = new ModelObjectClient(httpClient);
            ICollection<ModelObject> modelObjects = await client.GetAllAsync();
            return View(modelObjects);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(ModelObjectInput modelObjectInput)
        {
            ActionResult result = View(modelObjectInput);

            if (ModelState.IsValid)
            {
                HttpClient httpClient = ClientFactory.CreateClient("Project1Api");

                var client = new ModelObjectClient(httpClient);
                var createdAuthor = await client.PostAsync(modelObjectInput);

                result = RedirectToAction(nameof(Index));
            }

            return result;
        }

        public async Task<ActionResult> Edit(int id)
        {
            HttpClient httpClient = ClientFactory.CreateClient("Project1Api");

            var client = new ModelObjectClient(httpClient);
            var fetchedGift = await client.GetAsync(id);

            return View(fetchedGift);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int id, ModelObjectInput modelObjectInput)
        {
            HttpClient httpClient = ClientFactory.CreateClient("Project1Api");

            var client = new ModelObjectClient(httpClient);
            var updatedAuthor = await client.Put3Async(id, modelObjectInput);

            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> Delete(int id)
        {
            HttpClient httpClient = ClientFactory.CreateClient("Project1Api");

            var client = new ModelObjectClient(httpClient);

            await client.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }





        public async Task<ActionResult> Upload(int id)
        {
            HttpClient httpClient = ClientFactory.CreateClient("Project1Api");

            var client = new ModelObjectClient(httpClient);
            var fetchedModelObject = await client.GetAsync(id);

            return View(fetchedModelObject);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> Upload(int id, [FromForm] IFormFile fileInput)
        {
            HttpClient httpClient = ClientFactory.CreateClient("Project1Api");
            IStorageService storageService = new StorageServiceS3("ded20b31-0bf4-4d39-8d1f-9b8aba09cb38");

            MemoryStream streamMem = new MemoryStream();
            fileInput.CopyTo(streamMem);
            streamMem.Seek(0, SeekOrigin.Begin);

            (string urlPut, string urlGet, DateTime expiration) accessCredentials = await storageService.UploadStreamLimited(streamMem, fileInput.ContentType, fileInput);


            var client = new ModelObjectClient(httpClient);
            await client.Put2Async(id, accessCredentials.urlGet, accessCredentials.expiration);

            return RedirectToAction(nameof(Index));

        }
    }
}