using FluentAssertions;
using MineSweeper.Application.Match;
using MineSweeper.Application.Matchs;
using MineSweeper.Domain.Boards;
using MineSweeper.Domain.Fields;
using MineSweeper.Domain.Randoms;
using NSubstitute;

namespace MineSweeper.Application.IntegratedTest.Matchs;

public class DefaultMatchTest
{
    private readonly IMatch _match;
    private readonly IRandom _random;

    public DefaultMatchTest()
    {
        _random = Substitute.For<IRandom>();
        _random.GetRandom(Arg.Any<int>(), Arg.Any<int>()).Returns(2);
        var boardFabric = new BoardFabric(_random);
        _match = new DefaultMatch(3, 3, boardFabric);
    }


    [Fact]
    public void Mark_WhenFieldExist_ShouldFlag()
    {
        // Arrange
        var line = 2;
        var column = 2;
        var flag = FieldFlag.Bomb;

        // Act
        _match.Mark(line, column, flag);

        // Assert
        _match.RemainingBombs.Should().Be(0);
        _match.RemainingFields.Should().Be(8);
    }

    [Fact]
    public void Reveal_WhenFieldHasNoBomb_ShouldRevealAllAndWon()
    {
        // Arrange
        var line = 0;
        var column = 0;

        // Act
        _match.Reveal(line, column);

        // Assert
        _match.RemainingBombs.Should().Be(1);
        _match.RemainingFields.Should().Be(0);
        _match.State.Should().Be(MatchStateEnum.Finished);
        _match.Result.Should().Be(MatchResultEnum.Won);
    }

    [Fact]
    public void Reveal_WhenFieldHasNoBomb_ShouldRevealOneAndKeepRunning()
    {
        // Arrange
        var line = 2;
        var column = 1;

        // Act
        _match.Reveal(line, column);

        // Assert
        _match.RemainingBombs.Should().Be(1);
        _match.RemainingFields.Should().Be(7);
        _match.State.Should().Be(MatchStateEnum.Running);
        _match.Result.Should().Be(MatchResultEnum.None);

        // Arrange
        line = 0;
        column = 0;

        // Act
        _match.Reveal(line, column);

        // Assert
        _match.RemainingBombs.Should().Be(1);
        _match.RemainingFields.Should().Be(0);
        _match.State.Should().Be(MatchStateEnum.Finished);
        _match.Result.Should().Be(MatchResultEnum.Won);
    }

    [Fact]
    public void Reveal_WhenFieldBomb_ShouldRevealOneAndLose()
    {
        // Arrange
        var line = 2;
        var column = 2;

        // Act
        _match.Reveal(line, column);

        // Assert
        _match.RemainingBombs.Should().Be(1);
        _match.RemainingFields.Should().Be(8);
        _match.State.Should().Be(MatchStateEnum.Finished);
        _match.Result.Should().Be(MatchResultEnum.Loss);
    }
}
