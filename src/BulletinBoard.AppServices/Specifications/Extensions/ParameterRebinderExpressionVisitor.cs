using System.Linq.Expressions;

namespace BulletinBoard.AppServices.Specifications.Extensions;

/// <summary>
/// Класс предназначен для замены именованных параметров одного выражения на параметры другого.
/// </summary>
public sealed class ParameterRebinderExpressionVisitor : ExpressionVisitor
{
    private readonly IReadOnlyDictionary<ParameterExpression, ParameterExpression> _parametersMap;
    private ParameterRebinderExpressionVisitor(IReadOnlyDictionary<ParameterExpression, ParameterExpression> parametersMap)
    {
        _parametersMap = parametersMap;
    }
    /// <summary>
    /// Выполняет замену параметров второго выражения на параметры первого.
    /// </summary>
    /// <param name="left">Первое выражение.</param>
    /// <param name="right">Второе выражение.</param>
    public static Expression RebindParameters<T>(Expression<T> left, Expression<T> right)
    {
        var parameterMap = left.Parameters
            .Select((expr, index) => new { LeftParameter = expr, RightParameter = right.Parameters[index] })
            .ToDictionary(d => d.RightParameter, d => d.LeftParameter);
            
        var rebinder = new ParameterRebinderExpressionVisitor(parameterMap);
        return rebinder.Visit(right.Body);
    }
    /// <inheritdoc />
    protected override Expression VisitParameter(ParameterExpression node)
    {
        if (_parametersMap.TryGetValue(node, out var replacement))
            node = replacement;
        return base.VisitParameter(node);
    }
}