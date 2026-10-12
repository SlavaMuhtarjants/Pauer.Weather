using FluentAssertions;

using Pauer.Weather.Domain.ValueObjects;

using Xunit;

namespace Pauer.Weather.Tests.UnitTests.Domain;

public sealed class CitySearchTermTests
{
    [Fact]
    public void Create_SearchTermIs3Chars_ReturnsFailure()
    {
        var result = CitySearchTerm.Create("mow");

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("longer than 3 characters");
    }

    [Fact]
    public void Create_SearchTermIs4Chars_ReturnsSuccess()
    {
        var result = CitySearchTerm.Create("mows");

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be("mows");
    }

    [Fact]
    public void Create_InputWithLeadingTrailingWhitespace_Trims()
    {
        var result = CitySearchTerm.Create("  mows  ");

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be("mows");
    }

    [Fact]
    public void Create_TrimmedInputIs3Chars_ReturnsFailure()
    {
        var result = CitySearchTerm.Create("  mow  ");

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("longer than 3 characters");
    }

    [Fact]
    public void Create_NullInput_ReturnsFailure()
    {
        var result = CitySearchTerm.Create(null);

        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void Create_EmptyInput_ReturnsFailure()
    {
        var result = CitySearchTerm.Create(string.Empty);

        result.IsSuccess.Should().BeFalse();
    }
}