using System.Security.Claims;
using drf.Domain.Requests;

namespace drf.Application.Abstracts;

public interface IBucketService
{
    public Task UploadDreamToS3Bucket(ClaimsPrincipal claimsPrincipal, UploadRequest uploadRequest);
}