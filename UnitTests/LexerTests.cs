using ParserCombinator.Exceptions;
using ParserCombinator.Lexers;
using Xunit;

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
    public void Satisfy_ExpectedCharacter(string input)
    {
        var reader = new StringReader(input);
        Lexers
            .Satisfy(char.IsDigit, reader)
            .Match(
                l => throw new LexerException(l),
                r => Assert.Equal(input, $"{r.Result}"));
    }
    
    [Fact]
    public void Satisfy_UnexpectedCharacter()
    {
        var reader = new StringReader("abc");
        Lexers
            .Satisfy(char.IsDigit, reader)
            .Match(
                _ => { },
                r => throw new Exception("Lexer should have failed."));
    }
    
    [Theory]
    [InlineData("A")]
    [InlineData("1")]
    [InlineData("@")]
    public void Is_ExpectedCharacter(string input)
    {
        var reader = new StringReader(input);
        Lexers
            .Is(input[0], reader)
            .Match(
                l => throw new LexerException(l),
                r => Assert.Equal(input, $"{r.Result}"));
    }
}