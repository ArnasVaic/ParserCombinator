# ParserCombinator

![example workflow](https://github.com/ArnasVaic/ParserCombinator/actions/workflows/ci.yaml/badge.svg)

The goal of this project is to recreate Haskell like parser combinators in .NET.
While developing this library I am also trying to keep performance, ease of use and expressiveness in mind.

## Simple example

Few of the available ways to define a parser that parses vowels:

```csharp
// These static classes contain all the necesarry tools for building parsers.
using static ParserCombinator.Core.CommonParsers;
using static ParserCombinator.Core.Parser;

// Using Or parser function
var vowelParser = 
    Symbol('a').Or(
    Symbol('e')).Or(
    Symbol('i')).Or(
    Symbol('o')).Or(
    Symbol('u'));

// Using OneOf parser
var vowelParser = OneOf(
    Symbol('a'),
    Symbol('e'),
    Symbol('i'),
    Symbol('o'),
    Symbol('u'));

// QOL method equivalent to the above example.
var vowelParser = OneOf("aeiou");

vowelParser
    .ParseFromString("interesting")
    .Map(Console.Write);

```
aa
## More complicated example

This is an example of a parser that parses number palindromes of length 5 (12321, 98789, etc...) and converts the result to a number.

```csharp
var weirdPalindromeParser =
    digit       .Bind(fst => // We parse any digit and bring it scope as `fst`
    digit       .Bind(snd => // We parse any digit and bring it scope as `snd`
    digit       .Bind(mid => // We parse any digit and bring it scope as `mid`
    Symbol(snd) .Bind(_ =>   // We parse the same digit as `snd`
    Symbol(fst) .Bind(_ =>   // We parse the same digit as `fst`
    {
        var result = int.Parse($"{fst}{snd}{mid}{snd}{fst}");
        return Pure<char, int>(result);
    })))));

weirdPalindromeParser
    .ParseFromString("12321")
    .Map(Console.Write);
```

## Available parsers

- `Any` - parses any single symbol.
- `Symbol` - parses only if it matches provided symbol.
- `Seq` - applies provided parsers that have the same resulting type in sequence.
- `Pure` - parser that consumes nothing and always succeeds with provided result.
- `Zero` - parser that consumes nothing and always fails with provided error message.
- `Some` - applies provided parser 0 or more times.
- `Many` - applies provided parser 1 or more times.
- `Repeat` - applies provided parser n times.
- `Satisfy` - parses a single symbol only if it satisfies provided predicate.
- `OneOf` - given a list of parsers tries each one until succeeds or fails (multi-argument OR combinator).

## Combining parsers

Common parser operations are also available:

- `Bind` - Monadic bind operation used to combine parsers.
- `Map` - Map over a given parser (equivalent to functor `fmap` in Haskell)
- `LeftMap` - Equivalent to Haskell functor operator `<$`.
- `Or` - Equivalent to Haskell alternative operator `<|>`

## Contributing

Never done this before, would be interesting.
