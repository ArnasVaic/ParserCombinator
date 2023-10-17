using Xunit;
using static VArnas.ParserCombinator.CommonParsers;
using static VArnas.UnitTests.TestHelpers;
using static System.Array;

namespace VArnas.UnitTests.CharacterParsers;

public class OneOfTests
{
    [Fact]
    public void EmptyInput() => 
        FailTest(OneOf(new [] {Any<char>(), Any<char>() }), Empty<char>());
    
    [Theory]
    [InlineData("a")]
    [InlineData("b")]
    [InlineData("c")]
    public void ValidInput(string input) => SuccessTest(
        OneOf(new [] {Symbol('a'),Symbol('b'), Symbol('c')}),
        input.ToArray(),
        input[0],
        1);

    [Theory]
    [InlineData("1")]
    [InlineData("2")]
    [InlineData("3")]
    [InlineData("?")]
    public void InvalidInput(string input) => FailTest(
        OneOf(new [] {Symbol('a'),Symbol('b'), Symbol('c')}),
        input.ToArray());
}