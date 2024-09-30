namespace Employment_System.Domain.Entities
{
    public class Schedule
    {
        public int ScheduleId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Description { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

    }
}
