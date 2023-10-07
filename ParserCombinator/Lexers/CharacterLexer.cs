using ParserCombinator.Core;

namespace ParserCombinator.Lexers;

public class CharacterLexer(char target) : LexerBase<char>
{
    public override Either<string, LexResult<char>> Lex(LexerInput input) =>
            new PredicateLexer(target.Equals).Lex(input);
}