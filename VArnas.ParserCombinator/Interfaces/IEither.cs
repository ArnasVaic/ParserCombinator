namespace VArnas.ParserCombinator;

public interface IEither<TLeft, out TRight>
{
    IEither<TLeft, TNew> Bind<TNew>(Func<TRight, IEither<TLeft, TNew>> bind);
    
    TNew Match<TNew>(Func<TLeft, TNew> f1, Func<TRight, TNew> f2);

    IEither<TNew, TRight> First<TNew>(Func<TLeft, TNew> func);
    
    IEither<TLeft, TNew> Second<TNew>(Func<TRight, TNew> func);
    
    bool IsRight { get; }
    
    bool IsLeft { get; }
}