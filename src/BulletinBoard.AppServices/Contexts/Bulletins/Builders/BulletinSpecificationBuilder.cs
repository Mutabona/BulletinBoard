using BulletinBoard.AppServices.Contexts.Bulletins.Specifications;
using BulletinBoard.AppServices.Specifications;
using BulletinBoard.Contracts.Bulletins;
using BulletinBoard.Domain.Bulletins.Entity;

namespace BulletinBoard.AppServices.Contexts.Bulletins.Builders;

/// <inheritdoc />
public class BulletinSpecificationBuilder : IBulletinSpecificationBuilder
{
    /// <inheritdoc />
    public ISpecification<Bulletin> Build(SearchBulletinRequest request)
    {
        return new SearchStringSpecification(request.Search);
    }

    /// <inheritdoc />
    public ISpecification<Bulletin> Build(ICollection<Guid> categoriesIds)
    {
        return new ByCategorySpecification(categoriesIds);
    }
}