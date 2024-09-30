using Employment_System.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employment_System.Repository.Data.Config
{
    public class VacationRequestStatusConfig : IEntityTypeConfiguration<VocationRequestStatus>
    {
        public void Configure(EntityTypeBuilder<VocationRequestStatus> builder)
        {
            // Set the primary key
            builder.HasKey(e => e.StatusId);

            // Seed data
            builder.HasData(
                new VocationRequestStatus { StatusId = 1, StatusName = "Pending" },
                new VocationRequestStatus { StatusId = 2, StatusName = "Approved" },
                new VocationRequestStatus { StatusId = 3, StatusName = "Rejected" },
                new VocationRequestStatus { StatusId = 4, StatusName = "Escalated" },
                new VocationRequestStatus { StatusId = 5, StatusName = "Cancelled" },
                new VocationRequestStatus { StatusId = 6, StatusName = "Completed" }
            );
        }
    }

}
