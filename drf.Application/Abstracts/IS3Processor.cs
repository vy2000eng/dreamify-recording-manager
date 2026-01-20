using System.Security.Claims;
using drf.Domain.Requests;

namespace drf.Application.Abstracts;

public interface IS3Processor
{
    public Task UploadToS3(string userId, UploadRequest request);
}