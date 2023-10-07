# ParserCombinator

The goal of this project is to recreate Haskell like parser 
and lexer combinators in .NET

## Examples

So far only very primitive lexers are available.
Example lexer that lexes vowels.

```csharp
// All parsers like Or, Is and so on, are part of the Lexers static class.
using static ParserCombinator.Lexers.Lexers;

var vowelLexer = Or(
    Is('a'), 
    Is('e'), 
    Is('i'), 
    Is('o'), 
    Is('u'));

vowelLexer
    .Lex("e")
    .Map(r => Console.WriteLine(r.Result));

```

This will print the letter `e` as it is a vowel and was successfully lexed.

## Contributing

I would be surprised if anyone reads this
