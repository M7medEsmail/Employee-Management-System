using Employment_System.Domain.Entities;

namespace Employment_System.Dtos
{
    public class MeetingDto
    {
        public int MeetingId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string VirtualMeeting { get; set; }
        public ICollection<int> MeetingParticipants { get; set; }


    }
}
