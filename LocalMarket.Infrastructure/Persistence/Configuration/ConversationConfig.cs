using LocalMarket.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocalMarket.Infrastructure.Persistence.Configuration
{
    public class ConversationConfig : IEntityTypeConfiguration<Conversation>
    {
        public void Configure(EntityTypeBuilder<Conversation> builder)
        {
                builder.ToTable("conversations");
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => new { x.BusinessId, x.ClientId }).IsUnique();
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("NOW()");
            builder.HasOne<Business>().WithMany().HasForeignKey(x => x.BusinessId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne<User>().WithMany().HasForeignKey(x => x.ClientId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
