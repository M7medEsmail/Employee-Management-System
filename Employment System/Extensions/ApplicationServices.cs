using Employment_System.Domain.IRepositories;
using Employment_System.Domain.IServices;
using Employment_System.Helper;
using Employment_System.Repository.Implementations;
using Employment_System.Services.Services;

namespace Employment_System.Extensions
{
    public static class ApplicationServices
    {
        public static IServiceCollection AddAppService(this IServiceCollection services)
        {

            services.AddScoped<IAttendanceService, AttendanceService>();
            services.AddScoped<IEscalationService, EscalationService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IVocationService, VocationService>();
            services.AddScoped<IMeetingService, MeetingService>();
            //builder.Services.AddAutoMapper(typeof(MapingProfile)); // Add AutoMapper
            services.AddAutoMapper(typeof(MapingProfile));

            return services;

        }
    }
}
