using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MyBigBadBlog.Data.Postgres;
using MyBigBadBlog.Service.DatabaseMigration;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();

////builder.AddPostgresFeatures();

builder.Services.AddDbContext<ApplicationDbContext>(
    options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("MyBigBadBlog"));
        options.ConfigureWarnings(warnings => warnings.Log(RelationalEventId.PendingModelChangesWarning));
    });

builder.Services.AddOpenTelemetry()
                .WithTracing(tracing => tracing.AddSource(Worker.ActivityName));

var host = builder.Build();
await host.RunAsync();
