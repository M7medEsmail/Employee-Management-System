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
    public class EscalationService : IEscalationService
    {
        private readonly EmpManageDbContext _context;
        private readonly IEmailService _emailService;

        public EscalationService(EmpManageDbContext context , IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }
        public async Task EscalatePendingRequestsAsync()
        {
            var pendingRequests = await _context.VocationRequests
            .Where(v => v.StatusId ==1 && DateTime.Now > v.CreatedAt.AddDays(2))
            .ToListAsync();

            foreach (var request in pendingRequests)
            {

               await NotifyManager(request); // Notify the manager about this request
                request.StatusId =4 ;


            }

            _context.SaveChangesAsync();
        }

        private async Task NotifyManager(VocationRequest request)
        {
            // Implementation of notifying the manager
            var managerEmail = "m7med.esmail22@gmail.com"; 
            var subject = $"Pending Vacation Request from Employee ID {request.EmployeeId}";
            var message = $"<p>Dear Manager,</p><p>The vacation request from Employee ID {request.EmployeeId}" +
                $" has been pending for more than 2 days.</p><p>Request Details:</p><p>Start Date: {request.StartDateOfVocation}" +
                $"</p><p>End Date: {request.EndDateOfVocation}</p><p>Reason: {request.Reason}" +
                $"</p><p>Please take the necessary action.</p><p>Regards,<br>Your Company</p>";

            await _emailService.SendEmailAsync(managerEmail, subject, message);
        }
    }
}
