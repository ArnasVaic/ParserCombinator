using Xunit;
using static UnitTests.LexerTestHelpers;
using static ParserCombinator.Lexers.CommonLexers;

namespace UnitTests;

public class StringTests
{
    [Fact]
    public void EmptyInputFail() => FailTest(
        String("this is some string"),
        string.Empty,
        _ => { });

    [Theory]
    [InlineData("prefix")]
    [InlineData("/.:?, punctuations!")]
    [InlineData("")]
    public void InputIsPrefixOfExpectedString(string input) => FailTest(
        String(string.Join(input, "something else")),
        input,
        l => { });
}