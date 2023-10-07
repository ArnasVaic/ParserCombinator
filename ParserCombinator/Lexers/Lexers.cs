using ParserCombinator.Core;

using static ParserCombinator.Core.Either;

namespace ParserCombinator.Lexers;

public class CharLexer(char target) : LexerBase<char>
{
    public override Either<string, LexResult<char>> Lex(LexerInput input) =>
        new PredicateLexer(target.Equals).Lex(input);
}

public class PredicateLexer(Predicate<char> predicate) : LexerBase<char>
{
    public override Either<string, LexResult<char>> Lex(LexerInput input)
    {
        if(input.Data.Count <= input.Offset)
            return Left<string, LexResult<char>>("Empty input");

        var @char = input.Data.ElementAt(input.Offset);
        
        if (!predicate(@char))
            return Left<string, LexResult<char>>($"Unexpected character {@char}");

        input.Offset++;
        
        var result = new LexResult<char>(@char, input);
        return Right<string, LexResult<char>>(result);
    }
}

public class LexerOrCombinator<TResult>(ILexer<TResult> first, ILexer<TResult> second) : LexerBase<TResult>
{
    /// <summary>
    /// Tries to lex with the first lexer, if it fails, then tries to parse with the second lexer.
    /// </summary>
    /// <param name="input">Lexer input</param>
    /// <returns>Lex result</returns>
    public override Either<string, LexResult<TResult>> Lex(LexerInput input) =>
        first.Lex(input).Match<Either<string, LexResult<TResult>>>(
            _ => second.Lex(input).Match(
                Left<string, LexResult<TResult>>,
                Right<string, LexResult<TResult>>
                ),
                Right<string, LexResult<TResult>>
            );
}

public static class CommonLexers
{
    public static CharLexer Is(char target) => new(target);
    
    public static PredicateLexer Satisfy(Predicate<char> predicate) => new(predicate);
    
    public static LexerOrCombinator<TResult> Or<TResult>
        (ILexer<TResult> first, ILexer<TResult> second) => 
            new(first, second);
}