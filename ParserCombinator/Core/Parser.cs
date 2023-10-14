using static ParserCombinator.Core.Either;
using static ParserCombinator.Core.Parser;

namespace ParserCombinator.Core;

public class Parser<TSymbol, TResult>(Func<ParserInput<TSymbol>, Either<string, ParseResult<TSymbol, TResult>>> parse)
{
    private static int _errorInputSegmentLength = 16;
    
    public static int ErrorInputSegmentLength
    {
        get => _errorInputSegmentLength;
        set
        {
            if (_errorInputSegmentLength < 0)
                throw new ArgumentException("Error input segment length cannot be a negative number.");
            
            _errorInputSegmentLength = value;
        }
    }
    
    public Func<ParserInput<TSymbol>, Either<string, ParseResult<TSymbol, TResult>>> Parse { get; } = parse;
    
    public Parser<TSymbol, TOther> Map<TOther>(Func<TResult, TOther> func) =>
        new(input => parse(input).Map(res => res.Map(func)));

    public Parser<TSymbol, TOther> MapLeft<TOther>(TOther value) => Map(_ => value);
    
    public Parser<TSymbol, TOther> Bind<TOther>(Func<TResult, Parser<TSymbol, TOther>> func) =>
        new(input => 
            parse(input).Bind(res => 
                func(res.Result).Parse(res.Remaining)));

    private static string ErrorMessage(ParserInput<TSymbol> input, string error)
    {
        var failurePosition = input.Data
            .Skip(input.Offset)
            .Take(ErrorInputSegmentLength)
            .ToString();
                        
        return $"{input.Offset} Failed to parse {new string(failurePosition)}, error: {error}";
    }
    
    public Parser<TSymbol, TResult> Or(Parser<TSymbol, TResult> other) =>
        new(input => Parse(input).Match(
            _ => other.Parse(input).Match(error => 
                Bad<TSymbol, TResult>(ErrorMessage(input, error)), Ok), Ok));
}

public static class Parser
{
    public static Either<string, ParseResult<TSymbol, TResult>> 
        Ok<TSymbol, TResult>(ParseResult<TSymbol, TResult> result) =>
        Right<string, ParseResult<TSymbol, TResult>>(result);
    
    public static Either<string, ParseResult<TSymbol, TResult>> 
        Bad<TSymbol, TResult>(string error) =>
        Left<string, ParseResult<TSymbol, TResult>>(error);
    
    public static Parser<TSymbol, TResult> Zero<TSymbol, TResult>(string error) => 
        new(_ => Bad<TSymbol, TResult>(error));
    
    public static Parser<TSymbol, TResult> Pure<TSymbol, TResult>(TResult result) => 
        new(input => Ok(new ParseResult<TSymbol, TResult>(result, input)));
}