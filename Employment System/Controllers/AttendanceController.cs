using Employment_System.Domain.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Employment_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }
        [Authorize]
        [HttpPost("CheckIn")]
        public async Task<IActionResult> CheckIn()
        {
            foreach (var claim in User.Claims)
            {
                Console.WriteLine($"{claim.Type}: {claim.Value}");
            }

            var EmpId = User.FindFirstValue("EmployeeId");

            if (string.IsNullOrEmpty(EmpId))
            {
                return Unauthorized("Employee ID not found in token.");

            }

            // Proceed with the business logic, for example, calling your attendance service
             var result = await _attendanceService.CheckIn(int.Parse(EmpId));

            return Ok($"Check-in successful for Employee ID: {EmpId}");

        }
        [Authorize]

        [HttpPost("CheckOut")]
        public async Task<IActionResult> CheckOut()
        {
            //Get the User ID from the JWT token claims
           var EmpId = User.FindFirstValue("EmployeeId");
            if (string.IsNullOrEmpty(EmpId))
            {
                return Unauthorized("Employee ID not found in token.");
            }

           // Proceed with the business logic, for example, calling your attendance service

           var result = await _attendanceService.CheckOut(int.Parse(EmpId));

            return Ok($"Check-Out successful for Employee ID: {EmpId}");

        }

        //[Authorize]
        [HttpGet("All Attendance")]
        public async Task<IActionResult> AllAttendance()
        {
            var result = await _attendanceService.GetAllAttendance();

            return Ok(result);

        }

        [Authorize]
        [HttpGet("All Attendance For User")]
        public async Task<IActionResult> AllAttendanceForUser()
        {
            // Get the User ID from the JWT token claims
            var EmpId = User.FindFirstValue("EmployeeId");
            if (string.IsNullOrEmpty(EmpId))
            {
                return Unauthorized("Employee ID not found in token.");
            }

            // Proceed with the business logic, for example, calling your attendance service
            var result = await _attendanceService.GetAllAttendanceForUser(int.Parse(EmpId));

            return Ok(result);

        }
    }
}
