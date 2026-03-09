namespace TafelsStampen.Console.Schermen;
using Spectre.Console;
using TafelsStampen.Console.Navigatie;
using TafelsStampen.Console.Stijl;

public class TafelKeuzeScherm : IScherm
{
    private readonly ModusKeuzeScherm _modusKeuze;

    public Guid SpelerId { get; set; }

    public TafelKeuzeScherm(ModusKeuzeScherm modusKeuze)
    {
        _modusKeuze = modusKeuze;
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

        var keuze = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Welke tafel wil je oefenen?[/]")
                .PageSize(10)
                .AddChoices(Enumerable.Range(1, 10).Select(i => $"Tafel {i}")));

        _modusKeuze.SpelerId = SpelerId;
        _modusKeuze.TafelNummer = Thema.ParseTafelKeuze(keuze);
        await _modusKeuze.ToonAsync();
    }
}
