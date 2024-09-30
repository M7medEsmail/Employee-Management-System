using System.ComponentModel.DataAnnotations;

namespace Employment_System.Domain.Entities
{
    public class VocationRequestStatus
    {
        [Key]
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public ICollection<VocationRequest> VocationsRequest { get; set; }
    }
}
