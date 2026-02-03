using System.Security.Claims;
using dreamify.Domain.Entities;
using drf.Domain.Requests;

namespace drf.Application.Abstracts;

public interface IDatabaseService
{
    Task AddDreamToDataBase(ClaimsPrincipal claimsPrincipal, UploadRequest uploadRequest);

    Task<Dream?> GetDream(ClaimsPrincipal claimsPrincipal, string dreamId);
    Task<List<Dream>> GetDreamMetaData(ClaimsPrincipal claimsPrincipal);
    Task UpdateDream(ClaimsPrincipal claimsPrincipal, UpdateDreamRequest request);
    
    Task DeleteDream(ClaimsPrincipal claimsPrincipal, string dreamId);
}