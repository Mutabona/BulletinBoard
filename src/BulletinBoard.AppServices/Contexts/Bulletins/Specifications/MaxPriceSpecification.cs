using System.Linq.Expressions;
using BulletinBoard.AppServices.Specifications;
using BulletinBoard.Domain.Bulletins.Entity;

namespace BulletinBoard.AppServices.Contexts.Bulletins.Specifications;

/// <summary>
/// Спецификация отсечения по максимальной цене.
/// </summary>
public class MaxPriceSpecification : Specification<Bulletin>
{
    private readonly decimal _maxPrice;
        
    /// <summary>
    /// Создаёт спецификацию отсечения по максимальной цене <see cref="MaxPriceSpecification"/>.
    /// </summary>
    /// <param name="maxPrice">Максимальная цена.</param>
    public MaxPriceSpecification(decimal maxPrice)
    {
        _maxPrice = maxPrice;
    }

    /// <inheritdoc />
    public override Expression<Func<Bulletin, bool>> PredicateExpression => 
        bulletin => bulletin.Price <= _maxPrice;
}