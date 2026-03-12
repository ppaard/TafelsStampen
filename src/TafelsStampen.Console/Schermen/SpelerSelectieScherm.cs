namespace TafelsStampen.Console.Schermen;
using Spectre.Console;
using TafelsStampen.Application.Commands.RegisterPlayer;
using TafelsStampen.Application.Mediator;
using TafelsStampen.Application.Queries.GetPlayers;
using TafelsStampen.Console.Navigatie;
using TafelsStampen.Console.Stijl;
using TafelsStampen.Domain.ValueObjects;

public class SpelerSelectieScherm : IScherm
{
    private readonly IMediator _mediator;
    private readonly TafelKeuzeScherm _tafelKeuze;

    private const string NieuweSpelerOptie = "➕  Nieuwe speler";
    private const string TerugOptie = "⬅️  Terug";

    public GameMode Modus { get; set; }

    public SpelerSelectieScherm(IMediator mediator, TafelKeuzeScherm tafelKeuze)
    {
        _mediator = mediator;
        _tafelKeuze = tafelKeuze;
    }

    public async Task ToonAsync()
    {
        while (true)
        {
            AsciiArt.Toon();
            Thema.SchrijfSectieHeader("Speler selecteren");

            var spelers = await _mediator.QueryAsync(new GetPlayersQuery());

            Guid spelerId;
            string naam;

            if (spelers.Count > 0)
            {
                var keuzes = spelers.Select(s => s.Name).ToList();
                keuzes.Add(NieuweSpelerOptie);
                keuzes.Add(TerugOptie);

                var keuze = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[yellow]Kies een speler of maak een nieuwe aan:[/]")
                        .AddChoices(keuzes));

                if (keuze == TerugOptie)
                    return;

                if (keuze == NieuweSpelerOptie)
                {
                    naam = VraagNaam();
                    spelerId = await _mediator.SendAsync(new RegisterPlayerCommand(naam));
                }
                else
                {
                    var speler = spelers.First(s => s.Name == keuze);
                    spelerId = speler.Id;
                    naam = speler.Name;
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[grey]Nog geen spelers. Voer een naam in:[/]");
                naam = VraagNaam();
                spelerId = await _mediator.SendAsync(new RegisterPlayerCommand(naam));
            }

            AnsiConsole.MarkupLine($"\n[green]Welkom, [bold]{Markup.Escape(naam)}[/]![/]\n");
            await Task.Delay(800);

            _tafelKeuze.SpelerId = spelerId;
            _tafelKeuze.Modus = Modus;
            await _tafelKeuze.ToonAsync();

            if (_tafelKeuze.TerugGedrukt)
                continue;

            return;
        }
    }

    private static string VraagNaam() =>
        AnsiConsole.Prompt(
            new TextPrompt<string>("[yellow]Jouw naam:[/]")
                .ValidationErrorMessage("[red]Naam mag niet leeg zijn, max 30 tekens, en niet gelijk aan een systeemnaam.[/]")
                .Validate(n => !string.IsNullOrWhiteSpace(n)
                               && n.Trim().Length <= 30
                               && n.Trim() != NieuweSpelerOptie
                    ? ValidationResult.Success()
                    : ValidationResult.Error()))
        .Trim();
}
