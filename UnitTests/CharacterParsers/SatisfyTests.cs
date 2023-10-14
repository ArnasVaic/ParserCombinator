using Xunit;
using static ParserCombinator.Core.CommonParsers;
using static UnitTests.TestHelpers;
using static System.Array;

namespace UnitTests.CharacterParsers;

public class SatisfyTests
{
    [Fact]
    public void EmptyInput() => 
        FailTest(Satisfy<char>(_ => true), Empty<char>());

    [Theory]
    [InlineData("a")]
    [InlineData("b")]
    [InlineData("c")]
    [InlineData("d")]
    [InlineData("e")]
    [InlineData("x")]
    [InlineData("y")]
    [InlineData("z")]
    public void InputIsLetter(string input) => 
        FailTest(Satisfy<char>(char.IsDigit), input.ToArray());
    
    [Theory]
    [InlineData("0")]
    [InlineData("1")]
    [InlineData("2")]
    [InlineData("3")]
    [InlineData("4")]
    [InlineData("5")]
    [InlineData("6")]
    [InlineData("7")]
    [InlineData("8")]
    [InlineData("9")]
    public void InputIsDigit(string input) => SuccessTest(
        Satisfy<char>(char.IsDigit),
        input.ToArray(), 
        input[0], 
        1);
}