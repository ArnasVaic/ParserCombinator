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
/// <typeparam name="T">Type of result</typeparam>
public class LexResult<T> : FunctorBase<T>
{
    /// <summary>
    /// The result of a lexer is some structure and the remaining input.
    /// </summary>
    /// <param name="result">Lexed structure</param>
    /// <param name="remaining">Remaining input</param>
    /// <typeparam name="T">Type of result</typeparam>
    public LexResult(T result, LexerInput remaining)
    {
        Result = result;
        Remaining = remaining;
    }

    public T Result { get; }
    
    public LexerInput Remaining { get; set; }
    
    public override LexResult<TResult> Map<TResult>(Func<T, TResult> func) => 
        new(func(Result), Remaining);

    public override void Map(Action<T> func) => func(Result);

    public override LexResult<T> Map(Func<T, T> func)
    {
        func(Result);
        return this;
    }
}

/// <summary>
/// Lexer input reads from an immutable source of characters and tracks the reading position.
/// </summary>
/// <param name="data">Source of characters to read from</param>
/// <param name="offset">Index of a character where the lexer will begin reading</param>
public struct LexerInput(IReadOnlyCollection<char> data, int offset)
{
    public IReadOnlyCollection<char> Data { get; } = data;
    
    public int Offset { get; set; } = offset;
}