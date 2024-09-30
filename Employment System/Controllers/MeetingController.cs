using AutoMapper;
using Employment_System.Domain.Entities;
using Employment_System.Domain.IServices;
using Employment_System.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Employment_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingController : ControllerBase
    {
        private readonly IMeetingService _meetingService;
        private readonly IMapper _mapper;

        public MeetingController(IMeetingService meetingService , IMapper mapper)
        {
            _meetingService = meetingService;
            _mapper = mapper;
        }

        [HttpPost("Create Meeting")]
        public async Task<IActionResult> CreateMeeting([FromBody] MeetingDto meeting)
        {
            try
            {
                var meet = _mapper.Map<Meeting>(meeting);
                await _meetingService.CreateMeetingAsync(meet);
                return CreatedAtAction(nameof(CreateMeeting), new { id = meeting.MeetingId }, meeting);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllMeetings")] // Changed route for clarity
        public async Task<IActionResult> GetMeetings()
        {
            var meetings = await _meetingService.GetAllMeetingsAsync();
            // Map the meetings to MeetingDto and return the result
            var meetingDtos = _mapper.Map<IEnumerable<MeetingDto>>(meetings);

            return Ok(meetingDtos);
        }

        // GET: api/meeting/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMeetingById(int id)
        {
            try
            {
                var meeting = await _meetingService.GetMeetingByIdAsync(id);
                var meetingDto = _mapper.Map<MeetingDto>(meeting);
                return Ok(meetingDto);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Meeting not found.");
            }
        }

    }
}
