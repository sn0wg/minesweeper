namespace MineSweeper.Domain.Fields;

internal interface IInternalField : IField
{
    public bool HasBomb { get; }
    public bool Link(IInternalField field);
}
