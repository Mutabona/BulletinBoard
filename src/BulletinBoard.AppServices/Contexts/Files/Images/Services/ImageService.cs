using BulletinBoard.AppServices.Contexts.Files.Images.Repositories;
using BulletinBoard.Contracts.Files.Images;

namespace BulletinBoard.AppServices.Contexts.Files.Images.Services;

///<inheritdoc cref="IImageService"/>
public class ImageService(IImageRepository _repository) : IImageService
{
    /// <inheritdoc />
    public async Task<Guid> AddImageAsync(AddImageRequest request, CancellationToken cancellationToken)
    {
       return await _repository.AddAsync(request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task DeleteImageAsync(Guid imageId, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(imageId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ICollection<ImageDto>> GetByBulletinIdAsync(Guid bulletinId, CancellationToken cancellationToken)
    {
        return await _repository.GetByBulletinIdAsync(bulletinId, cancellationToken); 
    }

    /// <inheritdoc />
    public async Task<ImageDto> GetImageByIdAsync(Guid imageId, CancellationToken cancellationToken)
    {
        return await  _repository.GetByIdAsync(imageId, cancellationToken);
    }
}