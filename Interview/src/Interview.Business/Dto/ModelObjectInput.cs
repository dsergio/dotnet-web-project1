using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Interview.Business.Dto
{
	public class ModelObjectInput
	{
        [Required]
        [EmailAddress]
        public string? EmailAddress { get; set; }

        //[Required]
        public string? LinkToFile { get; set; }

        public DateTime? Expiration { get; set; }

        //[Required]
        //public IFormFile? FormFile { get; set; }
    }
}
