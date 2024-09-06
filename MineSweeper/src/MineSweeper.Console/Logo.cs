using Spectre.Console;

namespace MineSweeper.Console;

public static class Logo
{
    public static void Print()
    {
        AnsiConsole.Write(new FigletText("MineSweeper!")
                .Centered()
                .Color(Color.Red));
    }
}
