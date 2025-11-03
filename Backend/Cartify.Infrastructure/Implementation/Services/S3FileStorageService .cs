using Amazon.S3;
using Amazon.S3.Model;
using Cartify.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
namespace Cartify.Infrastructure.Implementation.Services
{
    public class S3FileStorageService : IFileStorageService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;

        public S3FileStorageService(IAmazonS3 s3Client, IConfiguration config)
        {
            _s3Client = s3Client ?? throw new ArgumentNullException(nameof(s3Client));
            _bucketName = config["AWS:BucketName"]
                          ?? throw new ArgumentNullException("AWS:BucketName not found in config");
        }

        public async Task<string> UploadFileAsync(IFormFile file, string folder)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Empty file");

            if (string.IsNullOrWhiteSpace(folder))
                folder = "misc";

            var key = $"{folder}/{Guid.NewGuid()}_{file.FileName}";

            using var stream = file.OpenReadStream();

            var request = new Amazon.S3.Model.PutObjectRequest
            {
                BucketName = _bucketName,
                Key = key,
                InputStream = stream,
                ContentType = file.ContentType,
               // CannedACL = S3CannedACL.PublicRead
            };

            await _s3Client.PutObjectAsync(request);

            return $"https://{_bucketName}.s3.amazonaws.com/{key}";
        }
    }
}