using ParserCombinator.Core;
using static ParserCombinator.Core.Either;

namespace ParserCombinator.Lexers;

/// <summary>
/// Apply given lexers in sequence.
/// </summary>
/// <param name="lexers">Sequence of lexers</param>
/// <typeparam name="TResult">Type of result</typeparam>
public class SequenceLexer<TResult>(IEnumerable<ILexer<TResult>> lexers)
    : LexerBase<IEnumerable<TResult>>
{
    public override Either<string, LexResult<IEnumerable<TResult>>> Lex(LexerInput input)
    {
        var result = Right<string, LexResult<IList<TResult>>>(
            new LexResult<IList<TResult>>(new List<TResult>(), input));
        
        var currentInput = input;

        foreach (var lexer in lexers)
        {
            var temp = lexer.Lex(currentInput);

            temp.Match(
                error => result = Left<string, LexResult<IList<TResult>>>(error),
                acc => result.Map(rr =>
                {
                    rr.Result.Add(acc.Result);
                    rr.Remaining = acc.Remaining;
                    currentInput = acc.Remaining;
                }));

            if (temp.Failure)
                break;
        }
        
        // TODO: Consider putting Either structure inside LexResult, maybe it would simplify code.
        return result.Map(r => r.Map(list => list.AsEnumerable()));
    }
}