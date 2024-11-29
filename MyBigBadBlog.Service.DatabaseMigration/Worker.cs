using Microsoft.EntityFrameworkCore;
using MyBigBadBlog.Data.Postgres;
using OpenTelemetry.Trace;
using System.Diagnostics;

namespace MyBigBadBlog.Service.DatabaseMigration;

public class Worker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly ILogger<Worker> _logger;

    internal const string ActivityName = "MigrationService";
    private static readonly ActivitySource _activitySource = new(ActivityName);

    public Worker(
        IServiceProvider serviceProvider,
        IHostApplicationLifetime hostApplicationLifetime,
        ILogger<Worker> logger)
    {
        _serviceProvider = serviceProvider;
        _hostApplicationLifetime = hostApplicationLifetime;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker running at: {Time}", DateTimeOffset.Now);

        using var activity = _activitySource.StartActivity("Migrating database", ActivityKind.Client);

        try
        {
            using var scope = _serviceProvider.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await dbContext.Database.MigrateAsync(stoppingToken);
        }
        catch (Exception ex)
        {
            activity?.RecordException(ex);

            _logger.LogError(ex, "Error with message {ErrorMessage}", ex.Message);

            throw;
        }

        _hostApplicationLifetime.StopApplication();
    }
}
