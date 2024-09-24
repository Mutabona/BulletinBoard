using AutoMapper;
using AutoMapper.QueryableExtensions;
using BulletinBoard.AppServices.Contexts.Bulletins.Repositories;
using BulletinBoard.AppServices.Specifications;
using BulletinBoard.Contracts.Bulletins;
using BulletinBoard.Domain.Bulletins.Entity;
using BulletinBoard.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoard.DataAccess.Bulletins.Repository;

///<inheritdoc cref="IBulletinRepository"/>
public class BulletinRepository : IBulletinRepository
{
    private readonly IRepository<Bulletin> _repository;
    private readonly IMapper _mapper;
    
    /// <summary>
    /// Инициализирует экземпляр <see cref="BulletinRepository"/>.
    /// </summary>
    public BulletinRepository(IRepository<Bulletin> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    ///<inheritdoc/>
    public async Task<ICollection<BulletinDto>> GetBySpecificationWithPaginationAsync(ISpecification<Bulletin> specification, int take, int? skip, CancellationToken cancellationToken)
    {
        var query = _repository
            .GetAll()
            .OrderBy(bulletin => bulletin.Id)
            .Where(specification.PredicateExpression);

        if (skip.HasValue)
        {
            query = query.Skip(skip.Value);
        }
            
        return await query
            .Take(take)
            .ProjectTo<BulletinDto>(_mapper.ConfigurationProvider)
            .ToArrayAsync(cancellationToken);
    }

    ///<inheritdoc/>
    public async Task<ICollection<BulletinDto>> GetBySpecificationAsync(ISpecification<Bulletin> specification, CancellationToken cancellationToken)
    {
        return await _repository
            .GetByPredicate(specification.PredicateExpression)
            .ProjectTo<BulletinDto>(_mapper.ConfigurationProvider)
            .ToArrayAsync(cancellationToken);
    }

    ///<inheritdoc/>
    public async Task<BulletinDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _repository
            .GetByPredicate(adv => adv.Id == id)
            .ProjectTo<BulletinDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }

    ///<inheritdoc/>
    public async Task<Guid> CreateAsync(CreateBulletinRequest bulletin, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Bulletin>(bulletin);
        
        await _repository.AddAsync(entity, cancellationToken);

        return entity.Id;
    }

    ///<inheritdoc/>
    public async Task UpdateAsync(BulletinDto bulletin, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Bulletin>(bulletin);
        await _repository.UpdateAsync(entity, cancellationToken);
    }

    ///<inheritdoc/>
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(id, cancellationToken); 
    }
}