namespace MineSweeper.Domain.Fields;

internal class Field : IInternalField
{
    public string Identifier => $"X{Column}-Y{Line}";
    public FieldState State { get; private set; }
    public FieldFlag Flag { get; set; }
    public int Line { get; }
    public int Column { get; }
    public int? AdjacentBombsQty => State == FieldState.Hidden ? null : _adjacentBombsQty;
    public bool HasBomb { get; private set; }
    private IDictionary<string, IInternalField> _fieldMap;
    private int _adjacentBombsQty;

    internal Field(int line, int column)
    {
        Line = line;
        Column = column;
        Flag = FieldFlag.None;
        State = FieldState.Hidden;
        _fieldMap = new Dictionary<string, IInternalField>();
        _adjacentBombsQty = 0;
    }

    public int Reveal()
    {
        if (State != FieldState.Hidden)
            return 0;

        var reveleadQty = 1;

        _adjacentBombsQty = _fieldMap.Values.Where(x => x.HasBomb).Count();

        if (HasBomb)
        {
            State = FieldState.Exploded;
            return reveleadQty;
        }

        State = FieldState.Revealed;

        if (_adjacentBombsQty > 0)
            return reveleadQty;        

        var noBombFields = _fieldMap.Values.Where(x => x.State == FieldState.Hidden && !x.HasBomb);

        foreach (var field in noBombFields)
            reveleadQty += field.Reveal();

        _adjacentBombsQty = _fieldMap.Values.Where(x => x.HasBomb).Count();

        return reveleadQty;
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
}
