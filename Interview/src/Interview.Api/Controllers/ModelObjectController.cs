using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Interview.Business.Dto;
using Interview.Business.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Interview.Api.Controllers
{
    public class ModelObjectController : BaseApiController<ModelObject, ModelObjectInput>
    {
        public ModelObjectController(IModelObjectService modelObjectService)
            : base(modelObjectService)
        { }

        [HttpPut("upload/{id}")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Put(int id, [FromForm] IFormFile file)
        {
            ModelObject entity = await Service.FetchByIdAsync(id);
            if (entity is null)
            {
                return NotFound();
            }
            else
            {
                if (file.Length > 0)
                {
                    await Service.UploadAsync(id, file);
                }
                return Ok("tbd");
            }
        }

        [HttpPut("upload-link/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Put(int id, string link, DateTime? expiration)
        {
            ModelObject entity = await Service.FetchByIdAsync(id);
            if (entity is null)
            {
                return NotFound();
            }
            else
            {
                await Service.UploadAsync(id, link, expiration);
                return Ok();
            }

        }
    }
}