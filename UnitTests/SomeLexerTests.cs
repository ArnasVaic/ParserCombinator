using ParserCombinator.Lexers;
using Xunit;
using static UnitTests.LexerTestHelpers;
using static ParserCombinator.Lexers.CommonLexers;

namespace UnitTests;

public class SomeLexerTests
{
    [Fact]
    public void Some_EmptyInputFailingLexer_Success() => SuccessTest(
        Some(Satisfy(_ => false)),
        string.Empty,
        r => Assert.Equal(Array.Empty<char>(), r.Result.ToArray())
        );
    
    [Fact]
    public void Some_NonEmptyInputFailingLexer_Success() => SuccessTest(
        Some(Satisfy(_ => false)),
        "some lexer has to always succeed and the result will be an empty array",
        r => Assert.Equal(Array.Empty<char>(), r.Result.ToArray())
    );
    
    [Theory]
    [InlineData("a")]
    [InlineData("aaa")]
    [InlineData("aaaaaabbbbbb")]
    public void Some_NonEmptyInput_Success(string input) => SuccessTest(
        Some(Char('a')),
        input,
        r =>
        {
            var expected = input.TakeWhile('a'.Equals);
            Assert.Equal(expected, r.Result);
        });
}