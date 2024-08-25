using FluentAssertions;
using MineSweeper.Domain.Boards;
using MineSweeper.Domain.Fields;
using MineSweeper.Domain.Randoms;
using NSubstitute;
using System.Linq;

namespace MineSweeper.Domain.IntegratedTest.Boards;

public class BoardTest
{
    private readonly IBoardFabric _boardFabric;
    private readonly IRandom _random;

    public BoardTest()
    {
        _random = Substitute.For<IRandom>();
        _boardFabric = new BoardFabric(_random);
    }

    [Fact]
    public void Reveal_BoardWithNoBomb_ShouldRevealAll()
    {
        // Arrange
        var bombQty = 0;
        var lines = 3;
        var columns = 3;

        // Act
        var board = _boardFabric.Create(columns, lines, bombQty);

        board.Fields.First().First().Reveal();

        // Assert
        board.BombQty.Should().Be(bombQty);

        foreach (var line in board.Fields)
        {
            foreach (var column in line)
            {
                column.State.Should().Be(FieldState.Revealed);
                column.AdjacentBombsQty.Should().Be(bombQty);
            }
        }
    }

    [Fact]
    public void Reveal_BoardWithBomb_ShouldNotRevealAll()
    {
        // Arrange
        var bombQty = 1;
        var lines = 3;
        var columns = 3;
        _random.GetRandom(Arg.Any<int>(), Arg.Any<int>()).Returns(2);

        // Act
        var board = _boardFabric.Create(columns, lines, bombQty);
        board.Fields.First().First().Reveal();

        // Assert
        board.BombQty.Should().Be(bombQty);
        var bombField = board.Fields.ElementAt(2).ElementAt(2);

        foreach (var line in board.Fields)
        {
            foreach (var column in line)
            {
                if (column == bombField)
                {
                    column.State.Should().Be(FieldState.Hidden);
                    continue;
                }

                column.State.Should().Be(FieldState.Revealed);
                if ((column.Line == 1 && (column.Column == 1 || column.Column == 2)) || 
                    (column.Line == 2 && column.Column == 1))
                {
                    column.AdjacentBombsQty.Should().Be(bombQty);
                } else
                {
                    column.AdjacentBombsQty.Should().Be(0);
                }                
            }
        }
    }

    [Fact]
    public void Reveal_BoardWithBombAsyde_ShouldNotRevealAll()
    {
        // Arrange
        var bombQty = 1;
        var lines = 3;
        var columns = 3;
        _random.GetRandom(Arg.Any<int>(), Arg.Any<int>()).Returns(1);

        // Act
        var board = _boardFabric.Create(columns, lines, bombQty);
        var firstField = board.Fields.First().First();
        firstField.Reveal();

        // Assert
        board.BombQty.Should().Be(bombQty);

        foreach (var line in board.Fields)
        {
            foreach (var column in line)
            {
                if (column == firstField)
                {
                    column.State.Should().Be(FieldState.Revealed);
                    column.AdjacentBombsQty.Should().Be(1);
                    continue;
                }

                column.State.Should().Be(FieldState.Hidden);
            }
        }
    }

    [Fact]
    public void Reveal_WhenRevealBomb_ShouldExplode()
    {
        // Arrange
        var bombQty = 1;
        var lines = 3;
        var columns = 3;
        _random.GetRandom(Arg.Any<int>(), Arg.Any<int>()).Returns(2);

        // Act
        var board = _boardFabric.Create(columns, lines, bombQty);
        var bombField = board.Fields.ElementAt(2).ElementAt(2);
        bombField.Reveal();

        // Assert
        board.BombQty.Should().Be(bombQty);

        foreach (var line in board.Fields)
        {
            foreach (var column in line)
            {
                if (column == bombField)
                {
                    column.State.Should().Be(FieldState.Exploded);
                    continue;
                }

                column.State.Should().Be(FieldState.Hidden);
            }
        }
    }
}
