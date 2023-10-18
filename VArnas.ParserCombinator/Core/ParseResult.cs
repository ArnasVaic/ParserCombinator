
namespace VArnas.ParserCombinator;

public class ParseResult<TSymbol, TResult>(
    TResult result, 
    IParserInput<TSymbol> remaining) 
    : IParseResult<TSymbol, TResult>
{
    public TResult Result { get; } = result;

    public IParserInput<TSymbol> Remaining { get; } = remaining;

    public IParseResult<TSymbol, TOther> Map<TOther>(Func<TResult, TOther> func) => 
        new ParseResult<TSymbol, TOther>(func(Result), Remaining);

    public void Map(Action<TResult> func) => func(Result);

    public IParseResult<TSymbol, TResult> Map(Func<TResult, TResult> func)
    {
        func(Result);
        return this;
    }
}