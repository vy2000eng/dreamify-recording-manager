using dreamify.Domain.Entities;
using drf.Application.Abstracts;
using drf.Domain.Exceptions;
using drf.Domain.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

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

    public async Task<Dream> GetDream(string dreamId)
    {
        var dream = await _context.Dreams.Where(dream => dream.LocalDreamId == dreamId).FirstOrDefaultAsync();//FindAsync(dream > dream.)//FindAsync(dreamId);
        if( dream == null)
        {
            throw new DreamNotFoundException();

        }
        return dream;
    }

    public async Task UpdateDream(Dream dream)
    {
          _context.Dreams.Update(dream);
          await _context.SaveChangesAsync();

    }
    
    
    
}