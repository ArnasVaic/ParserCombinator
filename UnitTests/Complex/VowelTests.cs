using ParserCombinator.Core;
using Xunit;
using static ParserCombinator.Core.CommonParsers;
using static UnitTests.TestHelpers;

namespace UnitTests.Complex;

public class VowelTests
{
    private static Parser<char, char> Vowel =
            Symbol('a')
        .Or(Symbol('e'))
        .Or(Symbol('i'))
        .Or(Symbol('o'))
        .Or(Symbol('u'));

    [Theory]
    [InlineData("a...")] 
    [InlineData("e...")]
    [InlineData("i...")] 
    [InlineData("o...")]
    [InlineData("u...")] 
    public void ValidInput(string input) => 
        SuccessTest(Vowel, input.ToArray(), input[0], 1);
    
    [Theory]
    [InlineData("b...")] 
    [InlineData("v...")]
    [InlineData("j...")] 
    [InlineData("k...")]
    [InlineData("s...")] 
    public void InvalidInput(string input) => 
        FailTest(Vowel, input.ToArray());
}