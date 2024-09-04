using FluentAssertions;
using MineSweeper.Domain.Fields;
using System.Data.Common;

namespace MineSweeper.Domain.UnitTest.Fields;

public class FieldTest
{
    [Fact]
    public void Construct_WhenDataIsOk_ShouldConstruct()
    {
        // Arrange
        ushort line = 0;
        ushort column = 0;

        // Act
        var field = new Field(line, column);

        // Assert
        field.Line.Should().Be(line);
        field.Column.Should().Be(column);
        field.Identifier.Should().Be($"X{column}-Y{line}");
        field.State.Should().Be(FieldState.Hidden);
        field.HasBomb.Should().Be(false);
    }

    [Fact]
    public void Plant_WhenHasBomb_ShouldPlace()
    {
        // Arrange
        var field = new Field(0, 0);

        // Act
        field.Plant();

        // Assert
        field.HasBomb.Should().BeTrue();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Reveal_AfterCall_ShouldChangeState(bool hasBomb)
    {
        // Arrange
        var field = new Field(0, 0);

        if (hasBomb)
            field.Plant();

        // Act
        var reveleadQty = field.Reveal();

        // Assert
        reveleadQty.Should().Be(1);
        var expectedState = hasBomb ? FieldState.Exploded : FieldState.Revealed;
        field.State.Should().Be(expectedState);
    }

    [Fact]
    public void Reveal_WhenHasNoBomb_ShouldRevealAdjacents()
    {
        // Arrange
        var field = new Field(0, 0);
        var adjacentField = new Field(0, 1);
        var adjacentField2 = new Field(0, 2);

        field.Link(adjacentField);
        adjacentField.Link(adjacentField2);

        adjacentField2.Plant();

        // Act
        var reveleadQty = field.Reveal();

        // Assert
        reveleadQty.Should().Be(2);
        field.State.Should().Be(FieldState.Revealed);
        field.AdjacentBombsQty.Should().Be(0);
        adjacentField.AdjacentBombsQty.Should().Be(1);
        adjacentField.State.Should().Be(FieldState.Revealed);
        adjacentField2.State.Should().Be(FieldState.Hidden);
    }

    [Fact]
    public void Reveal_WhenHasBombAdjacent_ShouldNotRevealAdjacents()
    {
        // Arrange
        var field = new Field(0, 0);
        var adjacentField = new Field(0, 1);

        field.Link(adjacentField);
        adjacentField.Plant();

        // Act
        var reveleadQty = field.Reveal();

        // Assert
        reveleadQty.Should().Be(1);
        field.State.Should().Be(FieldState.Revealed);
        field.AdjacentBombsQty.Should().Be(1);
        adjacentField.State.Should().Be(FieldState.Hidden);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, 1)]
    [InlineData(0, 2)]
    [InlineData(1, 0)]
    [InlineData(1, 2)]
    [InlineData(2, 0)]
    [InlineData(2, 1)]
    [InlineData(2, 2)]
    public void Link_WhenIsAdjacent_ShouldLink(ushort y, ushort x)
    {
        // Arrange
        var field = new Field(1, 1);
        var fieldToLink = new Field(y, x);

        // Act
        var linked = field.Link(fieldToLink);

        // Assert
        linked.Should().BeTrue();
    }

    [Theory]
    [InlineData(0, 3)]
    [InlineData(1, 3)]
    [InlineData(3, 0)]
    [InlineData(3, 1)]
    [InlineData(3, 2)]
    public void Link_WhenIsNotAdjacent_ShouldNotLink(ushort y, ushort x)
    {
        // Arrange
        var field = new Field(1, 1);
        var fieldToLink = new Field(y, x);

        // Act
        var linked = field.Link(fieldToLink);

        // Assert
        linked.Should().BeFalse();
    }

    [Fact]
    public void Link_WhenIsTheSame_ShouldNotLink()
    {
        // Arrange
        var field = new Field(1, 1);

        // Act
        var linked = field.Link(field);

        // Assert
        linked.Should().BeFalse();
    }

    [Fact]
    public void Link_WhenHasTheSamePoint_ShouldNotLink()
    {
        // Arrange
        var field = new Field(1, 1);
        var fieldToLink = new Field(1, 1);

        // Act
        var linked = field.Link(fieldToLink);

        // Assert
        linked.Should().BeFalse();
    }

    [Fact]
    public void Link_LinkingTwice_ShouldNotLink()
    {
        // Arrange
        var field = new Field(1, 1);
        var fieldToLink = new Field(1, 2);

        // Act
        var linked = field.Link(fieldToLink);
        var linked2 = field.Link(fieldToLink);

        // Assert
        linked.Should().BeTrue();
        linked2.Should().BeFalse();
    }
}
