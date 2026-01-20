using dreamify.Domain.Entities;
using drf.Application.Abstracts;
using drf.Domain.Requests;
using Microsoft.AspNetCore.Http;

namespace drf.Infrastructure.Repository;

public class DreamsRepository:IDreamsRepository
{
    private readonly ApplicationDbContext _context;

    public DreamsRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddDream(Dream dream)
    {
            _context.Dreams.Add(dream);
            await _context.SaveChangesAsync();

    }
}