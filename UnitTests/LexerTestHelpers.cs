using ParserCombinator.Exceptions;
using ParserCombinator.Lexers;

namespace UnitTests;

public static class LexerTestHelpers
{
    public static void wrongPath<T>(LexResult<T> _) => 
        throw new LexerException("Parser was not supposed to succeed");
    
    public static void wrongPath(string error) => 
        throw new LexerException(error);
    
    /// <summary>
    /// Test the failure branch of a certain lexer.
    /// </summary>
    /// <param name="lexer">Lexer</param>
    /// <param name="input">Lexer input</param>
    /// <param name="test">Test returned error</param>
    /// <typeparam name="TResult">Type of the result</typeparam>
    public static void TestFailure<TResult>(
        LexerBase<TResult> lexer,
        string input,
        Action<string> test) =>
        lexer.Lex(input).Match(test, wrongPath);

    public static void TestSuccess<TResult>(
        LexerBase<TResult> lexer,
        string input,
        Action<TResult> test) =>
        lexer.Lex(input).Match(wrongPath, lr => test(lr.Result));
}