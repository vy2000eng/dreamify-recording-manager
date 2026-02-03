using dreamify.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace drf.Application.Abstracts;

public interface IDreamsRepository
{
    public Task AddDream(Dream dream);
    
    public Task<Dream> GetDream(string dreamId);

    public  Task UpdateDream(Dream dream);
    
    public Task DeleteDream(Dream dream);

}