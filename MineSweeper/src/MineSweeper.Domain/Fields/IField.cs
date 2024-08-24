namespace MineSweeper.Domain.Fields;

public interface IField
{
    string Identifier { get; }
    FieldState State { get; }
    FieldFlag Flag { get; set; }
    ushort Line { get; }
    ushort Column { get; }
    ushort? AdjacentBombsQty { get; }
    void Reveal();
}
