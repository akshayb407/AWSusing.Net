using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        IAmazonS3 S3Clinent { get; set; }
        public HomeController(IAmazonS3 s3Clinent)
        {
            this.S3Clinent = s3Clinent;
        }
        [HttpPost("CreateFolder")]
        public async Task<int> CreateFolder(string bucketName, string newFolderName, string prefix = "")
        {
            PutObjectRequest request = new PutObjectRequest();
            request.BucketName = bucketName;
            request.Key = (prefix.TrimEnd('/') + "/" + newFolderName.TrimEnd('/') + "/").TrimStart('/');
            var response = await S3Clinent.PutObjectAsync(request);
            return (int)response.HttpStatusCode;


        }

        [HttpPost("UploadFile")]
        public async Task<int> UploadFile(string bucketName, string newFileName, string prefix = "")
        {
            FileInfo fileInfo = new FileInfo(newFileName);
            FileStream fileStream = fileInfo.OpenRead();

            PutObjectRequest request = new PutObjectRequest();
            request.BucketName = bucketName;
            request.Key = (prefix.TrimEnd('/') + "/" + Path.GetFileName(newFileName).TrimEnd('/')).TrimStart('/');
            request.InputStream = fileStream;
            var response = await S3Clinent.PutObjectAsync(request);
            return (int)response.HttpStatusCode;


        }
    }
}