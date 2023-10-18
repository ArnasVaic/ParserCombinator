namespace VArnas.ParserCombinator;

public interface IParser<TSymbol, out TResult>
{
    IEither<string, IParseResult<TSymbol, TResult>> Parse(IParserInput<TSymbol> input);

    IParser<TSymbol, TOther> Map<TOther>(Func<TResult, TOther> func);

    IParser<TSymbol, TOther> Map<TOther>(TOther func);
    
    IParser<TSymbol, TNew> Bind<TNew>(Func<TResult, IParser<TSymbol, TNew>> func);
}