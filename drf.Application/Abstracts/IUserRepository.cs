using dreamify.Domain.Entities;

namespace drf.Application.Abstracts;

public interface IUserRepository
{
    Task<User?> GetUserByRefreshToken(string refreshToken);
}