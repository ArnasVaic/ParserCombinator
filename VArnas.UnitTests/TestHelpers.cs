using VArnas.ParserCombinator;
using Xunit;

namespace VArnas.UnitTests;

public static class TestHelpers
{
    public static void FailTest<TSymbol, TResult>(
        Parser<TSymbol, TResult> parser,
        IReadOnlyCollection<TSymbol> input,
        Action<string> test) => parser
        .Parse(new (input, 0))
        .Match(test, _ => throw new Exception("Parser was supposed to fail."));

    public static void FailTest<TSymbol, TResult>(
        Parser<TSymbol, TResult> parser,
        IReadOnlyCollection<TSymbol> input) => 
        FailTest(parser, input, _ => { });
    
    private static void Throw(string s) => throw new Exception(s);
    
    public static void SuccessTest<TSymbol, TResult>(
        Parser<TSymbol, TResult> parser,
        IReadOnlyCollection<TSymbol> input,
        Action<ParseResult<TSymbol, TResult>> test) => 
        parser.Parse(new (input, 0)).Match(Throw, test);

    public static void SuccessTest<TSymbol, TResult>(
        Parser<TSymbol, TResult> parser,
        IReadOnlyCollection<TSymbol> input,
        TResult expectedResult,
        int expectedOffset) => SuccessTest(parser, input, parseResult =>
    {
        Assert.Equal(expectedResult, parseResult.Result);
        Assert.Equal(expectedOffset, parseResult.Remaining.Offset);
    });
}