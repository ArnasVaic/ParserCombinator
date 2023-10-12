using ParserCombinator.Core;

namespace ParserCombinator.Lexers;

/// <summary>
/// LexerBase provides a developer friendly way to use lexers.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class LexerBase<T> : ILexer<T>
{
    /// <summary>
    /// Lexes the input.
    /// </summary>
    /// <param name="input">Lexer input</param>
    /// <returns>Lexing result</returns>
    public abstract Either<string, LexResult<T>> Lex(LexerInput input);

    /// <summary>
    /// Lex directly from immutable collection of characters. Will initialise offset to 0.
    /// </summary>
    /// <param name="input">Lexer input</param>
    /// <returns>Lexing result</returns>
    public Either<string, LexResult<T>> Lex(IReadOnlyCollection<char> input) => 
        Lex(new LexerInput(input, 0));

    /// <summary>
    /// Lex directly from string.
    /// </summary>
    /// <param name="input">Lexer input</param>
    /// <returns>Lexing result</returns>
    public Either<string, LexResult<T>> Lex(string input) =>
        Lex(input.ToArray());
    
    // public LexerBase<TResult> Bind<TResult>(Func<T, LexerBase<TResult>> func)
    // {
    //     
    // }
}