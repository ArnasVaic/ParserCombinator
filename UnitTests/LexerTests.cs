using ParserCombinator.Exceptions;
using static ParserCombinator.Lexers.CommonLexers;
using Xunit;


using static UnitTests.LexerTestHelpers;

namespace UnitTests;

public class LexerTests
{
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
    public void Satisfy_ExpectedCharacter(string input) => TestSuccess(
        Satisfy(char.IsDigit), 
        input,
        @char => Assert.Equal(input, $"{@char}"));


    [Fact]
    public void Satisfy_UnexpectedCharacter() => TestFailure(
        Satisfy(char.IsDigit),
        "abc",
        _ => { });

    [Theory]
    [InlineData("A")]
    [InlineData("1")]
    [InlineData("@")]
    public void Is_ExpectedCharacter(string input) => TestSuccess(
        Is(input[0]), 
        input,
        @char => Assert.Equal(input, $"{@char}"));

    [Theory]
    [InlineData("b")]
    [InlineData("0")]
    [InlineData("7")]
    public void Or_Simple(string input) => TestSuccess(
        Or(Satisfy(char.IsDigit), Is('b')), 
        input,
        @char => Assert.Equal(input, $"{@char}"));
}