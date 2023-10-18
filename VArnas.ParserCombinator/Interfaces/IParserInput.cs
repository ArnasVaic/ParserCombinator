namespace VArnas.ParserCombinator.Interfaces;

public interface IParserInput<out TSymbol>
{
    public IReadOnlyCollection<TSymbol> Data { get; }
    
    public int Offset { get; set; }
}