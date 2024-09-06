namespace MineSweeper.Domain.Fields;

internal interface IFieldFabric
{
    public IInternalField Create(int x, int y);
}
