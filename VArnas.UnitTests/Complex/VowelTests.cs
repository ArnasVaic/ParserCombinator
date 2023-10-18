namespace VArnas.UnitTests.Complex;

public class VowelTests
{
    private static readonly IParser<char, char> Vowel = OneOf("aeiou");

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