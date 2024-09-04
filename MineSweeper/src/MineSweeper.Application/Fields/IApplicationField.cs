using MineSweeper.Domain.Fields;

namespace MineSweeper.Application.Fields;

public interface IApplicationField
{
    string Identifier { get; }
    FieldState State { get; }
    FieldFlag Flag { get; }
    int Line { get; }
    int Column { get; }
    int? AdjacentBombsQty { get; }
}
