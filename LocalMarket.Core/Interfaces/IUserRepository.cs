using LocalMarket.Core.Entities;

namespace LocalMarket.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task CreateAsync(User user);

        Task<User?> GetByEmailAsync(string email);
    }
}
