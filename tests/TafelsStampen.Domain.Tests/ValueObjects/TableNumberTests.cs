using Shouldly;

namespace TafelsStampen.Domain.Tests.ValueObjects;
using TafelsStampen.Domain.Exceptions;
using TafelsStampen.Domain.ValueObjects;

public class TableNumberTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    public void Create_WithValidNumber_Succeeds(int value)
    {
        var table = new TableNumber(value);
        table.Value.ShouldBe(value);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(11)]
    [InlineData(-1)]
    public void Create_WithInvalidNumber_ThrowsDomainException(int value)
    {
        Should.Throw<DomainException>(() => new TableNumber(value));
    }
}
