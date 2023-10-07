using static ParserCombinator.Lexers.Lexers;
using Xunit;
using Xunit.Abstractions;
using static UnitTests.LexerTestHelpers;

namespace UnitTests;

public class LexerTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public LexerTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Theory]
    [InlineData("0")]
    [InlineData("1")]
    [InlineData("2")]
    [InlineData("3")]
    [InlineData("4")]
    [InlineData("5")]
    [InlineData("6")]
    [InlineData("7")]
    [InlineData("8")]
    [InlineData("9")]
    public void Satisfy_ExpectedCharacter_LexerSucceeds(string input) => TestSuccess(
        Satisfy(char.IsDigit), 
        input,
        r => Assert.Equal(input, $"{r.Result}"));
    
    [Theory]
    [InlineData("A")]
    [InlineData("B")]
    [InlineData("C")]
    public void Satisfy_UnexpectedCharacter_LexerFails(string input) => TestFailure(
        Satisfy(char.IsDigit),
        input,
        _ => { });

    [Theory]
    [InlineData("A")]
    [InlineData("1")]
    [InlineData("@")]
    public void Is_ExpectedCharacter_LexerSucceeds(string input) => TestSuccess(
        Is(input[0]), 
        input,
        r => Assert.Equal(input, $"{r.Result}"));

    [Fact]
    public void Or_FirstLexerSucceeds() => TestSuccess(
        Or(Is('a'), Satisfy(_ => false)), 
        "a",
        r => Assert.Equal("a", $"{r.Result}"));
    
    [Fact]
    public void Or_FirstLexerFails_SecondLexerSucceeds() => TestSuccess(
        Or(Satisfy(_ => false), Is('a')), 
        "a",
        r => Assert.Equal("a", $"{r.Result}"));
    
    [Fact]
    public void Or_BothLexersFail_LexerFails() => TestFailure(
        Or(Satisfy(_ => false), Satisfy(_ => false)), 
        "a",
        r => { });

    [Theory]
    [InlineData("a")]
    [InlineData("e")]
    [InlineData("i")]
    [InlineData("o")]
    [InlineData("u")]
    public void ManyOr_LowercaseVowelParser_Succeeds(string input) => TestSuccess(
        Or(Is('a'), Is('e'), Is('i'), Is('o'), Is('u')), 
        input,
        r => Assert.Equal(input, $"{r.Result}"));
}