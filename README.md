# Tafels Stampen

Een interactieve console-applicatie om vermenigvuldigingstafels te oefenen en te leren.

## Wat is het?

Tafels Stampen is een leerspel waarbij je de vermenigvuldigingstafels van 1 t/m 10 kunt oefenen.
Je kiest een naam, kiest een tafel, en beantwoordt alle 10 sommen (in volgorde of willekeurig).
Per som wordt de reactietijd bijgehouden. Na het spel zie je je resultaten, en de beste scores staan in de Hall of Fame.

## Vereisten

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

## Installatie & starten

```bash
# Project klonen
git clone https://github.com/jouw-gebruikersnaam/TafelsStampen.git
cd TafelsStampen

# Starten
dotnet run --project src/TafelsStampen.Console
```

De eerste keer worden automatisch lege data-bestanden aangemaakt. Er zijn geen extra stappen nodig.

## Testen

```bash
dotnet test
```

38 tests verdeeld over Domain, Application en Infrastructure.

## Hoe werkt het?

1. **Speler kiezen** — voer je naam in of kies een bestaande speler
2. **Tafel kiezen** — kies een tafel van 1 t/m 10
3. **Modus kiezen** — volgorde (1×1 t/m 1×10) of willekeurig
4. **Spelen** — beantwoord alle 10 sommen zo snel mogelijk
5. **Resultaten** — zie je tijd per som, totaaltijd en aantal fouten
6. **Hall of Fame** — bekijk de beste scores per tafel of overall

## Data-opslag

Scores en spelers worden opgeslagen in JSON-bestanden op:

| Platform | Locatie                                          |
|----------|--------------------------------------------------|
| Windows  | `%LOCALAPPDATA%\TafelsStampen\data\`             |
| Linux    | `~/.local/share/TafelsStampen/data/`             |
| macOS    | `~/Library/Application Support/TafelsStampen/data/` |

Bestanden:
- `players.json` — geregistreerde spelers
- `sessions.json` — gespeelde sessies
- `halloffame.json` — Hall of Fame scores

## Technische stack

| Onderdeel       | Technologie                      |
|-----------------|----------------------------------|
| Taal            | C# (.NET 10)                     |
| Architectuur    | Clean Architecture               |
| Ontwerppatronen | Domain-Driven Design, CQRS       |
| Console UI      | Spectre.Console                  |
| Persistentie    | System.Text.Json (JSON-bestanden)|
| Tests           | xUnit, Shouldly, Moq             |

## Projectstructuur

```
TafelsStampen/
├── src/
│   ├── TafelsStampen.Domain/         # Domeinlogica, entiteiten, value objects
│   ├── TafelsStampen.Application/    # CQRS commands, queries, eigen mediator
│   ├── TafelsStampen.Infrastructure/ # JSON repositories, persistentie
│   └── TafelsStampen.Console/        # Console UI met Spectre.Console
└── tests/
    ├── TafelsStampen.Domain.Tests/
    ├── TafelsStampen.Application.Tests/
    └── TafelsStampen.Infrastructure.Tests/
```
