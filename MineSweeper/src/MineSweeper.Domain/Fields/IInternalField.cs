namespace MineSweeper.Domain.Fields;

internal interface IInternalField : IField
{
    public bool HasBomb { get; }
    public void Plant();
    public bool Link(IInternalField field);
}
