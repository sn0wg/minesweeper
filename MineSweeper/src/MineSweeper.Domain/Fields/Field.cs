namespace MineSweeper.Domain.Fields;

internal class Field : IInternalField
{
    public string Identifier => $"X{Column}-Y{Line}";
    public FieldState State { get; private set; }
    public FieldFlag Flag { get; set; }
    public int Line { get; }
    public int Column { get; }
    public int? AdjacentBombsQty => _adjacentBombsQty;
    public bool HasBomb { get; private set; }
    private IDictionary<string, IInternalField> _fieldMap;
    private int? _adjacentBombsQty;

    internal Field(int line, int column)
    {
        Line = line;
        Column = column;
        Flag = FieldFlag.None;
        State = FieldState.Hidden;
        _fieldMap = new Dictionary<string, IInternalField>();
        _adjacentBombsQty = null;
    }

    public int Reveal()
    {
        if (State != FieldState.Hidden)
            return 0;

        if (HasBomb)
        {
            State = FieldState.Exploded;
            return 0;
        }

        State = FieldState.Revealed;

        _adjacentBombsQty = GetAdjacentBombs();

        if (_adjacentBombsQty > 0)
            return 1;

        var noBombFields = _fieldMap.Values.Where(x => x.State == FieldState.Hidden && !x.HasBomb);

        foreach (var field in noBombFields)
            field.Reveal();

        return noBombFields.Count();
    }

    public void Plant()
    {
        HasBomb = true;
    }

    public bool Link(IInternalField field)
    {
        if (field == this || field.Identifier == Identifier)
            return false;

        if (_fieldMap.ContainsKey(field.Identifier))
            return false;

        if (field.Column != Column - 1 && field.Column != Column && field.Column != Column + 1)
            return false;

        if (field.Line != Line - 1 && field.Line != Line && field.Line != Line + 1)
            return false;

        _fieldMap.Add(field.Identifier, field);

        field.Link(this);

        return true;
    }

    private int? GetAdjacentBombs()
    {
        if (State == FieldState.Hidden)
            return null;

        return _fieldMap.Values.Where(x => x.HasBomb).Count();
    }
}
