using System.Linq.Expressions;
using BulletinBoard.AppServices.Specifications;
using BulletinBoard.Domain.Bulletins.Entity;

namespace BulletinBoard.AppServices.Contexts.Bulletins.Specifications;

/// <summary>
/// Спецификация отсечения по максимальной цене.
/// </summary>
public class MinPriceSpecification : Specification<Bulletin>
{
    private readonly decimal _minPrice;
    
    /// <summary>
    /// Создаёт спецификацию отсечения по минимальной цене <see cref="MinPriceSpecification"/>.
    /// </summary>
    /// <param name="minPrice">Минимальная цена.</param>
    public MinPriceSpecification(decimal minPrice)
    {
        _minPrice = minPrice;
    }
    
    /// <inheritdoc />
    public override Expression<Func<Bulletin, bool>> PredicateExpression => 
        bulletin => bulletin.Price >= _minPrice;
}