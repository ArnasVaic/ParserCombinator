using ParserCombinator.Lexers;
using Xunit;
using static UnitTests.LexerTestHelpers;
using static ParserCombinator.Lexers.Lexers;


namespace UnitTests;

public class NumberLexerTests
{
    [Fact]
    public void Number_EmptyInput_Fails() => TestFailure(
        Number,
        string.Empty,
        _ => { });
    
    [Theory]
    [InlineData("1")]
    [InlineData("0123")]
    [InlineData("1231235")]
    [InlineData("1231235not numbers anymore")]
    public void Number_NonEmptyInput_Success(string input) => TestSuccess(
        Number,
        input,
        r =>
        {
            var expected = input.TakeWhile(char.IsDigit);
            Assert.Equal(expected, r.Result);
        });
}