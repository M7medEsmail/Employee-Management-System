using AutoMapper;
using Employment_System.Domain.Entities;
using Employment_System.Domain.IRepositories;
using Employment_System.Domain.IServices;
using Employment_System.Domain.ISpecification;
using Employment_System.Dtos;
using Employment_System.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Employment_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VocationController : ControllerBase
    {
        private readonly IVocationService _vocationService;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<VocationRequest> _genericRepository;

        public VocationController(IVocationService vocationService , IMapper mapper ,IGenericRepository<VocationRequest> genericRepository)
        {
            _vocationService = vocationService;
            _mapper = mapper;
            _genericRepository = genericRepository;
        }

        [HttpGet("Get Vocations")]
        public async Task<IActionResult> GetVocations() 
        {
            var vocations = await _vocationService.GetAllAsync();
            return Ok(vocations);
        }

        [HttpGet("Get Vocations Specification")]
        public async Task<ActionResult<VocationDto>> GetVocationSpecification([FromQuery] Vocationparam empParams)
        {
            var spec = new VocationWithEmployeeSpecification(empParams);
            var employees = await _genericRepository.GetAllWithSpecAsync(spec);
            return Ok(_mapper.Map<IReadOnlyList<VocationRequest>, IReadOnlyList<VocationDto>>(employees));
        }

        [HttpPost("Request Vocation")]
        public async Task<IActionResult> RequestVocation(VocationDto vocationDto)
        {
            if (vocationDto == null)
            {
                return BadRequest("Vocation is empty"); // Use double quotes for strings
            }
            var vocationRequest = _mapper.Map<VocationDto,VocationRequest>(vocationDto); // No need to specify the source type, as Dapper will infer it.

            vocationRequest.Status = VocationHelper.MapToVocationRequestStatus(VocationStatus.Pending);
            return Ok(await _vocationService.CreateAsync(vocationRequest));
        }

        [HttpPut("Cancel Vocation")]
        public async Task<IActionResult> CancellVocation(int Vocationid , [FromBody]VocationDto vocationDto)
        {
            var vocation = await _vocationService.GetByIdAsync(Vocationid);
            if (vocation == null)
            {
                return NotFound($"Vocation request with id {Vocationid} not found."); ;
            }
            vocation.Status = VocationHelper.MapToVocationRequestStatus(VocationStatus.Cancelled);

            var result =  _vocationService.UpdateAsync(Vocationid,vocation);

              return Ok("Cancel Vocation successfully");
        }

        // Only Manager Can use this
        [HttpPut("Reject Vocation")]
        public async Task<IActionResult> RejectVocation(int Vocationid)
        {

            var vocation = await _vocationService.GetByIdAsync(Vocationid);
            if(vocation == null)
            {
                return NotFound($"Vocation request with id {Vocationid} not found."); ;
            }
            vocation.Status = VocationHelper.MapToVocationRequestStatus(VocationStatus.Rejected);

             _vocationService.UpdateAsync(Vocationid , vocation);

            return Ok("Vocation Rejected");
        }

        // Only Manager Can use this
        [HttpPut("Approve Vocation")]
        public async Task<IActionResult> ApproveVocation(int id)
        {
            var vocation = await _vocationService.GetByIdAsync(id);
            if(vocation == null)
            {
                return NotFound($"Vocation request with id {id} not found.");
            };
            vocation.Status = VocationHelper.MapToVocationRequestStatus(VocationStatus.Approved);

            var result = _vocationService.UpdateAsync(id, vocation);

            return Ok(" Approve Vocation Successfully");
        }


    }
}
