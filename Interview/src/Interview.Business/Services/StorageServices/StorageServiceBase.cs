using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Interview.Business.Services.StorageServices
{
	public abstract class StorageServiceBase : IStorageService, IDisposable
	{
		private bool _IsDisposed;
		public abstract Task<string> UploadStreamPublic(Stream stream);
        public abstract Task<(string, string, DateTime)> UploadStreamLimited(Stream stream, string contentType, IFormFile file);
        public abstract IDisposable GetClient();

        // Dispose() calls Dispose(true)
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected virtual void Dispose(bool disposing)
        {
            if (_IsDisposed) return;

            if (disposing)
            {
                // free managed resources
                GetClient().Dispose();
            }

            // free native resources if there are any.

            _IsDisposed = true;
        }

        // NOTE: Leave out the finalizer altogether if this class doesn't
        // own unmanaged resources, but leave the other methods
        // exactly as they are.
        ~StorageServiceBase()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }
    }
}
