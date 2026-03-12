namespace TafelsStampen.Console.Schermen;
using Spectre.Console;
using TafelsStampen.Console.Navigatie;
using TafelsStampen.Console.Stijl;
using TafelsStampen.Domain.ValueObjects;

public class HoofdmenuScherm : IScherm
{
    private readonly NavigatieService _navigatie;
    private readonly SpelerSelectieScherm _spelerSelectie;
    private readonly HallOfFameScherm _hallOfFame;
    private readonly InstellingenScherm _instellingen;

    private const string OefenesOptie    = "✏️  Oefenen";
    private const string SpelenOptie     = "🎮  Spelen";
    private const string HallOfFameOptie = "🏆  Hall of Fame";
    private const string InstellingenOptie = "⚙️   Instellingen";
    private const string AfsluitenOptie  = "🚪  Afsluiten";

    public HoofdmenuScherm(
        NavigatieService navigatie,
        SpelerSelectieScherm spelerSelectie,
        HallOfFameScherm hallOfFame,
        InstellingenScherm instellingen)
    {
        _navigatie = navigatie;
        _spelerSelectie = spelerSelectie;
        _hallOfFame = hallOfFame;
        _instellingen = instellingen;
    }

    public async Task ToonAsync()
    {
        while (true)
        {
            AsciiArt.Toon();

            var keuze = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[gold1]Wat wil je doen?[/]")
                    .PageSize(6)
                    .AddChoices(
                        OefenesOptie,
                        SpelenOptie,
                        HallOfFameOptie,
                        InstellingenOptie,
                        AfsluitenOptie));

            switch (keuze)
            {
                case OefenesOptie:
                    _spelerSelectie.Modus = GameMode.Volgorde;
                    await _navigatie.NaarAsync(_spelerSelectie);
                    break;
                case SpelenOptie:
                    _spelerSelectie.Modus = GameMode.Willekeurig;
                    await _navigatie.NaarAsync(_spelerSelectie);
                    break;
                case HallOfFameOptie:
                    await _navigatie.NaarAsync(_hallOfFame);
                    break;
                case InstellingenOptie:
                    await _navigatie.NaarAsync(_instellingen);
                    break;
                case AfsluitenOptie:
                    AnsiConsole.MarkupLine("\n[grey]Tot ziens![/]");
                    return;
            }
        }
    }
}
