using static ParserCombinator.Core.CommonParsers;

namespace ParserCombinator.Core;

public static class CharacterParsers
{
    public static Parser<char, char> OneOf(string dict) =>
        CommonParsers.OneOf(dict.Select(Symbol));
}