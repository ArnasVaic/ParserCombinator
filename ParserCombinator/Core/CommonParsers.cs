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

    public static Parser<TSymbol, IEnumerable<TSymbol>> Seq<TSymbol>
        (params Parser<TSymbol, TSymbol>[] parsers) => 
            Seq(parsers.AsEnumerable());
    
    public static Parser<TSymbol, IEnumerable<TSymbol>> 
        Seq<TSymbol>(IEnumerable<Parser<TSymbol, TSymbol>> parsers) => 
        new(input =>
    {
        var acc = new List<TSymbol>();
        var rem = input;
        
        foreach (var parser in parsers)
        {
            var result = parser.Parse(rem);
            
            if (result.Failure)
                return Bad<TSymbol, IEnumerable<TSymbol>>("Could not parse sequence.");

            result.Map(r =>
            {
                acc.Add(r.Result);
                rem = r.Remaining;
            });
        }

        return Ok<TSymbol, IEnumerable<TSymbol>>(new(acc, rem));
    });

    public static Parser<TSymbol, IEnumerable<TSymbol>> 
        Some<TSymbol>(Parser<TSymbol, TSymbol> parser) => 
        new (input =>
        {
            var acc = new List<TSymbol>();
            var rem = input;

            Either<string, ParseResult<TSymbol, TSymbol>> result;
            do
            {
                result = parser.Parse(rem);

                result.Map(r =>
                {
                    acc.Add(r.Result);
                    rem = r.Remaining;
                });
            } while (result.Success);

            return Ok<TSymbol, IEnumerable<TSymbol>>(new(acc, rem));
        });
    
    public static Parser<TSymbol, IEnumerable<TSymbol>> 
        Many<TSymbol>(Parser<TSymbol, TSymbol> parser) => parser
        .Bind<IEnumerable<TSymbol>>(r1 => Some(parser)
        .Bind<IEnumerable<TSymbol>>(r2 => 
        Pure<TSymbol, IEnumerable<TSymbol>>(new List<TSymbol> { r1 }.Concat(r2))));

    public static Parser<TSymbol, TSymbol>
        Symbol<TSymbol>(TSymbol target) 
        where TSymbol : IEquatable<TSymbol> => 
        Satisfy<TSymbol>(target.Equals);
}