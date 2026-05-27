
using LocalMarket.Core.Entities;
using LocalMarket.Core.Interfaces;
using LocalMarket.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LocalMarket.Infrastructure.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly AppDbContext _dbContext;

        public MessageRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Message>> GetByConversationIdAsync(Guid conversationId)
        {
            return await _dbContext.Messages
                .AsNoTracking()
                .Where(m => m.ConversationId == conversationId)
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }
        public async  Task<Message> CreateAsync(Message message)
        {
            await _dbContext.Messages.AddAsync(message);
            await _dbContext.SaveChangesAsync();
            return message;
        }       

        public async  Task MarkAsReadAsync(Guid conversationId, Guid readerId)
        {
            await _dbContext.Messages
                .Where(m => m.ConversationId == conversationId
                    && m.SenderId != readerId
                    && !m.IsRead)
                .ExecuteUpdateAsync(s =>
                    s.SetProperty(m => m.IsRead, true));
        }
    }
}
