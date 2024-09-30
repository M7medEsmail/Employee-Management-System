namespace Employment_System.Domain.Entities
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int? ManagerId { get; set; }
        public Employee Manager { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public ICollection<VocationRequest> VocationRequests { get; set; }
        public ICollection<AttendanceRecord> AttendanceRecords { get; set; }
        public ICollection<MeetingParticipant> MeetingParticipants { get; set; }
        public ICollection<Schedule> Schedules { get; set; }





    }
}
