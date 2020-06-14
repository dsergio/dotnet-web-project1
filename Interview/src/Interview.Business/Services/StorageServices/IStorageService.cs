using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Interview.Business.Services.StorageServices
{
	public interface IStorageService
	{
		public Task<string> UploadStreamPublic(Stream stream);
		public Task<(string, string, DateTime)> UploadStreamLimited(Stream stream, string contentType, IFormFile file);
		public IDisposable GetClient();
	}
}
