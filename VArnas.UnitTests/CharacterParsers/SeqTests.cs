using Xunit;
using static VArnas.ParserCombinator.CommonParsers;
using static VArnas.UnitTests.TestHelpers;
using static System.Array;

namespace VArnas.UnitTests.CharacterParsers;

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