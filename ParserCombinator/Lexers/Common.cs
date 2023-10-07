namespace ParserCombinator.Lexers;

/// <summary>
/// This class provides a develop friendly way to create lexers.
/// </summary>
public static class CommonLexers
{
    public static CharacterLexer Is(char target) => new(target);
    
    public static PredicateLexer Satisfy(Predicate<char> predicate) => new(predicate);
    
    public static OrLexerCombinator<TResult> Or<TResult>
        (ILexer<TResult> first, ILexer<TResult> second) => 
            new(first, second);
}