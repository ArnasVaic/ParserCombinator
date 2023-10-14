using ParserCombinator.Core;
using Xunit;
using static ParserCombinator.Core.CommonParsers;
using static ParserCombinator.Core.Parser;
using static UnitTests.TestHelpers;
using static System.Array;

namespace UnitTests.CharacterParsers;

public class OneOfTests
{
    [Fact]
    public void EmptyInput() => 
        FailTest(OneOf(Any<char>()), Empty<char>());
    
    [Theory]
    [InlineData("a")]
    [InlineData("b")]
    [InlineData("c")]
    public void ValidInput(string input) => SuccessTest(
        OneOf(Symbol('a'),Symbol('b'), Symbol('c')),
        input.ToArray(),
        input[0],
        1);

    [Theory]
    [InlineData("1")]
    [InlineData("2")]
    [InlineData("3")]
    [InlineData("?")]
    public void InvalidInput(string input) => FailTest(
        OneOf(Symbol('a'),Symbol('b'), Symbol('c')),
        input.ToArray());
}