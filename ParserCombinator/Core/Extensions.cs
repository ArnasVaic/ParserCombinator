namespace ParserCombinator.Core;

public static class Extensions
{
    public static List<T> Wrap<T>(this T element) => new () { element };
}