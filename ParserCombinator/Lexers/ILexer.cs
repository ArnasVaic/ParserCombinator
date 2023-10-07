using ParserCombinator.Core;

namespace ParserCombinator.Lexers;

/// <summary>
/// Lexer has one purpose, to lex some structure from its input.
/// </summary>
/// <typeparam name="TResult">Type of result</typeparam>
public interface ILexer<TResult>
{
    /// <summary>
    /// Lex the input.
    /// The possibility of this operation failing is represent by the Either result.
    /// In case of error, it will return an error message which will propagate to the top.
    /// </summary>
    /// <param name="input">Lexer input</param>
    /// <returns>Lexer result</returns>
    public Either<string, LexResult<TResult>> Lex(LexerInput input);
}

/// <summary>
/// The result of a lexer is some structure and the remaining input.
/// </summary>
/// <param name="Result">Lexed structure</param>
/// <param name="Remaining">Remaining input</param>
/// <typeparam name="TResult"></typeparam>
public record LexResult<TResult>(TResult Result, LexerInput Remaining);

/// <summary>
/// Lexer input reads from an immutable source of characters and tracks the reading position.
/// </summary>
/// <param name="Data">Source of characters to read from</param>
/// <param name="Offset">Index of a character where the lexer will begin reading</param>
public record LexerInput(IReadOnlyCollection<char> Data, int Offset)
{
    public int Offset { get; set; } = Offset;
}