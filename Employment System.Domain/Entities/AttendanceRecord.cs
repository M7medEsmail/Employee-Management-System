namespace Employment_System.Domain.Entities
{
    public class AttendanceRecord
    {
        public int AttendanceRecordId { get; set; }
        public DateTime ChickIn { get; set; }
        public DateTime? ChickOut { get; set; } // nullabe of current day

        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
