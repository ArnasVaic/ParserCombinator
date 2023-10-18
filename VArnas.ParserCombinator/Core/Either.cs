using static VArnas.ParserCombinator.Either;

namespace VArnas.ParserCombinator;

public class Either<TLeft, TRight>(TLeft? left, TRight? right) : IEither<TLeft, TRight>
{
    public IEither<TNew, TRight> First<TNew>(Func<TLeft, TNew> func) => left switch
    {
        null => Right<TNew, TRight>(right),
        _ => Left<TNew, TRight>(func(left))
    };

    public IEither<TLeft, TNew> Second<TNew>(Func<TRight, TNew> func) => right switch
    {
        null => Left<TLeft, TNew>(left),
        _ => Right<TLeft, TNew>(func(right))
    };

    public TResult Match<TResult>(
        Func<TLeft, TResult> fst, 
        Func<TRight, TResult> snd) => 
        right switch { null => fst(left!), _ => snd(right) };
    
    public IEither<TLeft, TResult> Bind<TResult>(Func<TRight, IEither<TLeft, TResult>> func) =>
        right is not null ? 
            func(right) : 
            Left<TLeft, TResult>(left);
    
    public bool IsRight => right is not null;
    
    public bool IsLeft => right is null;
}

/// <summary>
/// Provides helpers to initialize Either objects.
/// </summary>
public static class Either
{
    /// <summary>
    /// Construct an Either from Left value.
    /// </summary>
    /// <param name="left">Value</param>
    /// <typeparam name="TLeft">Type of left</typeparam>
    /// <typeparam name="TRight">Type of right</typeparam>
    /// <returns>Left initialized either</returns>
    public static Either<TLeft, TRight> Left<TLeft, TRight>
        (TLeft? left) => new(left, default);

    /// <summary>
    /// Construct an Either from Right value.
    /// </summary>
    /// <param name="right">Value</param>
    /// <typeparam name="TLeft">Type of left</typeparam>
    /// <typeparam name="TRight">Type of right</typeparam>
    /// <returns>Right initialized either</returns>
    public static Either<TLeft, TRight> Right<TLeft, TRight>
        (TRight? right) => new(default, right);
}