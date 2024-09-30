using Employment_System.Domain.Entities;
using Employment_System.Domain.IRepositories;
using Employment_System.Domain.ISpecification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employment_System.Domain.IServices
{
    public interface IVocationService :IGenericRepository<VocationRequest> 
    {
        Task<List<VocationRequestStatus>> GetVocationRequestTypes();


        Task<string> EsclationVocationRequestsAsync();


    }
}
