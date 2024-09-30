using Employment_System.Domain.Entities;
using Employment_System.Domain.IServices;
using Employment_System.Repository.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;


namespace Employment_System.Services.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly EmpManageDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AttendanceService(EmpManageDbContext context , IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> CheckIn(int employeeId)
        {

            // Check if user has already checked in today
            var existingCheckIn = await _context.AttendanceRecords
                .FirstOrDefaultAsync(a => a.EmployeeId == employeeId && a.ChickIn.Date == DateTime.Now.Date);

            // If the user hasn't checked in, add a new attendance record
            var attendance = new AttendanceRecord
            {
                EmployeeId = employeeId, // Use the employeeId from the token
                ChickIn = DateTime.Now
            };

            _context.AttendanceRecords.Add(attendance);
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<bool> CheckOut(int employeeId)
        {

            // Check if the user has already checked out today
            var existingCheckOut = await _context.AttendanceRecords
                .FirstOrDefaultAsync(a => a.EmployeeId == employeeId && a.ChickOut.HasValue && a.ChickOut.Value.Date == DateTime.Now.Date);


            // If no checkout record is found
            if (existingCheckOut == null)
            {
                // Optionally, you may want to check if the user has checked in before checking out
                var existingCheckIn =await CheckIn(employeeId);
                if (!existingCheckIn)
                {
                    // User has not checked in yet
                    return false;
                }

                // If user has checked in but not checked out yet, create a new checkout record
                existingCheckOut = new AttendanceRecord
                {
                    EmployeeId = employeeId, // Ass
                    ChickOut = DateTime.Now // Set the current time for checkout
                };
                await _context.AttendanceRecords.AddAsync(existingCheckOut);
            }
            else
            {
                // If the user has checked out before, return false
                if (existingCheckOut.ChickOut.HasValue)
                {
                    // User already checked out today
                    return false;
                }

                // Update the checkout time
                existingCheckOut.ChickOut = DateTime.Now;
                _context.AttendanceRecords.Update(existingCheckOut);
            }

            await _context.SaveChangesAsync(); // Save changes asynchronously
            return true;
        }

        public  Task<List<AttendanceRecord>> GetAllAttendance()
        {
            var Atttendances =  _context.AttendanceRecords.ToListAsync();
            return Atttendances;
        }


        public  Task<List<AttendanceRecord>> GetAllAttendanceForUser(int userId)
        {
            var userAttendances =  _context.AttendanceRecords
                                                 .Where(e => e.EmployeeId == userId)
                                                 .ToListAsync();
            return userAttendances; // Returns an empty list if no records found
        }
    }
}
