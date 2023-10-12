using Xunit;
using static ParserCombinator.Lexers.CommonLexers;

namespace UnitTests;

public class OrTests
{
    [Fact]
    public void EmptyInput_Failure()
    {
        AnyChar.Or(AnyChar)
            .Lex(string.Empty)
            .Match(
                l => { },
                _ => throw new Exception("Lexer was supposed to fail"));
    }
    
    [Theory]
    [InlineData("abc")]
    [InlineData("def")]
    public void FirstSucceeds_Success(string input)
    {
        Letter.Or(Zero<char>())
            .Lex(input)
            .Match(
                error => throw new Exception(error),
                r => Assert.Equal(input[0], r.Result));
    }
    
    [Theory]
    [InlineData("123")]
    [InlineData("456")]
    public void SecondSucceeds_Success(string input)
    {
        Letter.Or(Digit)
            .Lex(input)
            .Match(
                error => throw new Exception(error),
                r => Assert.Equal(input[0], r.Result));
    }
    
    [Theory]
    [InlineData("\t\t\t")]
    [InlineData("\n\n\n")]
    public void BothFail_Failure(string input)
    {
        Letter.Or(Digit)
            .Lex(input)
            .Match(
                _ => { },
                _ => throw new Exception("Lexer was supposed to fail"));
    }
}