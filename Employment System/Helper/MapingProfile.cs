using AutoMapper;
using Employment_System.Domain.Entities;
using Employment_System.Dtos;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Employment_System.Helper
{
    public class MapingProfile : Profile
    {
        public MapingProfile()
        {
            CreateMap<VocationRequest, VocationDto>()
           .ForMember(d => d.Employee,
                      opt => opt.MapFrom(s => s.Employee.AppUser.Name)) // Assuming 0 is a default or placeholder value
           .ForMember(d => d.ManagerOfRequest,
                      opt => opt.MapFrom(s => s.Employee.ManagerId))// Same assumption
           .ForMember(d => d.StatusName,
                      opt => opt.MapFrom(s => s.Status.StatusName )); // Assuming an empty string if null

            CreateMap<VocationDto, VocationRequest>()
            .ForMember(d => d.Status, opt => opt.MapFrom(s => VocationHelper.MapToVocationRequestStatus((VocationStatus)Enum.Parse(typeof(VocationStatus), s.StatusName))));



            //CreateMap<VocationRequest, VocationDto>().ReverseMap();

            CreateMap<LoginDto, AppUser>().ReverseMap();

            CreateMap<MeetingDto, Meeting>()
                .ForMember(dest => dest.MeetingParticipants, opt =>
                    opt.MapFrom(src => src.MeetingParticipants.Select(id => new MeetingParticipant
                    {
                        EmployeeId = id // Assuming EmployeeId is being mapped from ParticipatingMeetingIds
                    }).ToList()));

            CreateMap<MeetingParticipant, int>() // Maps from ParticipantMeeting to EmployeeId (int)
                .ConvertUsing(pm => pm.EmployeeId); // Custom conversion for lists
        }
    }
 }

