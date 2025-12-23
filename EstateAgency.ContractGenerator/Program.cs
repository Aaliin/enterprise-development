using EstateAgency.ContractGenerator.Models;
using EstateAgency.ContractGenerator.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

Console.Title = "Estate Agency Contract Generator";
Console.WriteLine("Starting Estate Agency Contract Generator...");

try
{
    var builder = Host.CreateApplicationBuilder(args);

    builder.Configuration
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables()
        .AddCommandLine(args);

    builder.Services.Configure<HostOptions>(options =>
    {
        options.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
        options.ServicesStartConcurrently = true;
    });

    builder.Services.Configure<NatsOptions>(builder.Configuration.GetSection("Nats"));
    builder.Services.Configure<GeneratorOptions>(builder.Configuration.GetSection("Generator"));

    builder.Services.AddSingleton<INatsPublisher, NatsPublisher>();
    builder.Services.AddSingleton<IRequestGenerator, RequestGenerator>();
    builder.Services.AddHostedService<ContractGenerationService>();

    builder.Logging.ClearProviders();
    builder.Logging.AddConsole();
    builder.Logging.AddDebug();
    builder.Logging.SetMinimumLevel(LogLevel.Information);

    var host = builder.Build();

    var logger = host.Services.GetRequiredService<ILogger<Program>>();
    var natsUrl = builder.Configuration["Nats:Url"] ?? "nats://localhost:4222";

    await host.RunAsync();
}
catch (Exception ex)
{
    Console.WriteLine($"Fatal error: {ex.Message}");
    Environment.Exit(1);
}