using System.Security.Claims;
using Amazon.S3.Transfer;
using dreamify.Domain.Entities;
using drf.Application.Abstracts;
using drf.Domain.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace drf.Application.Services;

public class BucketService:IBucketService
{
    
    //public readonly // ApplicationDbContext context;


    private readonly IS3Processor _s3Processor;
    //private readonly 
    public BucketService(IS3Processor s3Processor)
    {
        //_dreamsRepository = dreamsRepository;
        _s3Processor = s3Processor;
        
        
    }



    public async Task UploadDreamToS3Bucket(ClaimsPrincipal claimsPrincipal, UploadRequest uploadRequest)
    {
        var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value
                     ?? claimsPrincipal.FindFirst("sub")?.Value;
        
        await _s3Processor.UploadToS3(userId,uploadRequest);

        
        
        

        
    }
    
    
    
}