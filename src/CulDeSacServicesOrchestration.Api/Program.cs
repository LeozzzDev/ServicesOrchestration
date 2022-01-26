using CulDeSacServicesOrchestration.Core.BrokersInterfaces;
using CulDeSacServicesOrchestration.Core.Orchestrations.StudentsEvents;
using CulDeSacServicesOrchestration.Core.Services.Logging;
using CulDeSacServicesOrchestration.Core.Services.Students;
using CulDeSacServicesOrchestration.Core.Services.StudentsEvents;
using CulDeSacServicesOrchestration.Infrastructure.Database;
using CulDeSacServicesOrchestration.Infrastructure.Queue;
using Microsoft.Azure.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var connectionString =
    builder.Configuration["DatabaseConnectionString"];

builder.Services.AddSingleton<IQueueClient>(sp => new QueueClient(
    builder.Configuration["AzureServiceBus:ConnectionString"],
    builder.Configuration["AzureServiceBus:QueueName"]));

builder.Services.AddDbContext<DatabaseStudentsBroker>(options =>
    options.UseSqlServer(connectionString, assembly =>
        assembly.MigrationsAssembly(typeof(DatabaseStudentsBroker).Assembly.FullName)));

builder.Services.AddTransient<IDatabaseStudentsBroker, DatabaseStudentsBroker>();
builder.Services.AddTransient<IStudentsService, StudentsService>();
builder.Services.AddTransient<IStudentsEventsService, StudentsEventsService>();
builder.Services.AddTransient<IStudentsEventsOrchestrationService, StudentsEventsOrchestrationService>();
builder.Services.AddTransient<IQueueBroker, QueueStudentsBroker>();

builder.Host.UseSerilog((hostBuilder, loggerConfiguration) =>
    loggerConfiguration.WriteTo.Console().MinimumLevel.Information());
    
builder.Services.AddSingleton<ILoggerService, LoggerService>();

builder.Services.AddControllers();
builder.Services.AddHealthChecks();

var app = builder.Build();
using var scope = app.Services.CreateScope();
scope.ServiceProvider.GetRequiredService<IStudentsEventsOrchestrationService>().ListenToStudentsEvents();
app.MapControllers();
app.MapHealthChecks("/status");
await app.RunAsync();

public partial class Program { }