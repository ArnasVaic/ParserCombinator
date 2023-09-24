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

    public static Either<TLeft, TRight> Left(TLeft? left) => new(left, default);

    public static Either<TLeft, TRight> Right(TRight? right) => new(default, right);
    
    public Either<TLeft, TResult> Map<TResult>(Func<TRight, TResult> func)
    {
        if(_right is not null)
            return Either<TLeft, TResult>.Right(func(_right));
        
        if(_left is not null)
            return Either<TLeft, TResult>.Left(_left);

        throw new UnreachableException("Something went really wrong.");
    }

    public void Map(Action<TRight> action)
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
    
    public TResult Match<TResult>(
        Func<TLeft, TResult> handleLeft,
        Func<TRight, TResult> handleRight
        )
    {
        if(_right is not null)
            return handleRight(_right);

        if(_left is not null)
            return handleLeft(_left);

        throw new UnreachableException("Something went really wrong.");
    }
    
    public void Match(
        Action<TLeft> handleLeft,
        Action<TRight> handleRight
    )
    {
        if (_right is not null)
        {
            handleRight(_right);
            return;
        }
        
        if (_left is not null)
        {
            handleLeft(_left);
            return;
        }
        
        throw new UnreachableException("Something went really wrong.");
    }
}