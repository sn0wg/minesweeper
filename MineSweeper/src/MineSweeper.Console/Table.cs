using MineSweeper.Application.Fields;
using MineSweeper.Application.Matchs;
using MineSweeper.Domain.Fields;
using Spectre.Console;

namespace MineSweeper.Console;

public class Table
{
    private readonly IMatch _match;
    private readonly int _columns;
    private readonly int _lines;

    public Table(IMatch match)
    {
        _match = match;

        _columns = match.Fields.Select(x => x.Count).Max();
        _lines = match.Fields.Count;
    }

    public void Render()
    {
        var grid = new Grid();

        var headerLine = new List<Text>()
        {
            new Text("")
        };

        grid.AddColumn();
        for (var i = 0; i < _columns; i++)
        {
            grid.AddColumn();
            headerLine.Add(new Text($"{i}", new Style(decoration: Decoration.Underline)));
        }

        grid.AddRow(headerLine.ToArray());
            

        for (var lineIndex = 0; lineIndex < _lines; lineIndex++)
        {
            var lineText = new List<Text>()
            {
                new Text($"{lineIndex}", new Style(decoration: Decoration.Underline))
            };
            var line = _match.Fields.ElementAt(lineIndex);
            for (var columnIndex = 0; columnIndex < line.Count; columnIndex++)
            {
                var field = line.ElementAt(columnIndex);
                var text = GetFieldText(field).Centered();
                lineText.Add(text);
            }

            grid.AddRow(lineText.ToArray());
        }

        // Write centered cell grid contents to Console
        AnsiConsole.WriteLine($"Bombs: {_match.RemainingBombs} - Fields: {_match.RemainingFields}");
        AnsiConsole.Write(grid);
    }

    public Text GetFieldText(IApplicationField field)
    {
        if (field.State == FieldState.Hidden)
        {
            var color = field.Flag == FieldFlag.None ? Color.White : Color.Red;
            return new Text("O", new Style(color, decoration: Decoration.Bold));
        }
            

        if (field.State == FieldState.Exploded)
            return new Text("X", new Style(decoration: Decoration.Bold));

        if (field.State == FieldState.Revealed)
        {
            var color = field.AdjacentBombsQty == 0 ? Color.Green : Color.Red;
            return new Text($"{field.AdjacentBombsQty}", new Style(color, decoration: Decoration.Bold));
        }

        return new Text("?");
    }
}
