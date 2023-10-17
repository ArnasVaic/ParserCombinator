using System.Linq;
using static VArnas.ParserCombinator.CommonParsers;

namespace VArnas.ParserCombinator;

public static class ParserExtensions
{
    public static Either<string, ParseResult<char, TResult>> 
        ParseFromString<TResult>(this Parser<char, TResult> parser, string input) =>
            parser.Parse(new(input.ToArray(), 0));
    
    
}