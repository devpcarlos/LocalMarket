using LocalMarket.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace LocalMarket.Infrastructure.Persistence.Configuration
{
    public class ScheduleConfig : IEntityTypeConfiguration<Schedule>
    {
        public void Configure(EntityTypeBuilder<Schedule> builder)
        {
                builder.ToTable("schedules");
                builder.HasKey(x => x.Id);
                builder.HasIndex(x => new { x.BusinessId, x.DayOfWeek }).IsUnique();
                builder.Property(x => x.IsClosed).HasDefaultValue(false);
                builder.HasOne<Business>().WithMany().HasForeignKey(x => x.BusinessId).OnDelete(DeleteBehavior.Cascade);
 
        }
    }
}
