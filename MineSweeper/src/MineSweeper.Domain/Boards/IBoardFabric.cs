namespace MineSweeper.Domain.Boards;

public interface IBoardFabric
{
    IBoard Create(int columns, int lines, int bombQty);
}
