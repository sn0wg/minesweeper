namespace MineSweeper.Domain.Fields;

internal class FieldFabric : IFieldFabric
{
    public IInternalField Create(int x, int y)
    {
        return new Field(y, x);
    }
}
