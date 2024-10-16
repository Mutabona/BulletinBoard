﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using BulletinBoard.AppServices.Contexts.Bulletins.Repositories;
using BulletinBoard.AppServices.Exceptions;
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
    /// Создаёт экземпляр <see cref="BulletinRepository"/>.
    /// </summary>
    /// <param name="repository">Репозиторий.</param>
    /// <param name="mapper">Маппер.</param>
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
            .Include(c => c.Owner)
            .Include(c => c.Category)
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
            .GetAll()
            .Include(c => c.Owner)
            .Include(c => c.Category)
            .Where(specification.PredicateExpression)
            .ProjectTo<BulletinDto>(_mapper.ConfigurationProvider)
            .ToArrayAsync(cancellationToken);
    }

    ///<inheritdoc/>
    public async Task<BulletinDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var bulletin = await _repository
            .GetAll()
            .Where(bulletin => bulletin.Id == id)
            .Include(c => c.Owner)
            .Include(c => c.Category)
            .ProjectTo<BulletinDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (bulletin == null) throw new EntityNotFoundException();
        return bulletin;
    }

    ///<inheritdoc/>
    public async Task<Guid> CreateAsync(BulletinDto bulletin,
        CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Bulletin>(bulletin);

        return await _repository.AddAsync(entity, cancellationToken);
    }

    ///<inheritdoc/>
    public async Task UpdateAsync(BulletinDto bulletinDto, CancellationToken cancellationToken)
    {
        var bulletin = _mapper.Map<Bulletin>(bulletinDto);
        await _repository.UpdateAsync(bulletin, cancellationToken);
    }

    ///<inheritdoc/>
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(id, cancellationToken); 
    }

    ///<inheritdoc/>
    public async Task<ICollection<BulletinDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _repository.GetAll().Include(b => b.Owner).Include(b => b.Category).ProjectTo<BulletinDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);
    }
}