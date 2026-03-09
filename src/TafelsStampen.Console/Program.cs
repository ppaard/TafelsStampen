using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TafelsStampen.Application;
using TafelsStampen.Console.Navigatie;
using TafelsStampen.Console.Schermen;
using TafelsStampen.Infrastructure;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var dataDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "TafelsStampen",
            "data");

        services.AddApplication();
        services.AddInfrastructure(dataDir);

        services.AddSingleton<NavigatieService>();

        // Register all screens
        services.AddTransient<ResultatenScherm>();
        services.AddTransient<SpelScherm>();
        services.AddTransient<ModusKeuzeScherm>();
        services.AddTransient<TafelKeuzeScherm>();
        services.AddTransient<SpelerSelectieScherm>();
        services.AddTransient<HallOfFameScherm>();
        services.AddTransient<InstellingenScherm>();
        services.AddTransient<HoofdmenuScherm>();
    })
    .Build();

using var scope = host.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var navigatie = services.GetRequiredService<NavigatieService>();
    var hoofdmenu = services.GetRequiredService<HoofdmenuScherm>();
    await navigatie.NaarAsync(hoofdmenu);
}
catch (Exception ex)
{
    Spectre.Console.AnsiConsole.Clear();
    Spectre.Console.AnsiConsole.MarkupLine("[red bold]Er is een onverwachte fout opgetreden:[/]");
    Spectre.Console.AnsiConsole.MarkupLine($"[red]{Spectre.Console.Markup.Escape(ex.Message)}[/]");
    TafelsStampen.Console.Stijl.Thema.WachtOpEnter("Druk op Enter om af te sluiten...");
}
