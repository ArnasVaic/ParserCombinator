using Xunit;
using static ParserCombinator.Lexers.CommonLexers;

namespace UnitTests;

public class SeqTests
{
    [Fact]
    public void EmptyInput_Failure()
    {
        Seq(AnyChar, AnyChar)
            .Lex(string.Empty)
            .Match(
                l => { },
                _ => throw new Exception("Lexer was supposed to fail"));
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("def")]
    [InlineData("???")]
    public void GoodInput_Success(string input)
    {
        Seq(AnyChar, AnyChar, AnyChar)
            .Lex(input)
            .Match(
                error => throw new Exception(error),
                r => Assert.Equal(input.AsEnumerable(), r.Result));
    }
}