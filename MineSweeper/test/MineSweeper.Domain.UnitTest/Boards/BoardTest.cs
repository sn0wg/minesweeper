using FluentAssertions;
using MineSweeper.Domain.Boards;
using MineSweeper.Domain.Fields;
using MineSweeper.Domain.Randoms;
using NSubstitute;
using NSubstitute.ReceivedExtensions;

namespace MineSweeper.Domain.UnitTest.Boards;

public class BoardTest
{
    private readonly IFieldFabric _fieldFabric;
    private readonly IList<IInternalField> _fields;
    private readonly IRandom _random;

    public BoardTest()
    {
        _fieldFabric = Substitute.For<IFieldFabric>();
        _random = Substitute.For<IRandom>();
        _fields = new List<IInternalField>();

        _random.GetRandom(Arg.Any<int>(), Arg.Any<int>()).Returns(x =>
        {
            var min = x.ArgAt<int>(0);
            var max = x.ArgAt<int>(1);

            return new Random().Next(min, max);
        });

        _fieldFabric.Create(Arg.Any<int>(), Arg.Any<int>()).Returns(callInfo =>
        {
            var x = callInfo.ArgAt<int>(0);
            var y = callInfo.ArgAt<int>(1);

            var field = Substitute.For<IInternalField>();
            field.Line.Returns(y);
            field.Column.Returns(x);

            field.HasBomb.Returns(false);
            field.When(f => f.Plant()).Do(_ =>
            {
                field.HasBomb.Returns(true);
            });

            _fields.Add(field);

            return field;
        });
    }

    [Fact]
    public void Constructor_3x3Board_ShouldCreateRight()
    {
        // Arrange
        var lines = 3;
        var columns = 3;
        var bombs = 3;
        var expectedFields = lines * columns;

        // Act
        var board = new Board(lines, columns, bombs, _fieldFabric, _random);

        // Assert
        _fields.Count.Should().Be(expectedFields);

        for (var y = 0; y < lines; y++)
        {
            for (var x = 0; x < columns; x++)
            {
                var field = _fields.SingleOrDefault(field => field.Line == y && field.Column == x);

                field.Should().NotBeNull();

                field?.Received(Quantity.Within(4, 9)).Link(Arg.Any<IInternalField>());
            }
        }
    }
}
