using System.Security.Claims;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using drf.Application.Abstracts;
using drf.Domain.Requests;
using drf.Infrastructure.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace drf.Infrastructure.Processors;

public class S3Processor:IS3Processor
{
    
    
    private readonly AwsOptions _awsOptions;
    private readonly IAmazonS3 _s3Client;

    
    
    public S3Processor(IOptions<AwsOptions> awsOptions, IAmazonS3 s3Client)
    {
        _awsOptions = awsOptions.Value;
        _s3Client = s3Client;
        
    }

    public async Task UploadToS3(string userId, UploadRequest request)
    {
        
        var key = $"recordings/{userId}/{request.FileName}";
        
        using var stream = request.File.OpenReadStream();
        
        
        
        var uploadRequest = new TransferUtilityUploadRequest
        {
            InputStream = stream,
            BucketName = _awsOptions.Bucket,
            Key = key,
            ContentType = "audio/m4a"
        };
        
        var transferUtility = new TransferUtility(_s3Client);
        await transferUtility.UploadAsync(uploadRequest);
    }

    public async Task<MemoryStream> DownloadFromS3(string userId, string fileName)
    {
        var key = $"recordings/{userId}/{fileName}";
        var getRequest = new GetObjectRequest
        {
            BucketName = _awsOptions.Bucket,
            Key = key
        };
        using var response = await _s3Client.GetObjectAsync(getRequest);
        using var responseStream = response.ResponseStream;
        
        var memoryStream = new MemoryStream();
        await responseStream.CopyToAsync(memoryStream);
        return memoryStream;

        
        
    }

    public async Task DeleteFromS3(string userId, string fileName)
    {
        var key = $"recordings/{userId}/{fileName}";
        var deleteRequest = new DeleteObjectRequest
        {
            BucketName = _awsOptions.Bucket,
            Key = key
        };

        await _s3Client.DeleteObjectAsync(deleteRequest); //GetObjectAsync(getRequest); 


    }
}