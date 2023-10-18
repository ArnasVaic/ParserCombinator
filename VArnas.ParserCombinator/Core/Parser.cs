using static VArnas.ParserCombinator.Either;

namespace VArnas.ParserCombinator;

public class Parser<TSymbol, TResult>(
    Func<IParserInput<TSymbol>, IEither<string, IParseResult<TSymbol, TResult>>> parse)
    : IParser<TSymbol, TResult>
{
    
    public IEither<string, IParseResult<TSymbol, TResult>> 
        Parse(IParserInput<TSymbol> input) => parse(input);
    
    public IParser<TSymbol, TOther> Map<TOther>(Func<TResult, TOther> func) =>
        new Parser<TSymbol, TOther>(input => 
            parse(input).Second<IParseResult<TSymbol, TOther>>(res => 
                res.Map(func)));

    public IParser<TSymbol, TOther> Map<TOther>(TOther result) => 
        new Parser<TSymbol, TOther>(input => parse(input)
            .Second<IParseResult<TSymbol, TOther>>(res => 
                res.Map(_ => result)));

    public IParser<TSymbol, TOther> MapLeft<TOther>(TOther value) => 
        Map<TOther>(_ => value);
    
    public IParser<TSymbol, TOther> Bind<TOther>(Func<TResult, IParser<TSymbol, TOther>> func) =>
        new Parser<TSymbol, TOther>(input => 
            Parse(input).Bind(res => 
                func(res.Result).Parse(res.Remaining)));
}

public static class Parser
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
    
    public static string GetErrorMessage<TSymbol>(IParserInput<TSymbol> input, string error)
    {
        var failurePosition = input.Data
            .Skip(input.Offset)
            .Take(ErrorInputSegmentLength)
            .ToString();
                        
        return $"{input.Offset} Failed to parse {new string(failurePosition)}, error: {error}";
    }
    
    public static IEither<string, IParseResult<TSymbol, TResult>> 
        Ok<TSymbol, TResult>(IParseResult<TSymbol, TResult> result) =>
        Right<string, IParseResult<TSymbol, TResult>>(result);
    
    public static IEither<string, IParseResult<TSymbol, TResult>> 
        Bad<TSymbol, TResult>(string error) =>
        Left<string, IParseResult<TSymbol, TResult>>(error);
    
    public static Parser<TSymbol, TResult> Zero<TSymbol, TResult>(string error) => 
        new(_ => Bad<TSymbol, TResult>(error));
    
    public static Parser<TSymbol, TResult> Pure<TSymbol, TResult>(TResult result) => 
        new(input => Ok(new ParseResult<TSymbol, TResult>(result, input)));
}