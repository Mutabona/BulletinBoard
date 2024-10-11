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
        var specification = Specification<Bulletin>.FromPredicate(bulletin => bulletin.OwnerId != Guid.Empty);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            specification = specification.And(new SearchStringSpecification(request.Search));
        }

        if (request.MinPrice.HasValue)
        {
            specification = specification.And(new MinPriceSpecification(request.MinPrice.Value));
        }

        if (request.MaxPrice.HasValue)
        {
            specification = specification.And(new MaxPriceSpecification(request.MaxPrice.Value));
        }

        if (request.UserId.HasValue)
        {
            specification = specification.And(new ByUserSpecification(request.UserId.Value));
        }
        return specification;
    }

    /// <inheritdoc />
    public ISpecification<Bulletin> Build(ICollection<Guid?> categoriesIds)
    {
        return new ByCategorySpecification(categoriesIds);
    }
}