namespace TafelsStampen.Console.Stijl;
using Spectre.Console;

public static class Thema
{
    // Kleurenpalet — referentie voor consistente UI
    public static Style Titel    => new(Color.Gold1, decoration: Decoration.Bold);
    public static Style Accent   => new(Color.Cyan1);
    public static Style Succes   => new(Color.Green);
    public static Style Fout     => new(Color.Red);
    public static Style Subtekst => new(Color.Grey);
    public static Style Vraag    => new(Color.Yellow);

    public static void SchrijfSectieHeader(string titel) =>
        AnsiConsole.MarkupLine($"[cyan1]═══ {Markup.Escape(titel)} ═══[/]\n");

    public static void WachtOpEnter(string bericht = "Druk op Enter om terug te gaan...")
    {
        AnsiConsole.MarkupLine($"\n[grey]{Markup.Escape(bericht)}[/]");
        System.Console.ReadLine();
    }

    public static int ParseTafelKeuze(string keuze) =>
        int.Parse(keuze.Replace("Tafel ", ""));
}
