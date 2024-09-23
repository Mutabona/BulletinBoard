using System.Linq.Expressions;
using BulletinBoard.AppServices.Specifications.Internal;

namespace BulletinBoard.AppServices.Specifications;

/// <summary>
/// Набор расширений для спецификаций.
/// </summary>
public static class SpecificationExtensions
{
    /// <summary>
    /// Компонует две спецификации при помощи логического оператора "И".
    /// </summary>
    public static Specification<T> And<T>(this ISpecification<T> left, ISpecification<T> right)
    {
        if (left == null) 
            throw new ArgumentNullException(nameof(left));
        
        if (right == null) 
            throw new ArgumentNullException(nameof(right));
        return new AndSpecification<T>(left, right);
    }
    /// <summary>
    /// Компонует спецификацию с деревом выражений предиката при помощи логического оператора "И".
    /// </summary>
    public static Specification<T> AndPredicate<T>(this ISpecification<T> left, Expression<Func<T, bool>> expression)
    {
        if (expression == null) 
            throw new ArgumentNullException(nameof(expression));
        
        return new AndSpecification<T>(left, Specification<T>.FromPredicate(expression));
    }
    
    /// <summary>
    /// Компонует две спецификации при помощи логического оператора "ИЛИ".
    /// </summary>
    public static Specification<T> Or<T>(this ISpecification<T> left, ISpecification<T> right)
    {
        if (left == null) 
            throw new ArgumentNullException(nameof(left));
        
        if (right == null)
            throw new ArgumentNullException(nameof(right));
        return new OrSpecification<T>(left, right);
    }
    
    /// <summary>
    /// Компонует спецификацию с деревом выражений предиката при помощи логического оператора "ИЛИ".
    /// </summary>
    public static Specification<T> OrPredicate<T>(this ISpecification<T> left, Expression<Func<T, bool>> expression)
    {
        if (expression == null) 
            throw new ArgumentNullException(nameof(expression));
        
        return new OrSpecification<T>(left, Specification<T>.FromPredicate(expression));
    }
    
    /// <summary>
    /// Выполняет отрицание спецификации.
    /// </summary>
    public static Specification<T> Not<T>(this ISpecification<T> specification)
    {
        if (specification == null)
            throw new ArgumentNullException(nameof(specification));
        
        return new NotSpecification<T>(specification);
    }
}