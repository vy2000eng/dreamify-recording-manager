using dreamify.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace drf.Application.Abstracts;

public interface IDreamsRepository
{
    public Task AddDream(Dream dream);
}