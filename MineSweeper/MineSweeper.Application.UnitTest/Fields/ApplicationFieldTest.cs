using FluentAssertions;
using MineSweeper.Application.Fields;
using MineSweeper.Domain.Fields;
using NSubstitute;

namespace MineSweeper.Application.UnitTest.Fields;

public class ApplicationFieldTest
{
    private readonly IField _field;
    private readonly IApplicationField _applicationField;

    public ApplicationFieldTest()
    {
        _field = Substitute.For<IField>();
        _applicationField = new ApplicationField(_field);
    }

    [Fact]
    public void Identifier_WhenHas_ShouldReturn()
    {
        // Arrange
        var identifier = "123";
        _field.Identifier.Returns(identifier);

        // Act
        var returned = _applicationField.Identifier;

        // Assert
        returned.Should().Be(identifier);
    }

    [Fact]
    public void State_WhenHas_ShouldReturn()
    {
        // Arrange
        var state = FieldState.Revealed;
        _field.State.Returns(state);

        // Act
        var returned = _applicationField.State;

        // Assert
        returned.Should().Be(state);
    }

    [Fact]
    public void Flag_WhenHas_ShouldReturn()
    {
        // Arrange
        var flag = FieldFlag.Bomb;
        _field.Flag.Returns(flag);

        // Act
        var returned = _applicationField.Flag;

        // Assert
        returned.Should().Be(flag);
    }

    [Fact]
    public void Line_WhenHas_ShouldReturn()
    {
        // Arrange
        var line = 1;
        _field.Line.Returns(line);

        // Act
        var returned = _applicationField.Line;

        // Assert
        returned.Should().Be(line);
    }

    [Fact]
    public void Column_WhenHas_ShouldReturn()
    {
        // Arrange
        var column = 2;
        _field.Column.Returns(column);

        // Act
        var returned = _applicationField.Column;

        // Assert
        returned.Should().Be(column);
    }
    
    [Fact]
    public void AdjacentBombsQty_WhenHas_ShouldReturn()
    {
        // Arrange
        var bombs = 5;
        _field.AdjacentBombsQty.Returns(bombs);

        // Act
        var returned = _applicationField.AdjacentBombsQty;

        // Assert
        returned.Should().Be(bombs);
    }
}
