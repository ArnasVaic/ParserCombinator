using VArnas.ParserCombinator.Core;
using VArnas.ParserCombinator.Interfaces;

namespace VArnas.ParserCombinator;

public static class ParserExtensions
{
    public static IEither<string, IParseResult<char, TResult>> 
        ParseFromString<TResult>(this Parser<char, TResult> parser, string input) =>
            parser.Parse(new ParserInput<char>(input.ToArray(), 0));
}