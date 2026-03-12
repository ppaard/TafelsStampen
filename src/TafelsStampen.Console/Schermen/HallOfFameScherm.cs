namespace TafelsStampen.Console.Schermen;
using Spectre.Console;
using TafelsStampen.Application.Mediator;
using TafelsStampen.Application.Queries;
using TafelsStampen.Application.Queries.GetHallOfFameByTable;
using TafelsStampen.Application.Queries.GetHallOfFameOverall;
using TafelsStampen.Application.Queries.GetPlayers;
using TafelsStampen.Console.Navigatie;
using TafelsStampen.Console.Stijl;
using TafelsStampen.Domain.ValueObjects;

public class HallOfFameScherm : IScherm
{
    private readonly IMediator _mediator;

    private const int PaginaGrootte = 25;
    private const string VolgendePagina = "➡️   Volgende pagina";
    private const string VorigePagina   = "⬅️   Vorige pagina";

    private const string TerugOptie = "⬅️   Terug";

    private const string AlleModiOptie = "🔀  Alle";
    private const string AlleenVolgorde = "✏️  Oefenen";
    private const string AlleenWillekeurig = "🎮  Spelen";

    private const string AllePeriodeOptie = "🗓️  Alles";
    private const string PeriodeVandaag   = "🗓️  Vandaag";
    private const string PeriodeDezeWeek  = "🗓️  Deze week";
    private const string PeriodeDezeMaand = "🗓️  Deze maand";
    private const string PeriodeDitJaar   = "🗓️  Dit jaar";

    public int? InitieleTafelFilter { get; set; }
    public GameMode? InitieleModusFilter { get; set; }
    public Guid? InitieleSpelerFilterId { get; set; }
    public string? InitieleSpelerFilterNaam { get; set; }
    public Guid? HuidigeSessionId { get; set; }

