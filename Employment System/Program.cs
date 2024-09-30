using Employment_System.Domain.IRepositories;
using Employment_System.Domain.IServices;
using Employment_System.Extensions;
using Employment_System.Helper;
using Employment_System.Repository.Data;
using Employment_System.Repository.Implementations;
using Employment_System.Services.Services;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    options.JsonSerializerOptions.MaxDepth = 64; // Optional: increase max depth if needed
}); ;

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Hangfire and add services
builder.Services.AddHangfire(configuration =>
    configuration.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHangfireServer();

builder.Services.AddDbContext<EmpManageDbContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHttpContextAccessor(); // Add this line to register IHttpContextAccessor


var configration = builder.Configuration;
builder.Services.AddIdentityService(configration);
builder.Services.AddAppService();
var app = builder.Build();

//// Schedule the escalation job to run every hour
//RecurringJob.AddOrUpdate<EscalationService>(
//    "escalate-pending-requests",
//    service => service.EscalatePendingRequestsAsync(),
//    Cron.Hourly); // Change the frequency as needed


// Schedule the escalation job
using (var scope = app.Services.CreateScope())
{
    var recurringJobManager = app.Services.GetRequiredService<IRecurringJobManager>();

    recurringJobManager.AddOrUpdate<EscalationService>(
        "escalate-pending-requests",
        service => service.EscalatePendingRequestsAsync(),
        Cron.Hourly); // Change the frequency as needed
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseHangfireDashboard("/dashboard");
app.UseHangfireServer();
app.MapControllers();

app.Run();
