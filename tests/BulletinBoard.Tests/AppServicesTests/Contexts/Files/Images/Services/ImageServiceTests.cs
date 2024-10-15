using AutoFixture;
using AutoMapper;
using BulletinBoard.AppServices.Contexts.Bulletins.Services;
using BulletinBoard.AppServices.Contexts.Files.Images.Repositories;
using BulletinBoard.AppServices.Contexts.Files.Images.Services;
using BulletinBoard.AppServices.Contexts.Users.Services;
using BulletinBoard.AppServices.Exceptions;
using BulletinBoard.ComponentRegistrar.MapProfiles;
using BulletinBoard.Contracts.Bulletins;
using BulletinBoard.Contracts.Files.Images;
using BulletinBoard.Contracts.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Time.Testing;
using Moq;
using Shouldly;

namespace BulletinBoard.Tests.AppServicesTests.Contexts.Files.Images.Services;

public class ImageServiceTests
{
    private readonly IFixture _fixture;
    private readonly Mock<IImageRepository> _repositoryMock;
    private readonly Mock<ILogger<ImageService>> _loggerMock;
    private readonly IMapper _mapper;
    private readonly Mock<IBulletinService> _bulletinServiceMock;
    private readonly ImageService _imageService;
    private readonly FakeTimeProvider _timeProvider;
    private readonly CancellationToken _token;

    public ImageServiceTests()
    {
        var configurationProvider = new MapperConfiguration(delegate (IMapperConfigurationExpression configure)
        {
            configure.AddProfiles(new List<Profile>
            {
                new ImageProfile(),
            });
        });
        
        _mapper = configurationProvider.CreateMapper();
        _repositoryMock = new Mock<IImageRepository>();
        _loggerMock = new Mock<ILogger<ImageService>>();
        _timeProvider = new FakeTimeProvider();
        _bulletinServiceMock = new Mock<IBulletinService>();
        _imageService = new ImageService(_repositoryMock.Object, _loggerMock.Object, _mapper, _timeProvider, _bulletinServiceMock.Object);
        
        _token = new CancellationTokenSource().Token;
        _fixture = new Fixture();
    }

    [Fact]
    public async Task AddImageAsync_Should_Ensure_User_Is_BulletinOwner_And_Call_RepositoryMethod_Async()
    {
        // Arrange
        var bulletinId = _fixture.Create<Guid>();
        var userId = _fixture.Create<Guid>();
        
        var content = _fixture.Create<byte[]>();
        var contentType = _fixture.Create<string>();
        var length = content.Length;
        var stream = new MemoryStream(content);
        IFormFile image = new FormFile(stream, 0, length, "test", contentType);
            
        var bulletin = _fixture.Build<BulletinDto>().With(b => b.OwnerId, userId).Create();
        var user = _fixture.Build<UserDto>().With(u => u.Id, userId).Create();
        _timeProvider.SetUtcNow(DateTime.UtcNow);
        var utcNow = _timeProvider.GetUtcNow();
        
        _bulletinServiceMock.Setup(x => x.IsUserBulletinsOwnerAsync(bulletinId, userId, _token)).ReturnsAsync(true);
        _repositoryMock.Setup(x => x.AddAsync(It.IsAny<ImageDto>(), _token)).ReturnsAsync(It.IsAny<Guid>());

        // Act
        var result = await _imageService.AddImageAsync(bulletinId, userId, image, _token);

        // Assert
        result.ShouldBe(It.IsAny<Guid>());
        _repositoryMock.Verify(x => x.AddAsync(It.IsAny<ImageDto>(), _token), Times.Once);
        _bulletinServiceMock.Verify(x => x.IsUserBulletinsOwnerAsync(bulletinId, userId, _token), Times.Once);
    }

    [Fact]
    public async Task DeleteImageAsync_Should_Ensure_User_Is_BulletinOwner_And_Call_RepositoryMethod_Async()
    {
        // Arrange
        var imageId = _fixture.Create<Guid>();
        var userId = _fixture.Create<Guid>();
        var bulletin = _fixture.Build<BulletinDto>().With(b => b.OwnerId, userId).Create();
        var image = _fixture.Build<ImageDto>().With(i => i.BulletinId, bulletin.Id).Create();

        _repositoryMock.Setup(x => x.GetByIdAsync(imageId, _token)).ReturnsAsync(image);
        _bulletinServiceMock.Setup(x => x.FindByIdAsync(image.BulletinId, _token)).ReturnsAsync(bulletin);
        _bulletinServiceMock.Setup(x => x.IsUserBulletinsOwnerAsync(bulletin.Id, userId, _token)).ReturnsAsync(true);

        // Act
        await _imageService.DeleteImageAsync(imageId, userId, _token);

        // Assert
        _repositoryMock.Verify(x => x.DeleteAsync(imageId, _token), Times.Once);
        _bulletinServiceMock.Verify(x => x.IsUserBulletinsOwnerAsync(bulletin.Id, userId, _token), Times.Once);
        _bulletinServiceMock.Verify(x => x.FindByIdAsync(bulletin.Id, _token), Times.Once);
    }

