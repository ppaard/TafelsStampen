namespace TafelsStampen.Console.Schermen;
using Spectre.Console;
using TafelsStampen.Console.Navigatie;
using TafelsStampen.Console.Stijl;

public class HoofdmenuScherm : IScherm
{
    private readonly NavigatieService _navigatie;
    private readonly SpelerSelectieScherm _spelerSelectie;
    private readonly HallOfFameScherm _hallOfFame;
    private readonly InstellingenScherm _instellingen;

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
                    .PageSize(5)
                    .AddChoices(
                        "🎮  Nieuw spel",
                        "🏆  Hall of Fame",
                        "⚙️   Instellingen",
                        "🚪  Afsluiten"));

            switch (keuze)
            {
                case "🎮  Nieuw spel":
                    await _navigatie.NaarAsync(_spelerSelectie);
                    break;
                case "🏆  Hall of Fame":
                    await _navigatie.NaarAsync(_hallOfFame);
                    break;
                case "⚙️   Instellingen":
                    await _navigatie.NaarAsync(_instellingen);
                    break;
                case "🚪  Afsluiten":
                    AnsiConsole.MarkupLine("\n[grey]Tot ziens![/]");
                    return;
            }
        }
    }
}
