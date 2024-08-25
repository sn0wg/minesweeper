namespace MineSweeper.Domain.Fields;

public interface IField
{
    string Identifier { get; }
    FieldState State { get; }
    FieldFlag Flag { get; set; }
    int Line { get; }
    int Column { get; }
    int? AdjacentBombsQty { get; }
    void Reveal();
}