    [Fact]
    public async Task GetImageIdsByBulletinIdAsync_Should_Return_Images()
    {
        // Arrange
        var bulletinId = _fixture.Create<Guid>();
        var imageIds = _fixture.Create<ICollection<Guid>>();

        _bulletinServiceMock.Setup(x => x.FindByIdAsync(bulletinId, _token)).ReturnsAsync(new BulletinDto());
        _repositoryMock.Setup(x => x.GetImageIdByBulletinIdAsync(bulletinId, _token)).ReturnsAsync(imageIds);

        // Act
        var result = await _imageService.GetImageIdsByBulletinIdAsync(bulletinId, _token);

        // Assert
        result.ShouldBe(imageIds);
    }

    [Fact]
    public async Task GetImageByIdAsync_Should_Return_Image()
    {
        // Arrange
        var imageId = _fixture.Create<Guid>();
        var image = _fixture.Create<ImageDto>();

        _repositoryMock.Setup(x => x.GetByIdAsync(imageId, _token)).ReturnsAsync(image);

        // Act
        var result = await _imageService.GetImageByIdAsync(imageId, _token);

        // Assert
        result.ShouldBe(image);
    }
    
    [Fact]
    public async Task DeleteImageAsync_With_User_IsNot_BulletinOwner_Should_Throw_ForbiddenException_Async()
    {
        // Arrange
        var imageId = _fixture.Create<Guid>();
        var userId = _fixture.Create<Guid>();
        var bulletin = _fixture.Build<BulletinDto>().With(b => b.OwnerId, userId).Create();
        var image = _fixture.Build<ImageDto>().With(i => i.BulletinId, bulletin.Id).Create();

        _repositoryMock.Setup(x => x.GetByIdAsync(imageId, _token)).ReturnsAsync(image);
        _bulletinServiceMock.Setup(x => x.FindByIdAsync(image.BulletinId, _token)).ReturnsAsync(bulletin);
        _bulletinServiceMock.Setup(x => x.IsUserBulletinsOwnerAsync(bulletin.Id, userId, _token)).ReturnsAsync(false);

        // Act
        Should.Throw<ForbiddenException>(() => _imageService.DeleteImageAsync(imageId, userId, _token));

        // Assert
        _bulletinServiceMock.Verify(x => x.IsUserBulletinsOwnerAsync(bulletin.Id, userId, _token), Times.Once);
        _bulletinServiceMock.Verify(x => x.FindByIdAsync(bulletin.Id, _token), Times.Once);
    }
    
    [Fact]
    public async Task AddImageAsync_With_User_IsNot_BulletinOwner_Should_Throw_ForbiddenException_Async()
    {
        // Arrange
        var bulletinId = _fixture.Create<Guid>();
        var userId = _fixture.Create<Guid>();
        
        var content = _fixture.Create<byte[]>();
        var contentType = _fixture.Create<string>();
        var length = content.Length;
        var stream = new MemoryStream(content);
        IFormFile image = new FormFile(stream, 0, length, "test", contentType);
            
        var bulletin = _fixture.Build<BulletinDto>().With(b => b.OwnerId, userId).Create();
        var user = _fixture.Build<UserDto>().With(u => u.Id, userId).Create();
        _timeProvider.SetUtcNow(DateTime.UtcNow);
        var utcNow = _timeProvider.GetUtcNow();
        
        _bulletinServiceMock.Setup(x => x.IsUserBulletinsOwnerAsync(bulletinId, userId, _token)).ReturnsAsync(false);
        _repositoryMock.Setup(x => x.AddAsync(It.IsAny<ImageDto>(), _token)).ReturnsAsync(It.IsAny<Guid>());

        // Act
        Should.Throw<ForbiddenException>(() => _imageService.AddImageAsync(bulletinId, userId, image, _token));

        // Assert
        _bulletinServiceMock.Verify(x => x.IsUserBulletinsOwnerAsync(bulletinId, userId, _token), Times.Once);
    }
}