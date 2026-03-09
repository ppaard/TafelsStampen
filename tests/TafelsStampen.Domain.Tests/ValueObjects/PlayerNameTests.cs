using Shouldly;

namespace TafelsStampen.Domain.Tests.ValueObjects;
using TafelsStampen.Domain.Exceptions;
using TafelsStampen.Domain.ValueObjects;

public class PlayerNameTests
{
    [Fact]
    public void Create_WithValidName_Succeeds()
    {
        var name = new PlayerName("Jan");
        name.Value.ShouldBe("Jan");
    }

    [Fact]
    public void Create_TrimsWhitespace()
    {
        var name = new PlayerName("  Jan  ");
        name.Value.ShouldBe("Jan");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithEmptyName_ThrowsDomainException(string value)
    {
        Should.Throw<DomainException>(() => new PlayerName(value));
    }

    [Fact]
    public void Create_WithNameOver30Chars_ThrowsDomainException()
    {
        var longName = new string('a', 31);
        Should.Throw<DomainException>(() => new PlayerName(longName));
    }

    [Fact]
    public void Create_WithExactly30Chars_Succeeds()
    {
        var name = new PlayerName(new string('a', 30));
        name.Value.Length.ShouldBe(30);
    }
}
