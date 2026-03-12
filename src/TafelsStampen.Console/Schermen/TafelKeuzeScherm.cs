namespace TafelsStampen.Console.Schermen;
using Spectre.Console;
using TafelsStampen.Console.Navigatie;
using TafelsStampen.Console.Stijl;
using TafelsStampen.Domain.ValueObjects;

public class TafelKeuzeScherm : IScherm
{
    private readonly SpelScherm _spelScherm;

    public Guid SpelerId { get; set; }
    public GameMode Modus { get; set; }

    public TafelKeuzeScherm(SpelScherm spelScherm)
    {
        _spelScherm = spelScherm;
    }

    public async Task ToonAsync()
    {
        AsciiArt.Toon();
        Thema.SchrijfSectieHeader("Kies een tafel");

        var grid = new Grid();
        for (int i = 0; i < 5; i++) grid.AddColumn();
        grid.AddRow(Enumerable.Range(1, 5).Select(i => $"[gold1]{i,3}[/]").ToArray());
        grid.AddRow(Enumerable.Range(6, 5).Select(i => $"[gold1]{i,3}[/]").ToArray());
        AnsiConsole.Write(grid);
        AnsiConsole.WriteLine();

        var titel = Modus == GameMode.Volgorde
            ? "[yellow]Welke tafel wil je oefenen?[/]"
            : "[yellow]Welke tafel wil je spelen?[/]";

        var keuze = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(titel)
                .PageSize(10)
                .AddChoices(Enumerable.Range(1, 10).Select(i => $"Tafel {i}")));

        _spelScherm.SpelerId = SpelerId;
        _spelScherm.TafelNummer = Thema.ParseTafelKeuze(keuze);
        _spelScherm.Modus = Modus;
        await _spelScherm.ToonAsync();
    }
}
