using System.Linq.Expressions;

namespace BulletinBoard.AppServices.Specifications.Extensions;

/// <summary>
/// Набор расширений для <see cref="Expression"/>.
/// </summary>
public static class ExpressionExtensions
{
    /// <summary>
    /// Выполняет композицию выражений.
    /// </summary>
    /// <param name="left">Первое выражение.</param>
    /// <param name="right">Правое выражение.</param>
    /// <param name="compose">Операция композиции.</param>
    /// <typeparam name="T">Тип делегата.</typeparam>
    /// <returns>Скомпонованное выражение.</returns>
    public static Expression<T> Compose<T>(this Expression<T> left, Expression<T> right,
        Func<Expression, Expression, Expression> compose)
    {
        var rightBody = ParameterRebinderExpressionVisitor.RebindParameters(left, right);
        return Expression.Lambda<T>(compose(left.Body, rightBody), left.Parameters);
    }
}