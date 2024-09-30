using Employment_System.Domain.Entities;
using Employment_System.Domain.IServices;
using Employment_System.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employment_System.Services.Services
{
    public class MeetingService : IMeetingService
    {
        private readonly EmpManageDbContext _context;

        public MeetingService(EmpManageDbContext context)
        {
            _context = context;
        }
        // Create a new meeting
        public async Task CreateMeetingAsync(Meeting meeting)
        {
            // Check participants' availability before scheduling
            if (await CheckAvailabilityAsync(meeting))
            {
                await _context.Meetings.AddAsync(meeting);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("Some participants are not available during this time.");
            }
        }

        // Check if all participants are available
        private async Task<bool> CheckAvailabilityAsync(Meeting meeting)
        {
            //var overlappingMeetings = await _context.Meetings
            //    .Where(m => m.MeetingParticipants.Any(mp => meeting.MeetingParticipants
            //    .Any(p => p.EmployeeId == mp.EmployeeId)) &&
            //    (meeting.StartDate < m.EndDate && meeting.EndDate > m.StartDate))
            //    .ToListAsync();

            var overlappingMeetings = await _context.Meetings
       .Include(m => m.MeetingParticipants) // Ensure the navigation property is included
       .Where(m => m.MeetingParticipants.Any(mp =>
           meeting.MeetingParticipants.Any(p => p.EmployeeId == mp.EmployeeId) &&
           (meeting.StartDate < m.EndDate && meeting.EndDate > m.StartDate)))
       .ToListAsync(); // Ensure that we are pulling data to evaluate


            // Return true if no overlapping meetings are found
            return !overlappingMeetings.Any();
        }

        // Get a meeting by its ID
        public async Task<Meeting> GetMeetingByIdAsync(int id)
        {
            var meeting = await _context.Meetings
                .Include(m => m.MeetingParticipants)
                .FirstOrDefaultAsync(m => m.MeetingId == id);

            if (meeting == null)
            {
                throw new KeyNotFoundException("Meeting not found.");
            }
            return meeting;
        }

        // Get all meetings
        public async Task<IEnumerable<Meeting>> GetAllMeetingsAsync()
        {
            return await _context.Meetings
                .Include(m => m.MeetingParticipants)
                .ToListAsync();
        }

        // Check if a user (employee) has available time slots
        public async Task<bool> CheckTimeSlotForUser(int employeeId, DateTime appointmentDate, TimeSpan startTime, TimeSpan endTime)
        {
            var conflictingSchedule = await _context.Schedules
                .Where(s => s.EmployeeId == employeeId && s.AppointmentDate == appointmentDate &&
                            (s.StartTime < endTime && s.EndTime > startTime))
                .FirstOrDefaultAsync();

            // Return true if there is no conflicting schedule
            return conflictingSchedule == null;
        }

  
    }
}
