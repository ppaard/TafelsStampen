namespace TafelsStampen.Console.Schermen;
using Spectre.Console;
using TafelsStampen.Application.Mediator;
using TafelsStampen.Application.Queries.GetPrestatieSamenvatting;
using TafelsStampen.Console.Navigatie;
using TafelsStampen.Console.Stijl;
using TafelsStampen.Domain.ValueObjects;

public class PrestatieschermScherm : IScherm
{
    private readonly IMediator _mediator;
    private readonly NavigatieService _navigatie;
    private readonly HallOfFameScherm _hallOfFameScherm;

    public Guid SessionId { get; set; }
    public Guid PlayerId { get; set; }
    public string SpelerNaam { get; set; } = string.Empty;
    public int TafelNummer { get; set; }
    public GameMode Modus { get; set; }

    private const string BekijkHallOfFame = "🏆 Bekijk mijn positie in de Hall of Fame";
    private const string TerugNaarMenu = "↩️  Terug naar menu";

    public PrestatieschermScherm(IMediator mediator, NavigatieService navigatie, HallOfFameScherm hallOfFameScherm)
    {
        _mediator = mediator;
        _navigatie = navigatie;
        _hallOfFameScherm = hallOfFameScherm;
    }

    public async Task ToonAsync()
    {
        var samenvatting = await _mediator.QueryAsync(new GetPrestatieSamenvattingQuery(SessionId, PlayerId, TafelNummer, Modus));

        AsciiArt.Toon();
        Thema.SchrijfSectieHeader("Prestaties");

        // 1. Primaire prestatie
        AnsiConsole.WriteLine();
        if (samenvatting.IsEersteGame)
        {
            AnsiConsole.MarkupLine($"[yellow bold]⭐ Eerste score gezet voor Tafel {TafelNummer}![/]");
        }
        else if (samenvatting.IsNieuwPersoonlijkRecord)
        {
            var vorige = samenvatting.VorigeBesteMs!.Value / 1000.0;
            var huidig = samenvatting.HuidigeTijdMs / 1000.0;
            AnsiConsole.MarkupLine($"[green bold]🏆 NIEUW PERSOONLIJK RECORD! {huidig:F1}s  (was {vorige:F1}s)[/]");
        }
        else if (samenvatting.VorigeBesteMs.HasValue && samenvatting.HuidigeTijdMs < samenvatting.VorigeBesteMs.Value)
        {
            var vorige = samenvatting.VorigeBesteMs.Value / 1000.0;
            var huidig = samenvatting.HuidigeTijdMs / 1000.0;
            AnsiConsole.MarkupLine($"[cyan1]⬆️  Beter dan vorige keer!  {vorige:F1}s → {huidig:F1}s[/]");
        }
        else if (samenvatting.VorigeBesteMs.HasValue)
        {
            var record = samenvatting.VorigeBesteMs.Value / 1000.0;
            var huidig = samenvatting.HuidigeTijdMs / 1000.0;
            var verschil = huidig - record;
            AnsiConsole.MarkupLine($"[white]Persoonlijk record staat nog op {record:F1}s  (nog {verschil:F1}s te winnen)[/]");
        }

        // 2. Hall of Fame positie
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine($"[gold1]📊 Jouw positie: [bold]#{samenvatting.HallOfFameRang}[/] van {samenvatting.AantalDeelnemers}[/]");

        // 3. Verbeterde sommen
        if (samenvatting.VerbeterdeSommen.Count > 0)
        {
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[cyan1 bold]Verbeterde sommen:[/]");

            var table = new Table()
                .Border(TableBorder.Rounded)
                .BorderColor(Color.CornflowerBlue)
                .AddColumn(new TableColumn("[cyan1]Som[/]").Centered())
                .AddColumn(new TableColumn("[cyan1]Vorige[/]").Centered())
                .AddColumn(new TableColumn("[cyan1]Nu[/]").Centered())
                .AddColumn(new TableColumn("[cyan1]Winst[/]").Centered());

            foreach (var som in samenvatting.VerbeterdeSommen)
            {
                var winst = som.VorigeBesteMs - som.HuidigeMs;
                table.AddRow(
                    $"{som.Multiplicand} × {som.Multiplier}",
                    $"{som.VorigeBesteMs} ms",
                    $"[green]{som.HuidigeMs} ms[/]",
                    $"[green]-{winst} ms[/]");
            }

            AnsiConsole.Write(table);
        }

        // 4. Aanmoediging (alleen als niets verbeterd en niet eerste game)
        if (!samenvatting.IsEersteGame && !samenvatting.IsNieuwPersoonlijkRecord
            && samenvatting.VerbeterdeSommen.Count == 0
            && !(samenvatting.VorigeBesteMs.HasValue && samenvatting.HuidigeTijdMs < samenvatting.VorigeBesteMs.Value))
        {
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[yellow]💪 Blijf oefenen![/]");
        }

        // 5. Navigatie
        AnsiConsole.WriteLine();
        var keuze = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Wat wil je doen?[/]")
                .AddChoices(BekijkHallOfFame, TerugNaarMenu));

        if (keuze == BekijkHallOfFame)
        {
            _hallOfFameScherm.InitieleTafelFilter = TafelNummer;
            _hallOfFameScherm.InitieleModusFilter = Modus;
            _hallOfFameScherm.InitieleSpelerFilterId = PlayerId;
            _hallOfFameScherm.InitieleSpelerFilterNaam = SpelerNaam;
            await _navigatie.NaarAsync(_hallOfFameScherm);
        }
    }
}
