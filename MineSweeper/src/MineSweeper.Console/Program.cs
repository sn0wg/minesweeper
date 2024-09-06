using MineSweeper.Application.Match;
using MineSweeper.Application.Matchs;
using MineSweeper.Console;
using MineSweeper.Domain.Boards;
using MineSweeper.Domain.Fields;
using Spectre.Console;
using Table = MineSweeper.Console.Table;

Logo.Print();

var difficulty = Input.ReadDifficulty();

var match = new DefaultMatch(difficulty, new BoardFabric());

var table = new Table(match);

do
{
    Console.Clear();

    Logo.Print();

    table.Render();

    var action = Input.ReadAction();

    AnsiConsole.WriteLine($"Action: {action}");

    var line = Input.ReadLine();

    var column = Input.ReadColumn();

    if(action == ActionEnum.Reveal)
    {
        match.Reveal(column, line);
        continue;
    }

    var flag = Input.ReadFlag();

    match.Mark(column, line, flag);
    
} while (match.State != MatchStateEnum.Finished);

Console.Clear();

Logo.Print();

match.RevealAll();

table.Render();

var resultText = match.Result switch
{
    MatchResultEnum.Won => "Winner!",
    MatchResultEnum.Loss => "Looser!",
    MatchResultEnum.None => throw new NotImplementedException(),
};

var color = match.Result switch
{
    MatchResultEnum.Won => Color.Green,
    MatchResultEnum.Loss => Color.Red,
    MatchResultEnum.None => throw new NotImplementedException(),
};

AnsiConsole.Write(new FigletText(resultText)
                .Centered()
                .Color(color));