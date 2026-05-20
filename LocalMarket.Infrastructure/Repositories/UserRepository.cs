using Mapster;
using LocalMarket.Core.Entities;
using LocalMarket.Core.Interfaces;
using LocalMarket.Infrastructure.Persistence.Models;

namespace LocalMarket.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly Supabase.Client _supabase;

        public UserRepository(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            var result = await _supabase
                .From<UserModel>()
                .Where(u => u.Id == id)
                .Single();

            return result?.Adapt<User>();
        }

        public async Task CreateAsync(User user)
        {
            var model = user.Adapt<UserModel>();
            await _supabase.From<UserModel>().Insert(model);
        }
    }
}
