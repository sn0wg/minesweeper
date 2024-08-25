using MineSweeper.Domain.Fields;
using System.Linq;

namespace MineSweeper.Domain.Boards;

internal class Board : IBoard
{
    public IReadOnlyCollection<IReadOnlyCollection<IField>> Fields => _linesAndColumns;
    public int BombQty { get; }
    private readonly IFieldFabric _fieldFabric;
    private readonly IInternalField[][] _linesAndColumns;

    internal Board(int lines, int columns, int bombQty, IFieldFabric fabric)
    {
        _fieldFabric = fabric;
        _linesAndColumns = new IInternalField[lines][];

        for (int i = 0; i < lines; i++)
        {
            _linesAndColumns[i] = new IInternalField[columns];
        }

        InitBoard(bombQty);
        BombQty = bombQty;
    }

    private void InitBoard(int bombQty)
    {
        // Populate
        for(var line = 0; line < _linesAndColumns.Length; line++)
        {
            for(var column = 0; column < _linesAndColumns[line].Length; column++)
            {
                _linesAndColumns[line][column] = _fieldFabric.Create(column, line);
            }
        }


        // Link
        var columnsEvaluteAction = (IInternalField[] line, IInternalField field) =>
        {
            if (line.ElementAtOrDefault(field.Column + 1) != null)
                field.Link(line[field.Column + 1]);
            if (line.ElementAtOrDefault(field.Column - 1) != null)
                field.Link(line[field.Column - 1]);
            if (line.ElementAtOrDefault(field.Column) != null)
                field.Link(line[field.Column]);
        };
        foreach(var fieldLine in _linesAndColumns)
        {
            foreach(var field in fieldLine)
            {
                columnsEvaluteAction(fieldLine, field);

                var line = _linesAndColumns.ElementAtOrDefault(field.Line + 1);
                if (line != null)
                    columnsEvaluteAction(line, field);

                line = _linesAndColumns.ElementAtOrDefault(field.Line - 1);
                if (line != null)
                    columnsEvaluteAction(line, field);
            }
        }

        // Plant bombs
        var random = new Random();
        do
        {
            var line = random.Next(_linesAndColumns.Length);
            var column = random.Next(_linesAndColumns[line].Length);
            var field = _linesAndColumns[line][column];

            if (field.HasBomb)
                continue;

            field.Plant();
            bombQty--;
        } while(bombQty > 0);
    }
}
