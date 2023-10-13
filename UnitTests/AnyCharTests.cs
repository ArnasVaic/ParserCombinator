using Xunit;
using static ParserCombinator.Lexers.CommonLexers;

namespace UnitTests;

public class AnyCharTests
{
    [Fact]
    public void EmptyInput_Failure()
    {
        AnyChar
            .Lex(string.Empty)
            .Match(
                l => { },
                _ => throw new Exception("Lexer was supposed to fail"));
    }
    
    [Theory]
    [InlineData("abc")]
    [InlineData("hello, this is long input")]
    [InlineData("???")]
    [InlineData("\t")]
    public void NonEmptyInput_Success(string input)
    {
        AnyChar
            .Lex(input)
            .Match(
                error => throw new Exception(error),
                r =>
                {
                    Assert.Equal(1, r.Remaining.Offset);
                    Assert.Equal(input[0], r.Result);
                });
    }
}