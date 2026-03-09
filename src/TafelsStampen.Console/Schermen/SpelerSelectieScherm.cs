namespace TafelsStampen.Console.Schermen;
using Spectre.Console;
using TafelsStampen.Application.Commands.RegisterPlayer;
using TafelsStampen.Application.Mediator;
using TafelsStampen.Application.Queries.GetPlayers;
using TafelsStampen.Console.Navigatie;
using TafelsStampen.Console.Stijl;

public class SpelerSelectieScherm : IScherm
{
    private readonly IMediator _mediator;
    private readonly TafelKeuzeScherm _tafelKeuze;

    private const string NieuweSpelerOptie = "➕  Nieuwe speler";

    public SpelerSelectieScherm(IMediator mediator, TafelKeuzeScherm tafelKeuze)
    {
        _mediator = mediator;
        _tafelKeuze = tafelKeuze;
    }

    public async Task ToonAsync()
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

            var keuze = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Kies een speler of maak een nieuwe aan:[/]")
                    .AddChoices(keuzes));

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
        await _tafelKeuze.ToonAsync();
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
