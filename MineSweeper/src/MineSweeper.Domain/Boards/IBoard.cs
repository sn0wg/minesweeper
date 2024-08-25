using MineSweeper.Domain.Fields;

namespace MineSweeper.Domain.Boards;

public interface IBoard
{
    public IReadOnlyCollection<IReadOnlyCollection<IField>> Fields { get; }
    public int BombQty { get; }
}
