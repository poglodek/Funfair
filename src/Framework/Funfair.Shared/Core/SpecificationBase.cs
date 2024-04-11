using System.Linq.Expressions;

namespace Funfair.Shared.Core;

public abstract class SpecificationBase<T>
{
    protected abstract Expression<Func<T, bool>> AsPredicateExpression();
    
    public static implicit operator Expression<Func<T, bool>>(SpecificationBase<T> specification)
        => specification.AsPredicateExpression();

    public bool Check(T obj) 
        => AsPredicateExpression().Compile().Invoke(obj);
}