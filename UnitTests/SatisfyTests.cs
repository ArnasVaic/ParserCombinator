using Xunit;
using static ParserCombinator.Lexers.CommonLexers;

namespace UnitTests;

public class SatisfyTests
{
    [Fact]
    public void EmptyInput_Failure()
    {
        Satisfy(char.IsDigit)
            .Lex(string.Empty)
            .Match(
                l => {},
                _ => throw new Exception("Lexer was supposed to fail"));
    }
    
    [Theory]
    [InlineData("a")]
    [InlineData("b")]
    [InlineData("c")]
    [InlineData("d")]
    [InlineData("e")]
    [InlineData("x")]
    [InlineData("y")]
    [InlineData("z")]
    public void InputIsLetter_Failure(string input)
    {
        Satisfy(char.IsDigit)
            .Lex(input)
            .Match(
                _ => {},
                _ => throw new Exception("Lexer was supposed to fail"));
    }
    
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
    public void InputIsDigit_Success(string input)
    {
        Satisfy(char.IsDigit)
            .Lex(input)
            .Match(
                error => throw new Exception(error),
                r => Assert.Equal(input[0], r.Result));
    }
}