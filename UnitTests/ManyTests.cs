using ParserCombinator.Lexers;
using Xunit;
using static UnitTests.LexerTestHelpers;
using static ParserCombinator.Lexers.CommonLexers;

namespace UnitTests;

public class ManyTests
{
    [Fact]
    public void EmptyInputFailingLexer_Failure() => FailTest(
        Some(Satisfy(_ => false)),
        string.Empty,
        _ => { });
    
    [Fact]
    public void NonEmptyInputFailingLexer_Success() => SuccessTest(
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