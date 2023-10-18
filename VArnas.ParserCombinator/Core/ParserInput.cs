using VArnas.ParserCombinator.Interfaces;

namespace VArnas.ParserCombinator.Core;

/// <summary>
/// Parser input reads from an immutable source of symbols and tracks the reading position.
/// </summary>
/// <param name="data">Source of symbols to read from</param>
/// <param name="offset">Index of a symbol where the parser will begin reading</param>
/// <typeparam name="TSymbol">Type of symbol</typeparam>
public class ParserInput<TSymbol>(
    IReadOnlyCollection<TSymbol> data, 
    int offset) 
    : IParserInput<TSymbol>
{
    public IReadOnlyCollection<TSymbol> Data { get; } = data;
    public int Offset { get; set; } = offset;
}