namespace TafelsStampen.Console.Schermen;
using Spectre.Console;
using TafelsStampen.Application.Mediator;
using TafelsStampen.Application.Queries.GetHallOfFameByTable;
using TafelsStampen.Application.Queries.GetHallOfFameOverall;
using TafelsStampen.Application.Queries.GetPlayers;
using TafelsStampen.Console.Navigatie;
using TafelsStampen.Console.Stijl;
using TafelsStampen.Domain.ValueObjects;

public class HallOfFameScherm : IScherm
{
    private readonly IMediator _mediator;

    private const string TerugOptie = "⬅️   Terug";

    private const string AlleModiOptie = "🔀  Alle";
    private const string AlleenVolgorde = "✏️  Oefenen";
    private const string AlleenWillekeurig = "🎮  Spelen";

    public HallOfFameScherm(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task ToonAsync()
    {
        int? tafelFilter = null;
        GameMode? modusFilter = null;
        Guid? spelerFilterId = null;
        string? spelerFilterNaam = null;

        var spelers = await _mediator.QueryAsync(new GetPlayersQuery());

        while (true)
        {
            AsciiArt.Toon();
            Thema.SchrijfSectieHeader("Hall of Fame");

            // 1. Data ophalen
            var entries = tafelFilter == null
                ? (await _mediator.QueryAsync(new GetHallOfFameOverallQuery(modusFilter, spelerFilterId)))
                    .Select(e => (e.Rank, e.PlayerName, e.TableNumber, e.TotalTimeMs, e.ErrorCount, e.Date)).ToList()
                : (await _mediator.QueryAsync(new GetHallOfFameByTableQuery(tafelFilter.Value, modusFilter, spelerFilterId)))
                    .Select(e => (e.Rank, e.PlayerName, e.TableNumber, e.TotalTimeMs, e.ErrorCount, e.Date)).ToList();

            // 2. Tabel tonen
            var titelTafel  = tafelFilter == null ? "Overall Hall of Fame" : $"Hall of Fame — Tafel {tafelFilter}";
            var titelModus  = modusFilter switch { GameMode.Volgorde => " — Oefenen", GameMode.Willekeurig => " — Spelen", _ => "" };
            var titelSpeler = spelerFilterNaam == null ? "" : $" — {spelerFilterNaam}";
            ToonTabel(entries, $"{titelTafel}{titelModus}{titelSpeler}");

            // 3. Filter-menu
            var tafelLabel  = tafelFilter == null ? "📊  Tafel: Alle tafels" : $"📊  Tafel: Tafel {tafelFilter}";
            var modusLabel  = modusFilter switch
            {
                GameMode.Volgorde    => "🎯  Modus: Oefenen",
                GameMode.Willekeurig => "🎯  Modus: Spelen",
                _                    => "🎯  Modus: Alle"
            };
            var spelerLabel = spelerFilterNaam == null ? "👤  Speler: Alle spelers" : $"👤  Speler: {spelerFilterNaam}";

            var actie = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Filter of ga terug:[/]")
                    .AddChoices(tafelLabel, modusLabel, spelerLabel, TerugOptie));

            if (actie == TerugOptie) return;

            if (actie == tafelLabel)
            {
                var keuzes = new[] { "Alle tafels" }.Concat(Enumerable.Range(1, 10).Select(i => $"Tafel {i}")).ToList();
                var gekozen = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[yellow]Welke tafel?[/]")
                        .AddChoices(keuzes));
                tafelFilter = gekozen == "Alle tafels" ? null : Thema.ParseTafelKeuze(gekozen);
            }
            else if (actie == modusLabel)
            {
                var gekozen = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[yellow]Welke modus?[/]")
                        .AddChoices(AlleModiOptie, AlleenVolgorde, AlleenWillekeurig));
                modusFilter = gekozen switch
                {
                    AlleenVolgorde    => GameMode.Volgorde,
                    AlleenWillekeurig => GameMode.Willekeurig,
                    _                 => null
                };
            }
            else // spelerLabel
            {
                var alleSpelersOptie = "Alle spelers";
                var spelersKeuzes = new[] { alleSpelersOptie }.Concat(spelers.OrderBy(p => p.Name).Select(p => p.Name)).ToList();
                var gekozen = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[yellow]Welke speler?[/]")
                        .AddChoices(spelersKeuzes));
                if (gekozen == alleSpelersOptie)
                {
                    spelerFilterId   = null;
                    spelerFilterNaam = null;
                }
                else
                {
                    var speler = spelers.First(p => p.Name == gekozen);
                    spelerFilterId   = speler.Id;
                    spelerFilterNaam = speler.Name;
                }
            }
        }
    }

    private static void ToonTabel(List<(int Rank, string Naam, int Tafel, long TijdMs, int Fouten, DateTime Datum)> entries, string titel)
    {
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine($"[gold1 bold]{Markup.Escape(titel)}[/]\n");

        if (entries.Count == 0)
        {
            AnsiConsole.MarkupLine("[grey]Nog geen scores.[/]");
            return;
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Gold1)
            .AddColumn(new TableColumn("[gold1]#[/]").Centered())
            .AddColumn(new TableColumn("[gold1]Naam[/]"))
            .AddColumn(new TableColumn("[gold1]Tafel[/]").Centered())
            .AddColumn(new TableColumn("[gold1]Tijd[/]").Centered())
            .AddColumn(new TableColumn("[gold1]Fouten[/]").Centered())
            .AddColumn(new TableColumn("[gold1]Datum[/]").Centered());

        foreach (var e in entries)
        {
            var medal = e.Rank switch { 1 => "🥇", 2 => "🥈", 3 => "🥉", _ => e.Rank.ToString() };
            table.AddRow(
                medal,
                Markup.Escape(e.Naam),
                e.Tafel.ToString(),
                $"{e.TijdMs / 1000.0:F1}s",
                e.Fouten.ToString(),
                e.Datum.ToLocalTime().ToString("dd-MM-yyyy"));
        }

        AnsiConsole.Write(table);
    }
}
