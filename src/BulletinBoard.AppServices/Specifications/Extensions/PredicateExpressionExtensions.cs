using System.Linq.Expressions;

namespace BulletinBoard.AppServices.Specifications.Extensions;

/// <summary>
/// Набор расширений для предикатов.
/// </summary>
public static class PredicateExpressionExtensions
{
    /// <summary>
    /// Выполняет логическую операцию "И" над предикатами.
    /// </summary>
    public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> left,
        Expression<Func<T, bool>> right)
    {
        return left.Compose(right, Expression.AndAlso);
    }
    /// <summary>
    /// Выполняет логическую операцию "ИЛИ" над предикатами.
    /// </summary>
    public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> left,
        Expression<Func<T, bool>> right)
    {
        return left.Compose(right, Expression.OrElse);
    }
    /// <summary>
    /// Выполняет отрицание предиката.
    /// </summary>
    public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expression)
    {
        return Expression.Lambda<Func<T, bool>>(
            Expression.Not(expression.Body), 
            expression.Parameters);
    }
}