namespace VArnas.UnitTests.CharacterParsers;

public class RepeatTests
{
    [Fact]
    public void EmptyInput_InnerParserConsumesCharacter() => 
        FailTest(Repeat(Any<char>(), 1), Empty<char>());
    
    [Fact]
    public void NonEmptyInput_AnyCharInnerParser() => SuccessTest(
        Repeat(Any<char>(), 4),
        "repeat parser applied parser P n times".ToArray(),
        "repe".ToArray(),
        4);

    [Theory]
    [InlineData("acdba")]
    [InlineData("AwrdNAwdb")]
    [InlineData("cBA AWDA3r AWDb ")]
    [InlineData("B.A/R?WA AWEDB")]
    public void NonEmptyInput(string input) => SuccessTest(
        Repeat(Any<char>(), 5),
        input.ToArray(),
        input.Take(5), 5);
}