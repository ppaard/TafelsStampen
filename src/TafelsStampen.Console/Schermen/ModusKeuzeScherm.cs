namespace TafelsStampen.Console.Schermen;
using Spectre.Console;
using TafelsStampen.Console.Navigatie;
using TafelsStampen.Console.Stijl;
using TafelsStampen.Domain.ValueObjects;

public class ModusKeuzeScherm : IScherm
{
    private readonly SpelScherm _spelScherm;

    private const string VolgordeOptie    = "📋  Volgorde (1 t/m 10)";
    private const string WillekeurigOptie = "🎲  Willekeurig";

    public Guid SpelerId { get; set; }
    public int TafelNummer { get; set; }

    public ModusKeuzeScherm(SpelScherm spelScherm)
    {
        _spelScherm = spelScherm;
    }

    public async Task ToonAsync()
    {
        AsciiArt.Toon();
        Thema.SchrijfSectieHeader("Kies een modus");

        var keuze = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Hoe wil je de sommen oefenen?[/]")
                .AddChoices(VolgordeOptie, WillekeurigOptie));

        var modus = keuze == VolgordeOptie ? GameMode.Volgorde : GameMode.Willekeurig;

        _spelScherm.SpelerId = SpelerId;
        _spelScherm.TafelNummer = TafelNummer;
        _spelScherm.Modus = modus;
        await _spelScherm.ToonAsync();
    }
}
