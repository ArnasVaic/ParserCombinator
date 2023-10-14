using System.Runtime.InteropServices;
using Xunit;
using static ParserCombinator.Core.CommonParsers;
using static UnitTests.TestHelpers;
using static System.Array;

namespace UnitTests.CharacterParsers;

public class SomeTests
{
    [Fact]
    public void NonEmptyInput_AlwaysFailingInnerParser() => SuccessTest(
        Some(Satisfy<char>(_ => false)),
        "some lexer has to always succeed and the result will be an empty array".ToArray(),
        Empty<char>(),
        0);
    
    [Theory]
    [InlineData("")]
    [InlineData("a?")]
    [InlineData("aaaAAA")]
    [InlineData("aaaaaabbbbbb")]
    public void NonEmptyInput(string input) => SuccessTest(
        Some(Satisfy<char>('a'.Equals)),
        input.ToArray(),
        r =>
        {
            var expected = input.TakeWhile('a'.Equals).ToList();
            Assert.Equal(expected, r.Result);
            Assert.Equal(expected.Count, r.Remaining.Offset);
        });
}