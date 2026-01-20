using System.Security.Claims;
using dreamify.Domain.Entities;
using drf.Application.Abstracts;
using drf.Application.Services;
using drf.Domain.Requests;
using drf.Infrastructure.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;

namespace dreamify_recording_manager.Controllers;
using Amazon.S3;
using Amazon.S3.Transfer;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/[controller]")]
[DisableRequestSizeLimit]  // ← Add this
[RequestFormLimits(MultipartBodyLengthLimit = 209715200)]

public class BucketController:ControllerBase
{
    //private readonly AwsOptions _awsOptions;
    private readonly IBucketService _bucketService;
    private readonly IDatabaseService _databaseService;

    public BucketController(IBucketService bucketService,IDatabaseService databaseService)
    {
        //_awsOptions = awsOptions.Value;
        _bucketService = bucketService;
        _databaseService = databaseService;
    }
    
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost("upload")]

    public async Task<IResult> UploadDream(UploadRequest request)
    {
        
        
        if (request.File == null || request.File.Length == 0)
            return Results.BadRequest("No file");

        try
        {
            await _bucketService.UploadDreamToS3Bucket(User, request); 
            await _databaseService.AddDreamToDataBase(User, request);
            return Results.Ok(200);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Results.BadRequest("An Unexpected error occured while uploading file to S3 bucket");
        }
    }
    
   // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    // [HttpGet("download/{dreamId}")]
    // public IActionResult GetDownloadUrl(string dreamId)
    // {
    //     var key = $"recordings/{dreamId}.aac";
    //     
    //     var request = new GetPreSignedUrlRequest
    //     {
    //         BucketName = _awsOptions.Bucket,
    //         Key = key,
    //         Expires = DateTime.UtcNow.AddHours(1)
    //     };
    //     
    //     var url = _s3Client.GetPreSignedURL(request);
    //     
    //     return Ok(new { downloadUrl = url });
    // }
    
    // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    // [HttpDelete("{dreamId}")]
    // public async Task<IActionResult> DeleteDream(string dreamId)
    // {
    //     var key = $"recordings/{dreamId}.aac";
    //     
    //     await _s3Client.DeleteObjectAsync(_awsOptions.Bucket, key);
    //     
    //     return Ok();
    // }
    
}