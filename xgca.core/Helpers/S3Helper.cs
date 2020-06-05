using System;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;


namespace xgca.core.Helpers
{
    public class S3Helper
    {
        public static async Task<string> UploadObject(IFormFile file, Models.S3.Variables s3Variables) 
        {
            var client = new AmazonS3Client(s3Variables.AccessKey, s3Variables.SecretKey, RegionEndpoint.GetBySystemName(s3Variables.Region));

            byte[] fileBytes = new Byte[file.Length];
            file.OpenReadStream().Read(fileBytes, 0, Int32.Parse(file.Length.ToString()));
            
            var fileName = Guid.NewGuid() + file.FileName;

            PutObjectResponse response = null;

            using (var stream = new MemoryStream(fileBytes))
            {
                var request = new PutObjectRequest
                {
                    BucketName = s3Variables.S3Bucket,
                    Key = fileName,
                    InputStream = stream,
                    ContentType = file.ContentType,
                    CannedACL = S3CannedACL.PublicRead
                };

                response = await client.PutObjectAsync(request);
            };
            
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return $"https://{s3Variables.S3Bucket}.s3.{s3Variables.Region}.amazonaws.com/{fileName}";
            }
            else
            {
                return null;
            }
        }
    }
}