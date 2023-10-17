using Xunit;
using static VArnas.ParserCombinator.CommonParsers;
using static VArnas.ParserCombinator.Parser;
using static VArnas.UnitTests.TestHelpers;

namespace VArnas.UnitTests.CharacterParsers;

public class OrTests
{
    [Theory]
    [InlineData("abc")]
    [InlineData("def")]
    public void FirstSucceeds(string input) => SuccessTest(
        Or(Satisfy<char>(char.IsLetter), Zero<char, char>("Always failing")),
        input.ToArray(),
        input[0],
        1);
    
    [Theory]
    [InlineData("123")]
    [InlineData("456")]
    public void SecondSucceeds(string input) => SuccessTest(
        Or(Zero<char, char>("Always failing"), Satisfy<char>(char.IsDigit)),
        input.ToArray(),
        input[0],
        1);

    [Theory]
    [InlineData("\t\t\t")]
    [InlineData("\n\n\n")]
    public void BothFail(string input) => FailTest(
        Or(Satisfy<char>(char.IsLetter), Satisfy<char>(char.IsDigit)),
        input.ToArray());
}