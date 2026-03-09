namespace TafelsStampen.Console.Schermen;
using Spectre.Console;
using TafelsStampen.Application.Mediator;
using TafelsStampen.Application.Queries.GetHallOfFameByTable;
using TafelsStampen.Application.Queries.GetHallOfFameOverall;
using TafelsStampen.Console.Navigatie;
using TafelsStampen.Console.Stijl;

public class HallOfFameScherm : IScherm
{
    private readonly IMediator _mediator;

    private const string OverallOptie = "🏆  Overall (alle tafels)";
    private const string PerTafelOptie = "📊  Per tafel";
    private const string TerugOptie = "⬅️   Terug";

    public HallOfFameScherm(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task ToonAsync()
    {
        while (true)
        {
            AsciiArt.Toon();
            Thema.SchrijfSectieHeader("Hall of Fame");

            var keuze = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Bekijk:[/]")
                    .AddChoices(OverallOptie, PerTafelOptie, TerugOptie));

            if (keuze == TerugOptie) return;

            if (keuze == OverallOptie)
            {
                var entries = await _mediator.QueryAsync(new GetHallOfFameOverallQuery());
                ToonTabel(entries.Select(e => (e.Rank, e.PlayerName, e.TableNumber, e.TotalTimeMs, e.ErrorCount, e.Date)).ToList(), "Overall Hall of Fame");
            }
            else
            {
                var tafelKeuze = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[yellow]Welke tafel?[/]")
                        .AddChoices(Enumerable.Range(1, 10).Select(i => $"Tafel {i}")));

                var tafelNr = Thema.ParseTafelKeuze(tafelKeuze);
                var entries = await _mediator.QueryAsync(new GetHallOfFameByTableQuery(tafelNr));
                ToonTabel(entries.Select(e => (e.Rank, e.PlayerName, e.TableNumber, e.TotalTimeMs, e.ErrorCount, e.Date)).ToList(), $"Hall of Fame — Tafel {tafelNr}");
            }

            Thema.WachtOpEnter("Druk op Enter om door te gaan...");
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
