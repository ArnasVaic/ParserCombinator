using Microsoft.CSharp.RuntimeBinder;
using Binder = System.Reflection.Binder;

namespace ParserCombinator.Core;

public abstract class MonadBase<T>
{
    protected internal MonadBase()
    {
        
    }
    
    public abstract MonadBase<TResult> Bind<TResult>(Func<T, MonadBase<TResult>> func);
}