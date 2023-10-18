namespace VArnas.UnitTests.Complex;

public class NumberPalindromeTests
{
    private static readonly IParser<char, char> Digit = Satisfy<char>(char.IsDigit);
        
    private static readonly IParser<char, int> WeirdPalindromeParser =
        Digit       .Bind(fst => // We parse any digit and bring it scope as `fst`
        Digit       .Bind(snd => // We parse any digit and bring it scope as `snd`
        Digit       .Bind(mid => // We parse any digit and bring it scope as `mid`
        Symbol(snd) .Bind(_ =>       // We parse the same digit as `snd`
        Symbol(fst) .Bind(_ =>       // We parse the same digit as `fst`
        {
            var result = int.Parse($"{fst}{snd}{mid}{snd}{fst}");
            return Pure<char, int>(result);
        })))));

    [Theory]
    [InlineData("12321aaa")]
    [InlineData("00900abc")]
    [InlineData("11111def")]
    [InlineData("16161ghi")]
    [InlineData("89998??>")]
    public void ValidInput(string input) => SuccessTest(
        WeirdPalindromeParser,
        input.ToArray(),
        int.Parse(input.Take(5).ToArray()),
        5);
    
    [Theory]
    [InlineData("12311aaa")]
    [InlineData("01900abc")]
    [InlineData("21111def")]
    [InlineData("26161ghi")]
    [InlineData("89938??>")]
    public void InvalidInput(string input) => FailTest(
        WeirdPalindromeParser,
        input.ToArray());
}