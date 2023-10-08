using ParserCombinator.Core;
using static ParserCombinator.Core.Either;

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