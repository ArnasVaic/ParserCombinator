using ParserCombinator.Core;

namespace ParserCombinator.Lexers;

/// <summary>
/// LexerBase provides a developer friendly way to use lexers.
/// </summary>
/// <typeparam name="TResult"></typeparam>
public abstract class LexerBase<TResult> : ILexer<TResult>
{
    /// <summary>
    /// Lexes the input.
    /// </summary>
    /// <param name="input">Lexer input</param>
    /// <returns>Lexing result</returns>
    public abstract Either<string, LexResult<TResult>> Lex(LexerInput input);

    /// <summary>
    /// Lex directly from immutable collection of characters. Will initialise offset to 0.
    /// </summary>
    /// <param name="input">Lexer input</param>
    /// <returns>Lexing result</returns>
    public Either<string, LexResult<TResult>> Lex(IReadOnlyCollection<char> input) => 
        Lex(new LexerInput(input, 0));

    /// <summary>
    /// Lex directly from string.
    /// </summary>
    /// <param name="input">Lexer input</param>
    /// <returns>Lexing result</returns>
    public Either<string, LexResult<TResult>> Lex(string input) =>
        Lex(input.ToArray());
}
        