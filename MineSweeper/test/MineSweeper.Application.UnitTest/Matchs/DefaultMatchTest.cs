using FluentAssertions;
using FluentAssertions.Equivalency;
using MineSweeper.Application.Match;
using MineSweeper.Application.Matchs;
using MineSweeper.Domain.Boards;
using MineSweeper.Domain.Fields;
using NSubstitute;

namespace MineSweeper.Application.UnitTest.Matchs;

public class DefaultMatchTest
{
    private readonly IBoard _board = Substitute.For<IBoard>();
    private readonly IBoardFabric _boardFabric = Substitute.For<IBoardFabric>();
    private readonly List<List<IField>> _fields;
    private readonly IField _field;
    
    public DefaultMatchTest()
    {
        _field = Substitute.For<IField>();
        var fieldLine = new List<IField>() { _field };
        _fields = [fieldLine];
        _boardFabric.Create(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns(_board);
        _board.Fields.Returns((IReadOnlyCollection<IReadOnlyCollection<IField>>)_fields);
    }

    [Theory]
    [InlineData(MatchDifficultyEnum.Easy, 5, 5)]
    [InlineData(MatchDifficultyEnum.Normal, 10, 10)]
    [InlineData(MatchDifficultyEnum.Hard, 20, 20)]
    public void Constructor_WhenPassParameters_ShouldCreate(MatchDifficultyEnum matchDifficultyEnum, int x, int y)
    {
        // Arrange
        var expectedBombs = (int)(x * y * 0.1);
        var match = new DefaultMatch(matchDifficultyEnum, _boardFabric);

        // Act
        // N/A

        // Assert
        match.RemainingBombs.Should().Be(expectedBombs);
        match.RemainingFields.Should().Be(x * y - expectedBombs);
        _boardFabric.Received().Create(x, y, expectedBombs);
    }

    [Fact]
    public void Reveal_WhenCellExistAndIsNotABomb_ShouldReveal()
    {
        // Arrange
        var match = new DefaultMatch(MatchDifficultyEnum.Easy, _boardFabric);
        _field.State.Returns(FieldState.Revealed);
        _field.Reveal().Returns(23);

        // Act
        match.Reveal(0, 0);

        // Assert
        _field.Received().Reveal();
        match.Result.Should().Be(MatchResultEnum.Won);
        match.State.Should().Be(MatchStateEnum.Finished);
        match.RemainingFields.Should().Be(0);
    }

    [Fact]
    public void Reveal_WhenHasAnExplosion_ShouldReveal()
    {
        // Arrange
        var match = new DefaultMatch(MatchDifficultyEnum.Easy, _boardFabric);
        _field.State.Returns(FieldState.Exploded);
        _field.Reveal().Returns(1);

        // Act
        match.Reveal(0, 0);

        // Assert
        _field.Received().Reveal();
        match.Result.Should().Be(MatchResultEnum.Loss);
        match.State.Should().Be(MatchStateEnum.Finished);
        match.RemainingFields.Should().Be(23);
    }

    [Fact]
    public void Reveal_WhenCellNotExist_ShouldNotReveal()
    {
        // Arrange
        var match = new DefaultMatch(MatchDifficultyEnum.Easy, _boardFabric);

        // Act
        match.Reveal(1, 0);

        // Assert
        _field.DidNotReceive().Reveal();
    }

    [Fact]
    public void Mark_WhenCellExist_ShouldReveal()
    {
        // Arrange
        var match = new DefaultMatch(MatchDifficultyEnum.Easy, _boardFabric);

        // Act
        match.Mark(0, 0, FieldFlag.Bomb);

        // Assert
        _field.ReceivedCalls().Should().NotBeEmpty();
        match.RemainingBombs.Should().Be(1);

        // Act
        match.Mark(0, 0, FieldFlag.None);

        // Assert
        match.RemainingBombs.Should().Be(2);
    }

    [Fact]
    public void Mark_WhenCellNotExist_ShouldNotReveal()
    {
        // Arrange
        var match = new DefaultMatch(MatchDifficultyEnum.Easy, _boardFabric);

        // Act
        match.Mark(1, 0, FieldFlag.Bomb);

        // Assert
        _field.ReceivedCalls().Should().BeNullOrEmpty();
        match.RemainingBombs.Should().Be(2);
    }

    [Fact]
    public void RevealAll_WhenIsHidden_ShouldReveal()
    {
        // Arrange
        var match = new DefaultMatch(MatchDifficultyEnum.Easy, _boardFabric);

        // Act
        match.RevealAll();

        // Assert
        _field.Received().Reveal();
    }
}
