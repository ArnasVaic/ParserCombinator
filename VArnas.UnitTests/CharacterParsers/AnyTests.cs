using VArnas.ParserCombinator;
using Xunit;
using static VArnas.ParserCombinator.CommonParsers;
using static VArnas.UnitTests.TestHelpers;
using static System.Array;

namespace VArnas.UnitTests.CharacterParsers;

public class AnyTests
{
    [Fact]
    public void EmptyInput() => FailTest(Any<char>(), Empty<char>());

    [Theory]
    [InlineData("abc")]
    [InlineData("hello, this is long input")]
    [InlineData("???")]
    [InlineData("\t")]
    public void NonEmptyInput(string input) => SuccessTest(
        Any<char>(),
        input.ToArray(),
        input[0], 
        1);

   
    [Fact]
    public void Test()
    {
        
    }
}