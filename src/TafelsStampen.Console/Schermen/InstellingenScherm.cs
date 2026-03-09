namespace TafelsStampen.Console.Schermen;
using Spectre.Console;
using TafelsStampen.Application.Mediator;
using TafelsStampen.Application.Queries.GetPlayers;
using TafelsStampen.Console.Navigatie;
using TafelsStampen.Console.Stijl;

public class InstellingenScherm : IScherm
{
    private readonly IMediator _mediator;

    private const string BekijkSpelersOptie = "👥  Bekijk spelers";
    private const string TerugOptie          = "⬅️   Terug";

    public InstellingenScherm(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task ToonAsync()
    {
        while (true)
        {
            AsciiArt.Toon();
            Thema.SchrijfSectieHeader("Instellingen");

            var keuze = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Kies een optie:[/]")
                    .AddChoices(BekijkSpelersOptie, TerugOptie));

            if (keuze == TerugOptie) return;

            var spelers = await _mediator.QueryAsync(new GetPlayersQuery());

            AnsiConsole.WriteLine();
            if (spelers.Count == 0)
            {
                AnsiConsole.MarkupLine("[grey]Nog geen spelers aangemeld.[/]");
            }
            else
            {
                var table = new Table()
                    .Border(TableBorder.Rounded)
                    .BorderColor(Color.CornflowerBlue)
                    .AddColumn("[cyan1]Naam[/]")
                    .AddColumn("[cyan1]Aangemeld op[/]");

                foreach (var s in spelers)
                    table.AddRow(Markup.Escape(s.Name), s.CreatedAt.ToLocalTime().ToString("dd-MM-yyyy HH:mm"));

                AnsiConsole.Write(table);
            }

            Thema.WachtOpEnter();
        }
    }
}
