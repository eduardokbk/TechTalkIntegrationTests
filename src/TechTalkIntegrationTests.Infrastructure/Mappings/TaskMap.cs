using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechTalkIntegrationTests.Domain.Models.Entities;

namespace TechTalkIntegrationTests.Infrastructure.Mappings
{
    public class TaskMap : IEntityTypeConfiguration<TaskDomain>
    {
        public void Configure(EntityTypeBuilder<TaskDomain> builder)
        {
            builder.ToTable("Task");

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Priority)
                .IsRequired();

            builder.Property(x => x.Completed)
                .IsRequired();
        }
    }
}
