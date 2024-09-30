using Employment_System.Domain.Entities;

namespace Employment_System.Dtos
{
    public class VocationDto
    {
        public int VocatiionRequestId { get; set; }
        public DateTime StartDateOfVocation { get; set; }
        public DateTime EndDateOfVocation { get; set; }
        public string Reason { get; set; }
        public DateTime ModifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime EscalationAt { get; set; }
        public int ManagerOfRequest { get; set; }
        public int Escalation { get; set; }
        public bool IsDeleted { get; set; }
        public int EmployeeId { get; set; }
        public string Employee { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }

    }
}
