using System.Data;
using ParserCombinator.Core;
using static ParserCombinator.Core.Either;

namespace ParserCombinator.Lexers;

/// <summary>
/// Lex 0 or more.
/// </summary>
/// <param name="lexer"></param>
/// <typeparam name="TResult"></typeparam>
public class SomeLexer<TResult>(LexerBase<TResult> lexer) : LexerBase<IEnumerable<TResult>>
{
    public override Either<string, LexResult<IEnumerable<TResult>>> Lex(LexerInput input)
    {
        var list = new List<TResult>();
        var currentInput = input;

        Either<string, LexResult<TResult>> temp;
        do
        {
            temp = lexer.Lex(currentInput);    
            temp.Map(r =>
            {
                list.Add(r.Result);
                currentInput = r.Remaining;
            });
        } while (temp.Success);
        
        return Right<string, LexResult<IEnumerable<TResult>>>(
            new LexResult<IEnumerable<TResult>>(list, currentInput));
    }
}