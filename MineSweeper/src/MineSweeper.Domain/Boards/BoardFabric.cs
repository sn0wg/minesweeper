using MineSweeper.Domain.Fields;
using MineSweeper.Domain.Randoms;

namespace MineSweeper.Domain.Boards;

public class BoardFabric : IBoardFabric
{
    private readonly IFieldFabric _fieldFabric;
    private readonly IRandom _random;

    public BoardFabric()
    {
        _fieldFabric = new FieldFabric();
        _random = new DefaultRandom();
    }

    public BoardFabric(IRandom random)
    {
        _fieldFabric = new FieldFabric();
        _random = random;
    }
    public IBoard Create(int lines, int columns, int bombQty)
    {
        return new Board(lines, columns, bombQty, _fieldFabric, _random);
    }
}
