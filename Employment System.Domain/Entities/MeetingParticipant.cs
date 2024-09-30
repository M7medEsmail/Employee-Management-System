using System.Reflection.Metadata.Ecma335;

namespace Employment_System.Domain.Entities
{
    public class MeetingParticipant
    {
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public int MeetingId { get; set; }
        public Meeting Meeting { get; set; }
    }
}