    public HallOfFameScherm(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task ToonAsync()
    {
        int? tafelFilter = InitieleTafelFilter;
        GameMode? modusFilter = InitieleModusFilter;
        Guid? spelerFilterId = InitieleSpelerFilterId;
        string? spelerFilterNaam = InitieleSpelerFilterNaam;
        Guid? huidigeSessionId = HuidigeSessionId;
        HuidigeSessionId = null;
        HallOfFamePeriode periodeFilter = HallOfFamePeriode.DezeWeek;
        int pagina = 0;

        var spelers = await _mediator.QueryAsync(new GetPlayersQuery());

        while (true)
        {
            AsciiArt.Toon();
            Thema.SchrijfSectieHeader("Hall of Fame");

            // 1. Data ophalen
            var entries = tafelFilter == null
                ? (await _mediator.QueryAsync(new GetHallOfFameOverallQuery(modusFilter, spelerFilterId, periodeFilter)))
                    .Select(e => (e.Rank, e.PlayerName, e.TableNumber, e.TotalTimeMs, e.ErrorCount, e.Date, e.SessionId)).ToList()
                : (await _mediator.QueryAsync(new GetHallOfFameByTableQuery(tafelFilter.Value, modusFilter, spelerFilterId, periodeFilter)))
                    .Select(e => (e.Rank, e.PlayerName, e.TableNumber, e.TotalTimeMs, e.ErrorCount, e.Date, e.SessionId)).ToList();

            // 2. Paginering berekenen
            var aantalPaginas = (int)Math.Ceiling(entries.Count / (double)PaginaGrootte);
            if (aantalPaginas > 0 && pagina >= aantalPaginas) pagina = aantalPaginas - 1;
            var paginaEntries = entries.Skip(pagina * PaginaGrootte).Take(PaginaGrootte).ToList();

            // 3. Tabel tonen
            var titelTafel   = tafelFilter == null ? "Overall Hall of Fame" : $"Hall of Fame — Tafel {tafelFilter}";
            var titelModus   = modusFilter switch { GameMode.Volgorde => " — Oefenen", GameMode.Willekeurig => " — Spelen", _ => "" };
            var titelSpeler  = spelerFilterNaam == null ? "" : $" — {spelerFilterNaam}";
            var titelPeriode = periodeFilter switch
            {
                HallOfFamePeriode.Vandaag   => " — Vandaag",
                HallOfFamePeriode.DezeWeek  => " — Deze week",
                HallOfFamePeriode.DezeMaand => " — Deze maand",
                HallOfFamePeriode.DitJaar   => " — Dit jaar",
                _                           => ""
            };
            var paginaTitel = aantalPaginas > 1 ? $" — Pagina {pagina + 1} van {aantalPaginas}" : "";
            ToonTabel(paginaEntries, $"{titelTafel}{titelModus}{titelSpeler}{titelPeriode}{paginaTitel}", huidigeSessionId);

            // 4. Filter-menu
            var periodeLabel = periodeFilter switch
            {
                HallOfFamePeriode.Vandaag   => "🗓️  Periode: Vandaag",
                HallOfFamePeriode.DezeWeek  => "🗓️  Periode: Deze week",
                HallOfFamePeriode.DezeMaand => "🗓️  Periode: Deze maand",
                HallOfFamePeriode.DitJaar   => "🗓️  Periode: Dit jaar",
                _                           => "🗓️  Periode: Alles"
            };
            var tafelLabel  = tafelFilter == null ? "📊  Tafel: Alle tafels" : $"📊  Tafel: Tafel {tafelFilter}";
            var modusLabel  = modusFilter switch
            {
                GameMode.Volgorde    => "🎯  Modus: Oefenen",
                GameMode.Willekeurig => "🎯  Modus: Spelen",
                _                    => "🎯  Modus: Alle"
            };
            var spelerLabel = spelerFilterNaam == null ? "👤  Speler: Alle spelers" : $"👤  Speler: {spelerFilterNaam}";

            var menuKeuzes = new List<string>();
            if (pagina > 0) menuKeuzes.Add(VorigePagina);
            menuKeuzes.AddRange([periodeLabel, tafelLabel, modusLabel, spelerLabel]);
            if (aantalPaginas > 1 && pagina < aantalPaginas - 1) menuKeuzes.Add(VolgendePagina);
            menuKeuzes.Add(TerugOptie);

            var actie = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Filter of ga terug:[/]")
                    .AddChoices(menuKeuzes));

            if (actie == VolgendePagina) { pagina++; continue; }
            if (actie == VorigePagina)   { pagina--; continue; }
            if (actie == TerugOptie) return;

            if (actie == periodeLabel)
            {
                var gekozen = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[yellow]Welke periode?[/]")
                        .AddChoices(PeriodeDezeWeek, PeriodeVandaag, PeriodeDezeMaand,
                                    PeriodeDitJaar, AllePeriodeOptie));
                periodeFilter = gekozen switch
                {
                    PeriodeVandaag   => HallOfFamePeriode.Vandaag,
                    PeriodeDezeMaand => HallOfFamePeriode.DezeMaand,
                    PeriodeDitJaar   => HallOfFamePeriode.DitJaar,
                    AllePeriodeOptie => HallOfFamePeriode.Alles,
                    _                => HallOfFamePeriode.DezeWeek
                };
                pagina = 0;
            }
            else if (actie == tafelLabel)
            {
                var keuzes = new[] { "Alle tafels" }.Concat(Enumerable.Range(1, 10).Select(i => $"Tafel {i}")).ToList();
                var gekozen = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[yellow]Welke tafel?[/]")
                        .AddChoices(keuzes));
                tafelFilter = gekozen == "Alle tafels" ? null : Thema.ParseTafelKeuze(gekozen);
                pagina = 0;
            }
            else if (actie == modusLabel)
            {
                var gekozen = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[yellow]Welke modus?[/]")
                        .AddChoices(AlleModiOptie, AlleenVolgorde, AlleenWillekeurig));
                modusFilter = gekozen switch
                {
                    AlleenVolgorde    => GameMode.Volgorde,
                    AlleenWillekeurig => GameMode.Willekeurig,
                    _                 => null
                };
                pagina = 0;
            }
            else // spelerLabel
            {
                var alleSpelersOptie = "Alle spelers";
                var spelersKeuzes = new[] { alleSpelersOptie }.Concat(spelers.OrderBy(p => p.Name).Select(p => p.Name)).ToList();
                var gekozen = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[yellow]Welke speler?[/]")
                        .AddChoices(spelersKeuzes));
                if (gekozen == alleSpelersOptie)
                {
                    spelerFilterId   = null;
                    spelerFilterNaam = null;
                }
                else
                {
                    var speler = spelers.First(p => p.Name == gekozen);
                    spelerFilterId   = speler.Id;
                    spelerFilterNaam = speler.Name;
                }
                pagina = 0;
            }
        }
    }

    private static void ToonTabel(List<(int Rank, string Naam, int Tafel, long TijdMs, int Fouten, DateTime Datum, Guid Id)> entries, string titel, Guid? huidigeId = null)
    {
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine($"[gold1 bold]{Markup.Escape(titel)}[/]\n");

        if (entries.Count == 0)
        {
            AnsiConsole.MarkupLine("[grey]Nog geen scores.[/]");
            return;
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Gold1)
            .AddColumn(new TableColumn("[gold1]#[/]").Centered())
            .AddColumn(new TableColumn("[gold1]Naam[/]"))
            .AddColumn(new TableColumn("[gold1]Tafel[/]").Centered())
            .AddColumn(new TableColumn("[gold1]Tijd[/]").Centered())
            .AddColumn(new TableColumn("[gold1]Fouten[/]").Centered())
            .AddColumn(new TableColumn("[gold1]Datum[/]").Centered());

        foreach (var e in entries)
        {
            var medal = e.Rank switch { 1 => "🥇", 2 => "🥈", 3 => "🥉", _ => e.Rank.ToString() };
            bool isHuidig = huidigeId.HasValue && e.Id == huidigeId.Value;
            Func<string, string> stijl = isHuidig ? s => $"[bold yellow]{s}[/]" : s => s;
            table.AddRow(
                stijl(medal),
                stijl(Markup.Escape(e.Naam)),
                stijl(e.Tafel.ToString()),
                stijl($"{e.TijdMs / 1000.0:F1}s"),
                stijl(e.Fouten.ToString()),
                stijl(e.Datum.ToLocalTime().ToString("dd-MM-yyyy")));
        }

        AnsiConsole.Write(table);
    }
}
