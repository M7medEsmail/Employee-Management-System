using Employment_System.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Employment_System.Repository.Data
{
    public class EmpManageDbContext : IdentityDbContext<AppUser>
    {
        public EmpManageDbContext(DbContextOptions<EmpManageDbContext> options):base(options) 
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            // call FLuent Api (Call All Class that implement IEntityTypeConfigration)
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); //call all class that implement IEntityTypeConfiguration

        }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<AttendanceRecord> AttendanceRecords { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<MeetingParticipant> MeetingParticipants { get; set; }
        public DbSet<VocationRequest> VocationRequests { get; set; }
        public DbSet<VocationRequestStatus> VocationRequestStatuses { get; set; }





    }
}
