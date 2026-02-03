using System.Security.Claims;
using Amazon.S3.Transfer;
using dreamify.Domain.Entities;
using drf.Application.Abstracts;
using drf.Domain.Exceptions;
using drf.Domain.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace drf.Application.Services;

public class BucketService:IBucketService
{

    private readonly IS3Processor _s3Processor;
    //private readonly 
    public BucketService(IS3Processor s3Processor)
    {
        _s3Processor = s3Processor;
        
        
    }



    public async Task UploadDreamToS3Bucket(ClaimsPrincipal claimsPrincipal, UploadRequest uploadRequest)
    {
        var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value
                     ?? claimsPrincipal.FindFirst("sub")?.Value;
        if (userId == null)
        {
            throw new UserNotFoundException();
        }

        await _s3Processor.UploadToS3(userId, uploadRequest);
    }

    public async Task<MemoryStream> DownloadDreamFromS3Bucket(ClaimsPrincipal claimsPrincipal,string fileName)
    {
        var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value
                     ?? claimsPrincipal.FindFirst("sub")?.Value;
        if (userId == null)
        {
            throw new UserNotFoundException();
        }
        
        return await _s3Processor.DownloadFromS3(userId, fileName );
        
    }

    public async Task DeleteDreamFromS3Bucket(ClaimsPrincipal claimsPrincipal, string fileName)
    {
        var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value
                     ?? claimsPrincipal.FindFirst("sub")?.Value;
        if (userId == null)
        {
            throw new UserNotFoundException();
        }
        await _s3Processor.DeleteFromS3(userId, fileName);
    }
}