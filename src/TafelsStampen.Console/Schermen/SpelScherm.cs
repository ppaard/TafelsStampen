namespace TafelsStampen.Console.Schermen;
using System.Diagnostics;
using Spectre.Console;
using TafelsStampen.Application.Commands.FinishGame;
using TafelsStampen.Application.Commands.StartGame;
using TafelsStampen.Application.Commands.SubmitAnswer;
using TafelsStampen.Application.Mediator;
using TafelsStampen.Console.Navigatie;
using TafelsStampen.Console.Stijl;
using TafelsStampen.Domain.ValueObjects;

public class SpelScherm : IScherm
{
    private readonly IMediator _mediator;
    private readonly ResultatenScherm _resultaten;

    public Guid SpelerId { get; set; }
    public int TafelNummer { get; set; }
    public GameMode Modus { get; set; }

    public SpelScherm(IMediator mediator, ResultatenScherm resultaten)
    {
        _mediator = mediator;
        _resultaten = resultaten;
    }

    public async Task ToonAsync()
    {
        var sessionId = await _mediator.SendAsync(new StartGameCommand(SpelerId, TafelNummer, Modus));

        var multipliers = Enumerable.Range(1, 10).ToArray();
        if (Modus == GameMode.Willekeurig)
            Random.Shared.Shuffle(multipliers);

        for (int i = 0; i < multipliers.Length; i++)
        {
            var b = multipliers[i];
            AsciiArt.Toon();

            Thema.SchrijfSectieHeader($"Tafel {TafelNummer} — Vraag {i + 1} van 10");
            var voortgang = $"[{new string('█', i)}{new string('░', 10 - i)}]";
            AnsiConsole.MarkupLine($"[grey]Voortgang: {Markup.Escape(voortgang)}[/]\n");

            int antwoord;
            long ms;

            while (true)
            {
                var stopwatch = Stopwatch.StartNew();
                var input = AnsiConsole.Prompt(
                    new TextPrompt<string>($"[yellow bold]{TafelNummer} × {b} = [/]")
                        .AllowEmpty());
                stopwatch.Stop();

                if (int.TryParse(input, out antwoord))
                {
                    ms = stopwatch.ElapsedMilliseconds;
                    break;
                }

                AnsiConsole.MarkupLine("[red]Voer een geldig getal in![/]");
            }

            var isCorrect = await _mediator.SendAsync(
                new SubmitAnswerCommand(sessionId, TafelNummer, b, antwoord, ms));

            if (isCorrect)
                AnsiConsole.MarkupLine($"\n[green]✓ Correct! ({ms} ms)[/]");
            else
                AnsiConsole.MarkupLine($"\n[red]✗ Fout! Het goede antwoord is {TafelNummer * b}. ({ms} ms)[/]");

            await Task.Delay(1200);
        }

        await _mediator.SendAsync(new FinishGameCommand(sessionId));

        _resultaten.SessionId = sessionId;
        await _resultaten.ToonAsync();
    }
}
