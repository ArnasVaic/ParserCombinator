using System;
using System.Collections.Generic;
using System.Linq;
using static VArnas.ParserCombinator.Parser;

namespace VArnas.ParserCombinator;

public static class CommonParsers
{
    public static Parser<TSymbol, TSymbol> Any<TSymbol>() => new(input =>
    {
        if (input.Data.Count <= input.Offset)
            return Bad<TSymbol, TSymbol>("Empty input");

        var symbol = input.Data.ElementAt(input.Offset);
        input.Offset++;
        
        return Ok<TSymbol, TSymbol>(new(symbol, input));
    });
    
    public static Parser<TSymbol, TSymbol> 
        Satisfy<TSymbol>(Predicate<TSymbol> predicate) => new(input =>
    {
        if (input.Data.Count <= input.Offset)
            return Bad<TSymbol, TSymbol>("Empty input");

        var symbol = input.Data.ElementAt(input.Offset);
        
        if (!predicate(symbol))
            return Bad<TSymbol, TSymbol>($"Unexpected symbol {symbol}");
        
        input.Offset++;
        
        return Ok<TSymbol, TSymbol>(new(symbol, input));
    });

    public static Parser<TSymbol, IEnumerable<TOther>> Seq<TSymbol, TOther>
        (params Parser<TSymbol, TOther>[] parsers) => Sequence(parsers);
    
    public static Parser<TSymbol, IEnumerable<TOther>> 
        Sequence<TSymbol, TOther>(IReadOnlyCollection<Parser<TSymbol, TOther>> parsers) => 
        new(input =>
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

        return Ok<TSymbol, IEnumerable<TOther>>(new(acc, rem));
    });

    public static Parser<TSymbol, IEnumerable<TOther>> 
        Some<TSymbol, TOther>(Parser<TSymbol, TOther> parser) => 
        new (input =>
        {
            var acc = new List<TOther>();
            var rem = input;

            Either<string, ParseResult<TSymbol, TOther>> result;
            do
            {
                result = parser.Parse(rem);

                result.Map(r =>
                {
                    acc.Add(r.Result);
                    rem = r.Remaining;
                });
            } while (result.Success);

            return Ok<TSymbol, IEnumerable<TOther>>(new(acc, rem));
        });
    
    public static Parser<TSymbol, IEnumerable<TOther>> 
        Many<TSymbol, TOther>(Parser<TSymbol, TOther> parser) => parser
            .Bind<IEnumerable<TOther>>(r1 => Some(parser)
                .Bind<IEnumerable<TOther>>(r2 =>
                    Pure<TSymbol, IEnumerable<TOther>>(new List<TOther> { r1 }.Concat(r2))));

    public static Parser<TSymbol, TSymbol>
        Symbol<TSymbol>(TSymbol target) 
        where TSymbol : IEquatable<TSymbol> => 
        Satisfy<TSymbol>(target.Equals);

    public static Parser<TSymbol, IEnumerable<TOther>>
        Repeat<TSymbol, TOther>(Parser<TSymbol, TOther> parser, int times) => new(input =>
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

        return Ok<TSymbol, IEnumerable<TOther>>(new(array, rem));
    });

    public static Parser<TSymbol, TOther> 
        OneOf<TSymbol, TOther>(IEnumerable<Parser<TSymbol, TOther>> parsers) => 
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