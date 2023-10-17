using System;
using System.Collections.Generic;
using System.Linq;
using static VArnas.ParserCombinator.Parser;

namespace VArnas.ParserCombinator;

public static class CommonParsers
{
    public static IParser<TSymbol, TSymbol> Any<TSymbol>() => new Parser<TSymbol, TSymbol>(input =>
    {
        if (input.Data.Count <= input.Offset)
            return Bad<TSymbol, TSymbol>("Empty input");

        var symbol = input.Data.ElementAt(input.Offset);
        input.Offset++;
        
        return Ok(new ParseResult<TSymbol, TSymbol>(symbol, input));
    });

    public static IParser<TSymbol, TResult> Or<TSymbol, TResult>(IParser<TSymbol, TResult> fst, IParser<TSymbol, TResult> snd)
    {
        return new Parser<TSymbol, TResult>(NewParse);

        IEither<string, IParseResult<TSymbol, TResult>> NewParse(IParserInput<TSymbol> input) => 
            fst.Parse(input).Match<IEither<string, IParseResult<TSymbol, TResult>>>(
                error => snd.Parse(input).Match<IEither<string, IParseResult<TSymbol, TResult>>>(
                    _ =>
                    {
                        var msg = GetErrorMessage(input, error);
                        return Bad<TSymbol, TResult>(msg);
                    },
                    Ok), 
                Ok);
    }
    
    public static IParser<TSymbol, TSymbol> 
        Satisfy<TSymbol>(Predicate<TSymbol> predicate) => new Parser<TSymbol, TSymbol>(input =>
    {
        if (input.Data.Count <= input.Offset)
            return Bad<TSymbol, TSymbol>("Empty input");

        var symbol = input.Data.ElementAt(input.Offset);
        
        if (!predicate(symbol))
            return Bad<TSymbol, TSymbol>($"Unexpected symbol {symbol}");
        
        input.Offset++;
        
        return Ok(new ParseResult<TSymbol, TSymbol>(symbol, input));
    });

    // public static IParser<TSymbol, IEnumerable<TOther>> Seq<TSymbol, TOther>
    //     (params IParser<TSymbol, TOther>[] parsers) => Sequence(parsers);
    
    public static IParser<TSymbol, IEnumerable<TOther>> 
        Sequence<TSymbol, TOther>(IReadOnlyCollection<IParser<TSymbol, TOther>> parsers) => 
        new Parser<TSymbol, IEnumerable<TOther>>(input =>
    {
        var acc = new List<TOther>(parsers.Count);
        var rem = input;
        
        foreach (var parser in parsers)
        {
            var result = parser.Parse(rem);
            
            if (result.Failure)
                return Bad<TSymbol, IEnumerable<TOther>>("Could not parse sequence.");

            result.Map(r =>
            {
                acc.Add(r.Result);
                rem = r.Remaining;
            });
        }

        return Ok(new ParseResult<TSymbol, IEnumerable<TOther>>(acc, rem));
    });

    public static IParser<TSymbol, IEnumerable<TOther>> 
        Some<TSymbol, TOther>(IParser<TSymbol, TOther> parser) => 
        new Parser<TSymbol, IEnumerable<TOther>>(input =>
        {
            var acc = new List<TOther>();
            var rem = input;

            IEither<string, IParseResult<TSymbol, TOther>> result;
            do
            {
                result = parser.Parse(rem);

                result.Map(r =>
                {
                    acc.Add(r.Result);
                    rem = r.Remaining;
                });
            } while (result.Success);

            return Ok(new ParseResult<TSymbol, IEnumerable<TOther>>(acc, rem));
        });
    
    public static IParser<TSymbol, IEnumerable<TOther>> 
        Many<TSymbol, TOther>(IParser<TSymbol, TOther> parser) => parser
            .Bind<IEnumerable<TOther>>(r1 => Some(parser)
                .Bind<IEnumerable<TOther>>(r2 =>
                    Pure<TSymbol, IEnumerable<TOther>>(new List<TOther> { r1 }.Concat(r2))));

    public static IParser<TSymbol, TSymbol> Symbol<TSymbol>(TSymbol target) 
        where TSymbol : IEquatable<TSymbol> => new Parser<TSymbol, TSymbol>(input =>
    {
        if (input.Data.Count <= input.Offset)
            return Bad<TSymbol, TSymbol>("Empty input");

        var symbol = input.Data.ElementAt(input.Offset);
        
        if (!symbol.Equals(target))
            return Bad<TSymbol, TSymbol>($"Unexpected symbol {symbol}");
        
        input.Offset++;
        
        return Ok(new ParseResult<TSymbol, TSymbol>(symbol, input));
    });

    public static Parser<TSymbol, IEnumerable<TOther>>
        Repeat<TSymbol, TOther>(IParser<TSymbol, TOther> parser, int times) => new(input =>
    {
        var array = new TOther[times];
        var rem = input;

        for (var i = 0; i < times; ++i)
        {
            var result = parser.Parse(rem);

            if (result.Failure)
                return Bad<TSymbol, IEnumerable<TOther>>("Could not parse sequence.");

            result.Map(r =>
            {
                array[i] = r.Result;
                rem = r.Remaining;
            });
        }

        return Ok(new ParseResult<TSymbol, IEnumerable<TOther>>(array, rem));
    });

    public static Parser<TSymbol, TOther> 
        OneOf<TSymbol, TOther>(IEnumerable<IParser<TSymbol, TOther>> parsers) => 
        new(input =>
        {
            var result = Bad<TSymbol, TOther>("Could not parse anything"); 
            
            foreach (var parser in parsers)
            {
                result = parser.Parse(input);
                if (result.Success)
                    return result;
            }
            
            return result;
        });

    public static Parser<TSymbol, TOther>
        OneOf<TSymbol, TOther>(params Parser<TSymbol, TOther>[] parsers) =>
            OneOf(parsers.AsEnumerable());
}