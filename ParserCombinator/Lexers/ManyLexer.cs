using System.Data;
using ParserCombinator.Core;
using static ParserCombinator.Core.Either;
using static ParserCombinator.Lexers.Lexers;

namespace ParserCombinator.Lexers;

/// <summary>
/// Lex 1 or more.
/// </summary>
/// <param name="lexer"></param>
/// <typeparam name="TResult"></typeparam>
public class ManyLexer<TResult>(LexerBase<TResult> lexer) : LexerBase<IEnumerable<TResult>>
{
    public override Either<string, LexResult<IEnumerable<TResult>>> Lex(LexerInput input)
    {
        return lexer.Lex(input).Match(
            Left<string, LexResult<IEnumerable<TResult>>>,
            r1 =>
            {
                return Some(lexer).Lex(r1.Remaining).Match(
                    Left<string, LexResult<IEnumerable<TResult>>>,
                    r2 =>
                    {
                        var concatenated = new List<TResult>
                        {
                            r1.Result
                        }.Concat(r2.Result.ToList());
                        
                        return Right<string, LexResult<IEnumerable<TResult>>>(
                            new LexResult<IEnumerable<TResult>>(concatenated, r2.Remaining));
                    });
            }
        );
    }
}