using ParserCombinator.Lexers;

namespace UnitTests;

public static class LexerTestHelpers
{
    private static void WrongPath<T>(LexResult<T> _) => 
        throw new Exception("Parser was not supposed to succeed");
    
    private static void WrongPath(string error) => 
        throw new Exception(error);
    
    /// <summary>
    /// Test the failure branch of a certain lexer.
    /// </summary>
    /// <param name="lexer">Lexer</param>
    /// <param name="input">Lexer input</param>
    /// <param name="test">Test returned error</param>
    /// <typeparam name="TResult">Type of the result</typeparam>
    public static void TestFailure<TResult>
        (LexerBase<TResult> lexer, string input, Action<string> test) =>
            lexer.Lex(input).Match(test, WrongPath);

    /// <summary>
    /// Test the success branch of a lexer.
    /// </summary>
    /// <param name="lexer">Lexer</param>
    /// <param name="input">Lexer input</param>
    /// <param name="test">Test returned error</param>
    /// <typeparam name="TResult">Type of the result</typeparam>
    public static void TestSuccess<TResult>
        (LexerBase<TResult> lexer, string input, Action<LexResult<TResult>> test) =>
        lexer.Lex(input).Match(WrongPath, test);
}