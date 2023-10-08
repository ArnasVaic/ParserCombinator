namespace ParserCombinator.Core;

using System.Diagnostics;

/// <summary>
/// Represents a calculation that can fail.
/// </summary>
/// <typeparam name="TLeft">Failure type</typeparam>
/// <typeparam name="TRight">Success type</typeparam>
public class Either<TLeft, TRight> : FunctorBase<TRight>
{
    private readonly TLeft? _left;

    private TRight? _right;

    internal Either(TLeft? left, TRight? right)
    {
        _left = left;
        _right = right;
    }
    
    /// <summary>
    /// Apply a function. In case Either has failed - does nothing.
    /// </summary>
    /// <param name="func">Function</param>
    /// <typeparam name="TResult">New success type</typeparam>
    /// <returns>Transformed Either</returns>
    /// <exception cref="UnreachableException"></exception>
    public override Either<TLeft, TResult> Map<TResult>(Func<TRight, TResult> func)
    {
        if(Success)
            return Either.Right<TLeft, TResult>(func(_right));
        
        if(Failure)
            return Either.Left<TLeft, TResult>(_left);

        throw new UnreachableException("Something went really wrong.");
    }

    /// <summary>
    /// Apply a function that returns nothing. In case Either has failed - does nothing.
    /// </summary>
    /// <param name="action"></param>
    /// <exception cref="UnreachableException"></exception>
    public override void Map(Action<TRight> action)
    {
        if(Success)
        {
            action(_right);
            return;
        }

        if(Failure)
            return;

        throw new UnreachableException("Something went really wrong.");
    }

    public override FunctorBase<TRight> Map(Func<TRight, TRight> func)
    {
        if (Success)
            _right = func(_right);

        return this;
    }

    /// <summary>
    /// Pattern match underlying calculation.
    /// </summary>
    /// <param name="handleLeft">Handle failure</param>
    /// <param name="handleRight">Handle success</param>
    /// <typeparam name="TResult">Type of result</typeparam>
    /// <returns>Result of the left or right branch</returns>
    /// <exception cref="UnreachableException"></exception>
    public TResult Match<TResult>
        (Func<TLeft, TResult> handleLeft, Func<TRight, TResult> handleRight)
    {
        if(Success)
            return handleRight(_right);

        if(Failure)
            return handleLeft(_left);

        throw new UnreachableException("Something went really wrong.");
    }
    
    /// <summary>
    /// Pattern match underlying calculation, don't return anything.
    /// </summary>
    /// <param name="handleLeft">Handle failure</param>
    /// <param name="handleRight">Handle success</param>
    /// <exception cref="UnreachableException"></exception>
    public void Match(Action<TLeft> handleLeft, Action<TRight> handleRight)
    {
        if (Success)
        {
            handleRight(_right);
            return;
        }
        
        if (Failure)
        {
            handleLeft(_left);
            return;
        }
        
        throw new UnreachableException("Something went really wrong.");
    }

    /// <summary>
    /// Flag indicating calculation success.
    /// </summary>
    public bool Success => _right is not null;
    
    /// <summary>
    /// Flag indicating calculation failure.
    /// </summary>
    public bool Failure => _left is not null;
}

/// <summary>
/// Provides helpers to initialize Either objects.
/// </summary>
public static class Either
{
    /// <summary>
    /// Construct an Either object that represents failure.
    /// </summary>
    /// <param name="left">Value</param>
    /// <typeparam name="TLeft">Type of failure</typeparam>
    /// <typeparam name="TRight">Type of success</typeparam>
    /// <returns>Either object</returns>
    public static Either<TLeft, TRight> Left<TLeft, TRight>
        (TLeft? left) => new(left, default);

    /// <summary>
    /// Construct an Either object that represents success.
    /// </summary>
    /// <param name="right">Value</param>
    /// <typeparam name="TLeft">Type of failure</typeparam>
    /// <typeparam name="TRight">Type of success</typeparam>
    /// <returns>Either object</returns>
    public static Either<TLeft, TRight> Right<TLeft, TRight>
        (TRight? right) => new(default, right);
}