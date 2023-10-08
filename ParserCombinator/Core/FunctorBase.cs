namespace ParserCombinator.Core;

/// <summary>
/// Functor base class, provides a way to map underlying types.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class FunctorBase<T>
{
    /// <summary>
    /// Map functor to some other type.
    /// </summary>
    /// <param name="func">Function to apply</param>
    /// <typeparam name="TResult">Resulting underlying type</typeparam>
    /// <returns>Mapped functor</returns>
    public abstract FunctorBase<TResult> Map<TResult>(Func<T, TResult> func);
    
    /// <summary>
    /// Map functor without returning. Meant to be used with impure functions.
    /// </summary>
    /// <param name="func">Impure function to apply</param>
    public abstract void Map(Action<T> func);
    
    /// <summary>
    /// Map functor to same type. Can be used to avoid unnecessary allocation.
    /// </summary>
    /// <param name="func">Function to apply</param>
    /// <returns>Mapped functor</returns>
    public abstract FunctorBase<T> Map(Func<T, T> func);
}