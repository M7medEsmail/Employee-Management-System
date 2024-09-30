using Employment_System.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employment_System.Domain.ISpecification
{
    public class VocationWithEmployeeSpecification : BaseSpecification<VocationRequest>
    {
        public VocationWithEmployeeSpecification(Vocationparam vocation) :
            base(P =>
            (!vocation.EmpId.HasValue || P.EmployeeId == vocation.EmpId))
        {

            Includes.Add(p => p.Employee);
        }

        public VocationWithEmployeeSpecification(int id) : base(p => p.EmployeeId == id)
        {
            
                Includes.Add(p => p.Employee);
            
        }
        public VocationWithEmployeeSpecification()
        {
            Includes.Add(p => p.Employee);
            // Include the Employee navigation property correctly

        }

    }
}
