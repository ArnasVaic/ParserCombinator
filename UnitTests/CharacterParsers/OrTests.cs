using Xunit;
using static ParserCombinator.Core.CommonParsers;
using static ParserCombinator.Core.Parser;
using static UnitTests.TestHelpers;

namespace UnitTests.CharacterParsers;

public class OrTests
{
    [Theory]
    [InlineData("abc")]
    [InlineData("def")]
    public void FirstSucceeds(string input) => SuccessTest(
        Satisfy<char>(char.IsLetter).Or(Zero<char, char>("Always failing")),
        input.ToArray(),
        input[0],
        1);
    
    [Theory]
    [InlineData("123")]
    [InlineData("456")]
    public void SecondSucceeds(string input) => SuccessTest(
        Zero<char, char>("Always failing").Or(Satisfy<char>(char.IsDigit)),
        input.ToArray(),
        input[0],
        1);

    [Theory]
    [InlineData("\t\t\t")]
    [InlineData("\n\n\n")]
    public void BothFail(string input) => FailTest(
        Satisfy<char>(char.IsLetter).Or(Satisfy<char>(char.IsDigit)),
        input.ToArray());
}