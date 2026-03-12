using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using TafelsStampen.Application;
using TafelsStampen.Console.Navigatie;
using TafelsStampen.Console.Schermen;
using TafelsStampen.Infrastructure;

var appDir = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
    "TafelsStampen");

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.File(
        path: Path.Combine(appDir, "logs", "tafels-stampen-.log"),
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {SourceContext}: {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

try
{
    Log.Information("Applicatie gestart");

    var host = Host.CreateDefaultBuilder(args)
        .UseSerilog()
        .ConfigureServices((_, services) =>
        {
            var dataDir = Path.Combine(appDir, "data");

            services.AddApplication();
            services.AddInfrastructure(dataDir);

            services.AddSingleton<NavigatieService>();
            services.AddTransient<ResultatenScherm>();
            services.AddTransient<SpelScherm>();
services.AddTransient<TafelKeuzeScherm>();
            services.AddTransient<SpelerSelectieScherm>();
            services.AddTransient<HallOfFameScherm>();
            services.AddTransient<InstellingenScherm>();
            services.AddTransient<HoofdmenuScherm>();
        })
        .Build();

    using var scope = host.Services.CreateScope();
    var scopedServices = scope.ServiceProvider;

    var navigatie = scopedServices.GetRequiredService<NavigatieService>();
    var hoofdmenu = scopedServices.GetRequiredService<HoofdmenuScherm>();
    await navigatie.NaarAsync(hoofdmenu);

    Log.Information("Applicatie afgesloten");
}
catch (Exception ex)
{
    Log.Fatal(ex, "Applicatie onverwacht gestopt");

    Spectre.Console.AnsiConsole.Clear();
    Spectre.Console.AnsiConsole.MarkupLine("[red bold]Er is een onverwachte fout opgetreden:[/]");
    Spectre.Console.AnsiConsole.MarkupLine($"[red]{Spectre.Console.Markup.Escape(ex.Message)}[/]");
    TafelsStampen.Console.Stijl.Thema.WachtOpEnter("Druk op Enter om af te sluiten...");
}
finally
{
    Log.CloseAndFlush();
}
