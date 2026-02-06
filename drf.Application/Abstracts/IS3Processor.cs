using System.Security.Claims;
using drf.Domain.Requests;
using Microsoft.AspNetCore.Http;

namespace drf.Application.Abstracts;

public interface IS3Processor
{
    public Task UploadToS3(string userId, UploadRequest request);
    public Task<MemoryStream> DownloadFromS3(string userId, string fileName);
    
    public Task DeleteFromS3(string userId, string fileName);
    
    public Task DeleteAllUserDataFromS3(string userId);
}