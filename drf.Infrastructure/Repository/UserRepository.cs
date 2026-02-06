using dreamify.Domain.Entities;
using drf.Application.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace drf.Infrastructure.Repository;



public class UserRepository: IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUserByRefreshToken(string refreshToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);
        return user;
    }

    public async Task<List<Dream>> GetUserDreams(string userId)
    {
        //return await _context.Users.fin
        
        return await _context.Dreams.Where(dreams => dreams.UserId == Guid.Parse(userId)).ToListAsync(); ;
        
    }

    public async Task DeleteUser(string userId)
    {
        var user = await _context.Users.FindAsync(Guid.Parse(userId));
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }

    // public async Task


}   