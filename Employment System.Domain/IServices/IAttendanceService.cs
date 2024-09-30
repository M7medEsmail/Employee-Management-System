using Employment_System.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employment_System.Domain.IServices
{
    public interface IAttendanceService
    {

        Task<bool> CheckIn(int employeeId );
        Task<bool> CheckOut(int employeeId);
        Task<List<AttendanceRecord>> GetAllAttendance();
        Task<List<AttendanceRecord>>  GetAllAttendanceForUser(int UserId);


    }   
}
