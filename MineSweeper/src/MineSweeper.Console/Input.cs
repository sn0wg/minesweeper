using MineSweeper.Application.Match;
using MineSweeper.Domain.Fields;
using Spectre.Console;

namespace MineSweeper.Console;

public static class Input
{
    public static MatchDifficultyEnum ReadDifficulty()
    {
        var difficulty = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Choose a game difficulty")
                .AddChoices(new[] {
                    "Easy", "Normal", "Hard"
                }));

        return  Enum.Parse<MatchDifficultyEnum>(difficulty);
    }

    public static ActionEnum ReadAction()
    {
        var action = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Choose a action")
                .AddChoices(new[] {
                    "Reveal", "Mark"
                }));

        return Enum.Parse<ActionEnum>(action);
    }

    public static FieldFlag ReadFlag()
    {
        var action = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Choose a flag")
                .AddChoices(new[] {
                    "None", "Bomb"
                }));

        return Enum.Parse<FieldFlag>(action);
    }

    public static int ReadLine()
    {
        var line = AnsiConsole.Prompt(
            new TextPrompt<int>("Choose a line:"));

        return line;
    }

    public static int ReadColumn()
    {
        var column = AnsiConsole.Prompt(
            new TextPrompt<int>("Choose a column:"));

        return column;
    }
}
