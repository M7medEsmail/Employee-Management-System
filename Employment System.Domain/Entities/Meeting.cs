namespace Employment_System.Domain.Entities
{
    public class Meeting
    {
        public int MeetingId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string VirtualMeeting { get; set; }
        public ICollection<MeetingParticipant> MeetingParticipants { get; set; }


    }
}
