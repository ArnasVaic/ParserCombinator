namespace ParserCombinator.Core;

/// <summary>
/// Parser input reads from an immutable source of symbols and tracks the reading position.
/// </summary>
/// <param name="data">Source of symbols to read from</param>
/// <param name="offset">Index of a symbol where the parser will begin reading</param>
/// <typeparam name="TSymbol">Type of symbol</typeparam>
public record ParserInput<TSymbol>(IReadOnlyCollection<TSymbol> Data, int Offset);