using Xunit;
using static ParserCombinator.Core.CommonParsers;
using static UnitTests.TestHelpers;
using static System.Array;

namespace UnitTests.CharacterParsers;

public class ManyTests
{
    [Fact]
    public void EmptyInput_InnerParserConsumesCharacter() => 
        FailTest(Many(Any<char>()), Empty<char>());
    
    [Fact]
    public void NonEmptyInput_AnyCharInnerParser() => SuccessTest(
        Many(Satisfy<char>('s'.Equals)),
        "some lexer has to always succeed and the result will be an empty array".ToArray(),
        "s".ToArray(),
        1);
    
    [Theory]
    [InlineData("a")]
    [InlineData("aaa")]
    [InlineData("aaaaaabbbbbb")]
    public void NonEmptyInput(string input) => SuccessTest(
        Many(Satisfy<char>('a'.Equals)),
        input.ToArray(),
        r =>
        {
            var expected = input.TakeWhile('a'.Equals).ToList();
            Assert.Equal(expected, r.Result);
            Assert.Equal(expected.Count, r.Remaining.Offset);
        });
}