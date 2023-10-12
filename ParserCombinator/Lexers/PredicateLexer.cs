using System.Diagnostics.Contracts;
using ParserCombinator.Core;
using static ParserCombinator.Core.Either;
using static ParserCombinator.Lexers.Lexer;

namespace ParserCombinator.Lexers;

/// <summary>
/// Lexes a character if it satisfies some given predicate.
/// </summary>
/// <param name="predicate"></param>
public class PredicateLexer(Predicate<char> predicate) : LexerBase<char>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public override Either<string, LexResult<char>> Lex(LexerInput input)
    {
        if(input.Data.Count <= input.Offset)
            return Left<string, LexResult<char>>("Empty input");

        var @char = input.Data.ElementAt(input.Offset);

        var success = predicate(@char);
        
        if (!success)
            return Left<string, LexResult<char>>($"Unexpected character {@char}");

        input.Offset++;
        
        return Right<string, LexResult<char>>(new(@char, input));
    }
}

public class Lexer<T>(Func<LexerInput, Either<string, LexResult<T>>> lex)
{
    public Func<LexerInput, Either<string, LexResult<T>>> LexFunc { get; } = lex;

    public Either<string, LexResult<T>> Lex(string input) => 
        lex(new(input.ToArray(), 0));
    
    public Lexer<TResult> Map<TResult>(Func<T, TResult> func) =>
        new(input => lex(input).Map(res => res.Map(func)));

    public Lexer<TResult> Bind<TResult>(Func<T, Lexer<TResult>> func) =>
        new(input => 
            lex(input).Bind(res => 
                func(res.Result).LexFunc(res.Remaining)));

    private static string LexErrorMessage(LexerInput input, string error)
    {
        var failurePosition = input.Data
            .Skip(input.Offset)
            .Take(16)
            .ToString();
                        
        return $"Failed to lex {new string(failurePosition)}, error: {error}";
    }
    
    public Lexer<T> Or(Lexer<T> other) =>
        new(input => LexFunc(input).Match(
            _ => other.LexFunc(input).Match(error => 
                Bad<T>(LexErrorMessage(input, error)), Ok), Ok));
}

public static class Lexer
{
    public static Either<string, LexResult<TResult>> 
        Ok<TResult>(LexResult<TResult> result) =>
        Right<string, LexResult<TResult>>(result);
    
    public static Either<string, LexResult<TResult>> 
        Bad<TResult>(string error) =>
        Left<string, LexResult<TResult>>(error);
}

public static class CommonLexers
{
    
    public static readonly Lexer<char> AnyChar = new(input => 
        input.Data.Count <= input.Offset ? 
        Bad<char>("Empty input") : 
        Ok<char>(new (
            input.Data.First(), 
            input.Advance())));

    public static Lexer<char> Satisfy(Predicate<char> predicate) => 
        AnyChar.Bind(c => predicate(c) ? 
            Pure(c) : 
            Zero<char>($"Unexpected character {c}."));

    public static Lexer<char> Digit => Satisfy(char.IsDigit);
    
    public static Lexer<char> Letter => Satisfy(char.IsLetter);
    
    public static Lexer<T> Zero<T>(string error) => new(_ => Bad<T>(error));
    public static Lexer<T> Zero<T>() => new(_ => Bad<T>(string.Empty));
    
    public static Lexer<T> Pure<T>(T result) => 
        new(input => Ok(new LexResult<T>(result, input)));
}