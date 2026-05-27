

using LocalMarket.Core.Entities;
using LocalMarket.Core.Interfaces;
using LocalMarket.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LocalMarket.Infrastructure.Repositories
{
    public class ConversationRepository : IConversationRepository
    {

        private readonly AppDbContext _dbContext;

        public ConversationRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Conversation?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Conversations.FindAsync(id);
        }

        public async Task<Conversation?> GetByBusinessAndClientAsync(Guid businessId, Guid clientId)
        {
            return await _dbContext.Conversations
                .AsNoTracking()
                .FirstOrDefaultAsync(c =>
                    c.BusinessId == businessId && c.ClientId == clientId);
        }
        public async Task<List<Conversation>> GetByUserIdAsync(Guid userId)
        {
            return await _dbContext.Conversations
               .AsNoTracking()
               .Where(c => c.ClientId == userId)
               .OrderByDescending(c => c.LastMessageAt ?? c.CreatedAt)
               .ToListAsync();
        }
        public async Task<List<Conversation>> GetByBusinessIdAsync(Guid businessId)
        {
            return await _dbContext.Conversations
                .AsNoTracking()
                .Where(c => c.BusinessId == businessId)
                .OrderByDescending(c => c.LastMessageAt ?? c.CreatedAt)
                .ToListAsync();
        }
        public async Task<Conversation> CreateAsync(Conversation conversation)
        {
            await _dbContext.Conversations.AddAsync(conversation);
            await _dbContext.SaveChangesAsync();
            return conversation;
        }                        

        public async Task UpdateLastMessageAtAsync(Guid conversationId, DateTime sentAt)
        {
            await _dbContext.Conversations
                 .Where(c => c.Id == conversationId)
                 .ExecuteUpdateAsync(s =>
                     s.SetProperty(c => c.LastMessageAt, sentAt));
        }
    }
}
