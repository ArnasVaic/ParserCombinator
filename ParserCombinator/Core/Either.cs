namespace ParserCombinator.Core;

using System.Diagnostics;

public class Either<TLeft, TRight>
{
    private readonly TLeft? _left;

    private readonly TRight? _right;

    private Either(TLeft? left, TRight? right)
    {
        _left = left;
        _right = right;
    }

    public static Either<L, R> From<L, R>(L? left) => new(left, default);

    public static Either<L, R> From<L, R>(R? right) => new(default, right);

    public Either<TLeft, TResult> Map<TResult>(Func<TRight, TResult> func)
    {
        if(_right is not null)
            return From<TLeft, TResult>(func(_right));
        
        if(_left is not null)
            return From<TLeft, TResult>(_left);

        throw new UnreachableException("Something went really wrong.");
    }

    public void Map<TResult>(Action<TRight> action)
    {
        if(_right is not null)
        {
            action(_right);
            return;
        }

        if(_left is not null)
            return;

        throw new UnreachableException("Something went really wrong.");
    }
}