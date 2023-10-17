using System;

namespace VArnas.ParserCombinator;

public class ParseResult<TSymbol, TResult>(TResult result, ParserInput<TSymbol> remaining)
{
    public TResult Result { get; } = result;

    public ParserInput<TSymbol> Remaining { get; } = remaining;

    public ParseResult<TSymbol, TOther> Map<TOther>(Func<TResult, TOther> func) => 
        new(func(Result), Remaining);

    public void Map(Action<TResult> func) => func(Result);

    public ParseResult<TSymbol, TResult> Map(Func<TResult, TResult> func)
    {
        func(Result);
        return this;
    }
}