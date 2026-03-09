namespace TafelsStampen.Console.Stijl;
using Spectre.Console;

public static class AsciiArt
{
    public static void Toon()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new FigletText("Tafels Stampen")
            .Centered()
            .Color(Color.Gold1));
        AnsiConsole.Write(new Rule("[cyan1]Leer de tafels van 1 t/m 10![/]")
            .Centered());
        AnsiConsole.WriteLine();
    }
}
