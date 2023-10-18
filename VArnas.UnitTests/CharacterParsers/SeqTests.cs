namespace VArnas.UnitTests.CharacterParsers;

public class SeqTests
{
    private static readonly IParser<char, char> Any = Any<char>();
    
    [Fact]
    public void EmptyInput() => FailTest(
        Seq(Any, Any), 
        Empty<char>());

    [Theory]
    [InlineData("abc")]
    [InlineData("def")]
    [InlineData("???")]
    public void GoodInput(string input) => SuccessTest(
        Seq(Any, Any, Any),
        input.ToArray(),
        input,
        3);
}