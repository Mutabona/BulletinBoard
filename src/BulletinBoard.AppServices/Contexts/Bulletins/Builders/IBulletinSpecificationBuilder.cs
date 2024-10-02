using BulletinBoard.AppServices.Specifications;
using BulletinBoard.Contracts.Bulletins;
using BulletinBoard.Domain.Bulletins.Entity;

namespace BulletinBoard.AppServices.Contexts.Bulletins.Builders;

/// <summary>
/// Строит спецификации для объявлений.
/// </summary>
public interface IBulletinSpecificationBuilder
{
    /// <summary>
    /// Строит спецификацию по запросу.
    /// </summary>
    /// <param name="request">Запрос.</param>
    /// <returns>Спецификация.</returns>
    ISpecification<Bulletin> Build(SearchBulletinRequest request);
    
    /// <summary>
    /// Строит спецификацию по категории.
    /// </summary>
    /// <param name="categoriesIds">Идентификаторы категорий.</param>
    /// <returns>Спецификация.</returns>
    ISpecification<Bulletin> Build(ICollection<Guid?> categoriesIds);
}