using System.Security.Claims;
using drf.Domain.Requests;

namespace drf.Application.Abstracts;

public interface IDatabaseService
{
    Task AddDreamToDataBase(ClaimsPrincipal claimsPrincipal, UploadRequest uploadRequest);

}