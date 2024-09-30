using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employment_System.Domain.IServices
{
    public interface IEscalationService
    {
        Task EscalatePendingRequestsAsync();

    }
}
