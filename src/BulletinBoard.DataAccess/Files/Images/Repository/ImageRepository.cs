using AutoMapper;
using AutoMapper.QueryableExtensions;
using BulletinBoard.AppServices.Contexts.Files.Images.Repositories;
using BulletinBoard.Contracts.Files.Images;
using BulletinBoard.Domain.Files.Images.Entity;
using BulletinBoard.Infrastructure.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoard.DataAccess.Files.Images.Repository;

///<inheritdoc cref="IImageRepository"/>
public class ImageRepository : IImageRepository
{
    private readonly IRepository<Image> _repository;
    private readonly IMapper _mapper;
    
    /// <summary>
    /// Создаёт экземпляр <see cref="ImageRepository"/>.
    /// </summary>
    /// <param name="repository">Репозиторий.</param>
    /// <param name="mapper">Маппер.</param>
    public ImageRepository(IRepository<Image> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    ///<inheritdoc/>
    public async Task<Guid> AddAsync(Guid bulletinId, IFormFile image, CancellationToken cancellationToken)
    {
        var imageEntity = _mapper.Map<Image>(image);
        imageEntity.BulletinId = bulletinId;
        return await _repository.AddAsync(imageEntity, cancellationToken);
    }

    ///<inheritdoc/>
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(id, cancellationToken);
    }

    ///<inheritdoc/>
    public async Task<ICollection<Guid>> GetImageIdByBulletinIdAsync(Guid bulletinId, CancellationToken cancellationToken)
    {
        return await _repository.GetAll().Where(i => i.BulletinId == bulletinId).Select(i => i.Id).ToListAsync(cancellationToken);
    }

    ///<inheritdoc/>
    public async Task<ImageDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var image = await _repository.GetByIdAsync(id, cancellationToken);
        return _mapper.Map<ImageDto>(image);
    }
}