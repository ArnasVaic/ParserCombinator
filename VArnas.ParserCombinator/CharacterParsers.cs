using System.Linq;
using static VArnas.ParserCombinator.CommonParsers;

namespace VArnas.ParserCombinator;

public static class CharacterParsers
{
    public static IParser<char, char> OneOf(string dict) =>
        CommonParsers.OneOf(dict.Select(Symbol));
}