using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Project1.Business.Services.StorageServices
{
    /// <summary>
    /// S3 Storage Implementation
    /// Defaults to Region US-West-2
    /// 
    /// </summary>
	public class StorageServiceS3 : StorageServiceBase
    {
        private readonly AmazonS3Client _Client;
        public string BucketName { get; }
        public RegionEndpoint Region { get; }

        public StorageServiceS3(string bucketName) : this(RegionEndpoint.USWest2, bucketName)
        { }

        public StorageServiceS3(RegionEndpoint region, string bucketName)
        {
            _Client = new Amazon.S3.AmazonS3Client(region);
            BucketName = bucketName;
            Region = region;
        }

        public override IDisposable GetClient()
        {
            return _Client;
        }

        /// <summary>
        /// S3 Presigned URL Limited Access Upload Implementaion. Defaults to 10 minute expiration.
        /// </summary>
        /// <param name="fileStream"></param>
        /// <returns>(urlPut, urlGet, expiration)</returns>
        public override async Task<(string, string, DateTime)> UploadStreamLimited(Stream fileStream, string contentType, IFormFile file)
        {
            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            string keyName = "" + Guid.NewGuid();
            (string urlPut, string urlGet, DateTime expiration) accessCredentials;
            accessCredentials = GeneratePreSignedURL(keyName, 10, contentType); // expires 10 minuntes from time of upload
            await UploadPresignedUrl(accessCredentials.urlPut, fileStream, contentType, file);
            return accessCredentials;
        }

        private async Task UploadPresignedUrl(string url, Stream fileStream, string contentType, IFormFile file)
        {
            if (url is null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            if (fileStream is null)
            {
                throw new ArgumentNullException(nameof(fileStream));
            }

            HttpWebRequest? httpRequest = WebRequest.Create(url) as HttpWebRequest;

            if (httpRequest is HttpWebRequest)
            {
                httpRequest.Method = "PUT";
                httpRequest.ContentType = file.ContentType;
                
                using (Stream dataStream = httpRequest.GetRequestStream())
                {
                    var buffer = new byte[8000];
                    int bytesRead = 0;
                    while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        dataStream.Write(buffer, 0, bytesRead);
                    }

                    await httpRequest.GetResponseAsync();
                }
                
            }

            
        }

        /// <summary>
        /// Generate S3 Presigned URL
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expirationTimeMinutes"></param>
        /// <returns>(urlPut, urlGet, Expiration)</returns>
        private (string, string, DateTime) GeneratePreSignedURL(string key, int expirationTimeMinutes, string contentType)
        {
            DateTime expiration = DateTime.Now.AddMinutes(expirationTimeMinutes);

            var requestGet = new GetPreSignedUrlRequest
            {
                BucketName = BucketName,
                Key = key,
                Verb = HttpVerb.GET,
                Expires = expiration
            };

            var requestPut = new GetPreSignedUrlRequest
            {
                BucketName = BucketName,
                Key = key,
                Verb = HttpVerb.PUT,
                Expires = expiration
            };

            string urlPut = _Client.GetPreSignedURL(requestPut);
            string urlGet = _Client.GetPreSignedURL(requestGet);
            return (urlPut, urlGet, expiration);
        }

        /// <summary>
        /// S3 Public Access Upload Implementation. This method simply uploads a file to S3, without generating Presigned URL. 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns>Public URL</returns>
        public override async Task<string> UploadStreamPublic(Stream stream)
        {
            string keyName = "" + Guid.NewGuid();

            PutObjectRequest request = new PutObjectRequest
            {
                BucketName = BucketName,
                Key = keyName,
                InputStream = stream,
                CannedACL = S3CannedACL.PublicRead
            };

            await _Client.PutObjectAsync(request);

            string link = "https://" + BucketName + ".s3-" + Region.SystemName + ".amazonaws.com/" + keyName;
            return link;
        }
        
        public async Task<IEnumerable<string>> GetListBucketNames()
        {
            ListBucketsRequest request = new ListBucketsRequest();
            ListBucketsResponse response;

            List<string> bucketNames = new List<string>();

            response = await _Client.ListBucketsAsync(request);

            if (response.Buckets.Count > 0)
            {
                foreach (S3Bucket b in response.Buckets)
                {
                    bucketNames.Add(b.BucketName);
                }
            }

            return bucketNames;
        }
    }
}
