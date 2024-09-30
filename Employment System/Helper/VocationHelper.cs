using Employment_System.Domain.Entities;

namespace Employment_System.Helper
{
    public class VocationHelper
    {
        public static VocationRequestStatus MapToVocationRequestStatus(VocationStatus status)
        {
            return status switch
            {
                VocationStatus.Pending => new VocationRequestStatus { StatusId = 1, StatusName = "Pending" },
                VocationStatus.Approved => new VocationRequestStatus { StatusId = 2, StatusName = "Approved" },
                VocationStatus.Rejected => new VocationRequestStatus { StatusId = 3, StatusName = "Rejected" },
                VocationStatus.Escalated => new VocationRequestStatus { StatusId = 4, StatusName = "Escalated" },
                VocationStatus.Cancelled => new VocationRequestStatus { StatusId = 5, StatusName = "Cancelled" },
                VocationStatus.Completed => new VocationRequestStatus { StatusId = 6, StatusName = "Completed" },
                _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
            };

        }
    }
}
