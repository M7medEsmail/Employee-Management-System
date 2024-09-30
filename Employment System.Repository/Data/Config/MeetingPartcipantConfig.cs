using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Employment_System.Domain.Entities;
using System.Reflection.Emit;

namespace Employment_System.Repository.Data.Config
{
    public class MeetingPartcipantConfig : IEntityTypeConfiguration<MeetingParticipant>
    {
        public void Configure(EntityTypeBuilder<MeetingParticipant> builder)
        {
           builder.HasKey(e => new { e.EmployeeId, e.MeetingId });
            builder.HasOne(e => e.Employee)
                 .WithMany(e => e.MeetingParticipants)
                 .HasForeignKey(e => e.EmployeeId);
            builder.HasOne(e => e.Meeting)
                .WithMany(e => e.MeetingParticipants)
                .HasForeignKey(e =>e.MeetingId);

    
        }
    }
}
