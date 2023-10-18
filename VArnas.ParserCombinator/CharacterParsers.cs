using VArnas.ParserCombinator.Interfaces;
using static VArnas.ParserCombinator.CommonParsers;

namespace VArnas.ParserCombinator;

public static class CharacterParsers
{
    public static IParser<char, char> OneOf(string dict) =>
        CommonParsers.OneOf(dict.Select(Symbol));

    public static IParser<char, IEnumerable<char>> String(string str) =>
        Sequence(str.Select(Symbol).ToArray());
}