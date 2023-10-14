using static ParserCombinator.Core.CommonParsers;

namespace ParserCombinator.Core;

public static class ParserExtensions
{
    public static Either<string, ParseResult<char, TResult>> 
        ParseFromString<TResult>(this Parser<char, TResult> parser, string input) =>
            parser.Parse(new(input.ToArray(), 0));
}