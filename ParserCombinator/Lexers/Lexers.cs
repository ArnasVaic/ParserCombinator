namespace ParserCombinator.Lexers;

/// <summary>
/// This class provides a develop friendly way to combine lexers.
/// Include this class as static for maximum fluency.
/// </summary>
public static class Lexers
{
    public static CharacterLexer Is(char target) => new(target);
    
    public static PredicateLexer Satisfy(Predicate<char> predicate) => new(predicate);
    
    public static OrLexerCombinator<TResult> Or<TResult>
        (ILexer<TResult> first, ILexer<TResult> second) => 
            new(first, second);
    
    public static ManyOrLexerCombinator<TResult> Or<TResult>
        (params ILexer<TResult>[] lexers) => 
            new(lexers);

    public static SequenceLexer<TResult> Seq<TResult>
        (params ILexer<TResult>[] lexers) =>
            new(lexers);
    
    public static SequenceLexer<char> Seq(string pattern) =>
        new(pattern.Select(Is));

    public static PredicateLexer Digit => new(char.IsDigit);
    
    public static PredicateLexer WhiteSpace => new(char.IsWhiteSpace);
}