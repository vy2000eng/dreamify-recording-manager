using System.Security.Claims;
using dreamify.Domain.Entities;
using drf.Application.Abstracts;
using drf.Application.Services;
using drf.Domain.Requests;
using drf.Domain.Response;
using drf.Infrastructure.Options;
using drf.Infrastructure.Repository;
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

    [HttpGet("download/{dreamId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IResult> DownloadDream(string dreamId)
    {
        
        var dream = await _databaseService.GetDream(User, dreamId);
        if (dream == null)
        {
            return Results.NotFound(new { error = "Dream not found" });
        }
        var memoryStream = await _bucketService.DownloadDreamFromS3Bucket(User, dream.FileName);
        memoryStream.Position = 0;
        return Results.File(memoryStream, "audio/m4a", $"{dream.FileName}.m4a");

    }

    [HttpGet("dreamsMetaData")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IResult> GetDreamMetaData()
    {
        return Results.Ok(new DreamMetaDataResponse
        {
            Dreams = await _databaseService.GetDreamMetaData(User)

        });


    }
    
    
}