namespace TafelsStampen.Console.Schermen;
using Spectre.Console;
using TafelsStampen.Application.Mediator;
using TafelsStampen.Application.Queries.GetGameResult;
using TafelsStampen.Console.Navigatie;
using TafelsStampen.Console.Stijl;

public class ResultatenScherm : IScherm
{
    private readonly IMediator _mediator;

    public Guid SessionId { get; set; }

    public ResultatenScherm(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task ToonAsync()
    {
        var resultaat = await _mediator.QueryAsync(new GetGameResultQuery(SessionId));

        AsciiArt.Toon();
        Thema.SchrijfSectieHeader("Resultaten");

        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.CornflowerBlue)
            .AddColumn(new TableColumn("[cyan1]Som[/]").Centered())
            .AddColumn(new TableColumn("[cyan1]Jouw antwoord[/]").Centered())
            .AddColumn(new TableColumn("[cyan1]Correct[/]").Centered())
            .AddColumn(new TableColumn("[cyan1]Tijd[/]").Centered());

        foreach (var a in resultaat.Answers)
        {
            var correctSymbol = a.IsCorrect ? "[green]✓[/]" : "[red]✗[/]";
            table.AddRow(
                $"{a.Multiplicand} × {a.Multiplier}",
                a.GivenAnswer.ToString(),
                correctSymbol,
                $"{a.ReactionTimeMs} ms");
        }

        AnsiConsole.Write(table);

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine($"[gold1]Totaaltijd:[/] [white]{resultaat.TotalTimeMs / 1000.0:F1} seconden[/]");
        AnsiConsole.MarkupLine($"[gold1]Fouten:[/] [white]{resultaat.ErrorCount}[/]");

        AnsiConsole.MarkupLine(resultaat.ErrorCount switch
        {
            0 => "\n[green bold]🎉 Perfect! Geen enkele fout![/]",
            <= 2 => "\n[yellow]Goed gedaan! Bijna perfect![/]",
            _ => "\n[red]Blijf oefenen! Je kunt het![/]"
        });

        Thema.WachtOpEnter();
    }
}
