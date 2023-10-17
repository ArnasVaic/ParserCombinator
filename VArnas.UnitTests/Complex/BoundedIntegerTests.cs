using VArnas.ParserCombinator;
using Xunit;
using static VArnas.ParserCombinator.CommonParsers;
using static VArnas.ParserCombinator.Parser;
using static VArnas.UnitTests.TestHelpers;
using static System.Array;

namespace VArnas.UnitTests.Complex;

public class BoundedIntegerTests
{
    private static readonly Parser<char, char> OptionalSign = 
        Symbol('-').Or(Pure<char, char>(' '));
    
    private static readonly Parser<char, char> Digit = Satisfy<char>(char.IsDigit);

    private static readonly Parser<char, char> NonZeroDigit = Digit.Bind(d => d == 0 ? 
        Zero<char, char>("0 cannot be the first digit") : 
        Pure<char, char>(d));

    private static readonly Parser<char, int> Int32Parser = 
        OptionalSign    .Bind(sgn => 
        NonZeroDigit    .Bind(fst => 
        Some(Digit)     .Bind(rem =>
        {
            var chars = new List<char> { sgn, fst }.Concat(rem);
            var asString = new string(chars.ToArray());
            
            // Bounds checking logic wouldn't be implemented in a functional manner so why bother.
            return int.TryParse(asString, out var result)
                ? Pure<char, int>(result)
                : Zero<char, int>("Integer out of bounds");
        })));

    [Fact]
    public void EmptyInput() => FailTest(Int32Parser, Empty<char>());

    [Theory]
    [InlineData(1)]
    [InlineData(99)]
    [InlineData(123)]
    [InlineData(4_444)]
    [InlineData(98_765)]
    [InlineData(765_174)]
    [InlineData(1_999_999)]
    [InlineData(18_123_456)]
    [InlineData(100_000_000)]
    [InlineData(2147483647)]
    public void PositiveIntegers(int input)
    {
        var inputAsStr = input.ToString();
        SuccessTest(
            Int32Parser,
            inputAsStr.ToArray(),
            input, 
            inputAsStr.Length
        );
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-7)]
    [InlineData(-99)]
    [InlineData(-123)]
    [InlineData(-4_444)]
    [InlineData(-98_765)]
    [InlineData(-765_174)]
    [InlineData(-1_999_999)]
    [InlineData(-18_123_456)]
    [InlineData(-100_000_000)]
    [InlineData(-2147483648)]
    [InlineData(-2147482648)]
    public void NonPositiveIntegers(int input)
    {
        var inputAsStr = input.ToString();
        SuccessTest(
            Int32Parser,
            inputAsStr.ToArray(),
            input, 
            inputAsStr.Length
        );
    }

    [Theory]
    [InlineData("123abc")]
    [InlineData("-999999999foobar")]
    [InlineData("8512312testinggg")]
    [InlineData("-0interastingcase")]
    public void ValidSurplusInput(string input)
    {
        var target = input.TakeWhile(c => !char.IsLetter(c)).ToArray();
        
        SuccessTest(
            Int32Parser,
            input.ToArray(),
            int.Parse(target),
            target.Length);
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("-abc")]
    [InlineData("x12345")]
    [InlineData("?/.,:punctuators123")]
    public void InvalidInput(string input) => 
        FailTest(Int32Parser, input.ToArray());
}