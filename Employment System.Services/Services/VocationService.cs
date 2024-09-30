using Employment_System.Domain.Entities;
using Employment_System.Domain.IRepositories;
using Employment_System.Domain.IServices;
using Employment_System.Domain.ISpecification;
using Employment_System.Repository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TaskEF.Specifications;

namespace Employment_System.Services.Services
{
    public class VocationService : IVocationService
    {


        private readonly IGenericRepository<VocationRequest> VocationRepo;
        private readonly IGenericRepository<VocationRequestStatus> StatusRepo;
        private readonly EmpManageDbContext _empManageContext;
        private readonly ILogger<VocationRequest> _logger;
        private readonly EmpManageDbContext _context;

        public VocationService(IGenericRepository<VocationRequest> genericRepository ,EmpManageDbContext empManageContext ,ILogger<VocationRequest> logger , EmpManageDbContext context)
        {
            VocationRepo = genericRepository;
            _empManageContext = empManageContext;
            _logger = logger;
            _context = context;
        }

    
        public Task<VocationRequest> CreateAsync(VocationRequest entity)
        {
            var Vocation = VocationRepo.CreateAsync(entity);
            return Vocation;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var result = await VocationRepo.DeleteAsync(id);
            return result; // Return the result of the deletion
        }

        public async Task<string> EsclationVocationRequestsAsync()
        {
            //// Await the asynchronous operation to retrieve the vocation requests
            //var requests = await _empManageContext.VocationRequests
            //                         .Where(r => r.StatusId == 1 && r.CreatedAt <= DateTime.Now.AddDays(-2))
            //                         .ToArrayAsync();

            //foreach (var request in requests)
            //{
            //    // Await the asynchronous operation to retrieve the new manager
            //    var newManager = await _empManageContext.Users.FirstOrDefaultAsync(u => u.Id == request.ManagerOfRequest);

            //    if (newManager == null)
            //    {
            //        _logger.LogError($"Manager not found for request {request.}");
            //        continue; // Skip this iteration if the manager is not found
            //    }

            //    // Update the manager ID and mark the entity as modified
            //    request.ManagerOfRequest = newManager.Id ?? "1";
            //    _empManageContext.Entry(request).State = EntityState.Modified;
            //}

            //// Save the changes asynchronously
            await _empManageContext.SaveChangesAsync();

            return "Escalation successful";
        }


        public async Task<IReadOnlyList<VocationRequest>> GetAllAsync()
        {
            return await VocationRepo.GetAllAsync();
        }

        public Task<IReadOnlyList<VocationRequest>> GetAllWithSpecAsync(ISpecification<VocationRequest> spec)
        {
            return VocationRepo.GetAllWithSpecAsync(spec);
           
        }

        public Task<VocationRequest> GetByIdAsync(int id)
        {
            var Vocation = VocationRepo.GetByIdAsync(id);
            return Vocation;
        }

        public Task<VocationRequest> GetByIdWithSpecAsync(ISpecification<VocationRequest> spec)
        {
            throw new NotImplementedException();
        }

        public Task<List<VocationRequestStatus>> GetVocationRequestTypes()
        {
            var Vacations = _context.VocationRequestStatuses.ToListAsync();
            return Vacations;
        }

        public bool UpdateAsync(int id ,VocationRequest entity)
        {
            VocationRepo.UpdateAsync(id,entity);
            return true;
        }

    }
}
