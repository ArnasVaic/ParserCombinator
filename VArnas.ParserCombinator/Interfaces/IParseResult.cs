namespace VArnas.ParserCombinator.Interfaces;

public interface IParseResult<out TSymbol, out TValue>
{
    TValue Result { get; }

    IParserInput<TSymbol> Remaining { get; }

    IParseResult<TSymbol, TOther> Map<TOther>(Func<TValue, TOther> map);
}