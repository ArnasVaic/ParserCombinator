using Xunit;
using static ParserCombinator.Core.CommonParsers;
using static UnitTests.TestHelpers;
using static System.Array;

namespace UnitTests.CharacterParsers;

public class SeqTests
{
    [Fact]
    public void EmptyInput() => FailTest(
        Seq(Any<char>(), Any<char>()), 
        Empty<char>());

    [Theory]
    [InlineData("abc")]
    [InlineData("def")]
    [InlineData("???")]
    public void GoodInput(string input) => SuccessTest(
        Seq(Any<char>(), Any<char>(), Any<char>()),
        input.ToArray(),
        input,
        3);
}