using Employment_System.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employment_System.Domain.IServices
{
    public interface IMeetingService
    {
        Task CreateMeetingAsync(Meeting meeting);
        Task<bool> CheckTimeSlotForUser(int employeeId, DateTime appointmentDate, TimeSpan startTime, TimeSpan endTime);
        Task<IEnumerable<Meeting>> GetAllMeetingsAsync();
        Task<Meeting> GetMeetingByIdAsync(int id);
    }
}
