using Serilog;

namespace CulDeSacServicesOrchestration.Core.Services.Logging;

public class LoggerService : ILoggerService
{
    private readonly ILogger logger;

    public LoggerService(ILogger logger)
    {
        this.logger = logger;
    }

    public void LogInformation(string message)
    {
        logger.Information(message);
    }
}