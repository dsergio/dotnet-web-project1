using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Interview.Web.Api
{
	[ModelMetadataType(typeof(ModelObjectInputMetadata))]
	public partial class ModelObjectInput
	{

	}

	public class ModelObjectInputMetadata
	{
		[Display(Name = "Email Address")]
		[Required]
		[EmailAddress]
		public string? EmailAddress { get; set; }

		[Display(Name = "Link to File")]
		public string? LinkToFile { get; set; }

		[Display(Name = "Link Expiration")]
		public string? Expiration { get; set; }
	}
}
