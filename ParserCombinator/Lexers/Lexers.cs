using ParserCombinator.Core;

namespace ParserCombinator.Lexers;

public record Token
{
}

public record LexResult<T>(T Result, MemoryStream Remaining);

public class LexerState
{
    public LexerState()
    {
        
    }
}

public static class Lexers
{
    public static Either<string, LexResult<char>> Satisfy(
        Predicate<char> predicate,
        IReadOnlyCollection<char> reader)
    {
        if()
            reader.ElementAt()
        var ch = reader.Read()

        if(ch == -1)
            return Either<string, LexResult<char>>
                .Left($"Empty stream");

        var @char = (char)ch;
        
        if (!predicate(@char))
            return Either<string, LexResult<char>>
                .Left($"Unexpected character {@char}");
        
        var result = new LexResult<char>(@char, reader);
        return Either<string, LexResult<char>>.Right(result);
    }

    public static Either<string, LexResult<char>> Is(
        char target,
        StringReader reader) => 
        Satisfy(target.Equals, reader);

    /// <summary>
    /// `Or` parser. Takes two parsers as input.
    /// Note that in case of success both parsers must return the same type.
    /// </summary>
    /// <param name="first">First parser</param>
    /// <param name="second">Second parser</param>
    /// <typeparam name="TResult">Type parsed by the input parsers</typeparam>
    /// <returns>Parser that tries to parse with the first parser and in case of failure tries the second parser</returns>
    public static Func<StringReader, Either<string, LexResult<TResult>>> Or<TResult>(
        Func<StringReader, Either<string, LexResult<TResult>>> first,
        Func<StringReader, Either<string, LexResult<TResult>>> second) => input =>
    {
        first(input).Match(
            left =>
            {
                // try to parse again
            },
            right => right
        );
    };
}