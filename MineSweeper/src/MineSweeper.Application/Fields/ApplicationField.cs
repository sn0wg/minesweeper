using MineSweeper.Domain.Fields;

namespace MineSweeper.Application.Fields;

internal class ApplicationField : IApplicationField
{
    private readonly IField _field;

    public ApplicationField(IField field)
    {
        _field = field;
    }

    public string Identifier => _field.Identifier;

    public FieldState State => _field.State;

    public FieldFlag Flag => _field.Flag;

    public int Line => _field.Line;

    public int Column => _field.Column;

    public int? AdjacentBombsQty => _field.AdjacentBombsQty;
}
