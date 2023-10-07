﻿using ParserCombinator.Core;
using static ParserCombinator.Core.Either;

namespace ParserCombinator.Lexers;

public class OrLexerCombinator<TResult>(
    ILexer<TResult> first, 
    ILexer<TResult> second)
    : LexerBase<TResult>
{
    /// <summary>
    /// Tries to lex with the first lexer, if it fails, then tries to lex with the second lexer.
    /// </summary>
    /// <param name="input">Lexer input</param>
    /// <returns>Lex result</returns>
    public override Either<string, LexResult<TResult>> 
        Lex(LexerInput input) => first.Lex(input).Match(
            _ => second.Lex(input),
            Right<string, LexResult<TResult>>);
}