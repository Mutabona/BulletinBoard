using BulletinBoard.AppServices.Contexts.Files.Images.Repositories;
using BulletinBoard.Contracts.Files.Images;
using Microsoft.AspNetCore.Http;

namespace BulletinBoard.AppServices.Contexts.Files.Images.Services;

///<inheritdoc cref="IImageService"/>
public class ImageService : IImageService
{
    private readonly IImageRepository _repository;

    public ImageService(IImageRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> AddImageAsync(Guid bulletinId, IFormFile image, CancellationToken cancellationToken)
    {
        return await _repository.AddAsync(bulletinId, image, cancellationToken);
    }

    /// <inheritdoc />
    public async Task DeleteImageAsync(Guid imageId, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(imageId, cancellationToken);
    }

    public async Task<ICollection<Guid>> GetImageIdsByBulletinIdAsync(Guid bulletinId, CancellationToken cancellationToken)
    {
        return await _repository.GetImageIdByBulletinIdAsync(bulletinId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ImageDto> GetImageByIdAsync(Guid imageId, CancellationToken cancellationToken)
    {
        return await  _repository.GetByIdAsync(imageId, cancellationToken);
    }
}