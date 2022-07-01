using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;

namespace TheWardrobe.API.Repositories
{
  public interface IImageUploadRepository
  {
    public string UploadImageToS3(Stream fileStream, string fileNameInS3);
  }

  public class ImageUploadRepository : IImageUploadRepository
  {
    private readonly string BucketName = "the-wardrobe-media";
    private readonly AmazonS3Client _s3Client;

    public ImageUploadRepository()
    {
      _s3Client = new AmazonS3Client("AKIA3YIUBBFU3AGYTZXD", "VPpcyS4nxfxH/tajHDh5EplTGKvhghm4WvwBJxuH", RegionEndpoint.EUCentral1);
    }
    public string UploadImageToS3(Stream fileStream, string fileNameInS3)
    {
      var utility = new TransferUtility(_s3Client);
      var request = new TransferUtilityUploadRequest();

      request.BucketName = BucketName;
      request.Key = fileNameInS3; //file name up in S3  
      request.InputStream = fileStream;
      utility.Upload(request); //commensing the transfer  

      return $"https://{BucketName}.s3.eu-central-1.amazonaws.com/{fileNameInS3}";
    }
  }
}