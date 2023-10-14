using static ParserCombinator.Core.Parser;

namespace ParserCombinator.Core;

public static class CommonParsers
{
    public static Parser<TSymbol, TSymbol> 
        Any<TSymbol>() => new(input => 
        input.Data.Count <= input.Offset ? 
        Bad<TSymbol, TSymbol>("Empty input") : 
        Ok<TSymbol, TSymbol>(new (
            input.Data.ElementAt(input.Offset), 
            input with { Offset = input.Offset + 1})));
    
    public static Parser<TSymbol, TSymbol> 
        Satisfy<TSymbol>(Predicate<TSymbol> predicate) => 
        Any<TSymbol>().Bind<TSymbol>(c => predicate(c) ? 
            Pure<TSymbol, TSymbol>(c) : 
            Zero<TSymbol, TSymbol>($"Unexpected character {c}."));

    public static Parser<TSymbol, IEnumerable<TOther>> Seq<TSymbol, TOther>
        (params Parser<TSymbol, TOther>[] parsers) => 
            Seq(parsers.AsEnumerable());
    
    public static Parser<TSymbol, IEnumerable<TOther>> 
        Seq<TSymbol, TOther>(IEnumerable<Parser<TSymbol, TOther>> parsers) => 
        new(input =>
    {
        var acc = new List<TOther>();
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
        Repeat<TSymbol, TOther>(Parser<TSymbol, TOther> parser, int times) => 
            Seq(Enumerable.Repeat(parser, times));

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