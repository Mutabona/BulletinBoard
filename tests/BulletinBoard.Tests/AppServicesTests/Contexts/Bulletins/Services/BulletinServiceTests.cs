using AutoFixture;
using AutoMapper;
using BulletinBoard.AppServices.Contexts.Bulletins.Builders;
using BulletinBoard.AppServices.Contexts.Bulletins.Repositories;
using BulletinBoard.AppServices.Contexts.Bulletins.Services;
using BulletinBoard.AppServices.Contexts.Bulletins.Specifications;
using BulletinBoard.AppServices.Contexts.Categories.Services;
using BulletinBoard.AppServices.Contexts.Users.Services;
using BulletinBoard.AppServices.Exceptions;
using BulletinBoard.ComponentRegistrar.MapProfiles;
using BulletinBoard.Contracts.Bulletins;
using BulletinBoard.Contracts.Categories;
using BulletinBoard.Contracts.Users;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Time.Testing;
using Moq;
using Shouldly;

namespace BulletinBoard.Tests.AppServicesTests.Contexts.Bulletins.Services;

public class BulletinServiceTests
{
    private readonly Mock<IBulletinRepository> _repositoryMock;
    private readonly Mock<IBulletinSpecificationBuilder> _specificationBuilderMock;
    private readonly Mock<ICategoryService> _categoryServiceMock;
    private readonly Mock<ILogger<BulletinService>> _loggerMock;
    private readonly IMapper _mapper;
    private readonly Mock<IUserService> _userServiceMock;
    private readonly FakeTimeProvider _fakeTimeProvider;
    private readonly BulletinService _service;
    private readonly Fixture _fixture;

    public BulletinServiceTests()
    {
        
        var configurationProvider = new MapperConfiguration(delegate (IMapperConfigurationExpression configure)
        {
            configure.AddProfiles(new List<Profile>
            {
                new BulletinProfile(),
            });
        });
    
        _fixture = new Fixture();
        _mapper = configurationProvider.CreateMapper();
        _repositoryMock = new Mock<IBulletinRepository>();
        _specificationBuilderMock = new Mock<IBulletinSpecificationBuilder>();
        _categoryServiceMock = new Mock<ICategoryService>();
        _loggerMock = new Mock<ILogger<BulletinService>>();
        _fakeTimeProvider = new FakeTimeProvider();
        _userServiceMock = new Mock<IUserService>();

        _service = new BulletinService(
            _repositoryMock.Object,
            _specificationBuilderMock.Object,
            _categoryServiceMock.Object,
            _loggerMock.Object,
            _mapper,
            _fakeTimeProvider,
            _userServiceMock.Object);
    }

    [Fact]
    public async Task SearchBulletinsAsync_ShouldReturnBulletins()
    {
        var request = _fixture.Create<SearchBulletinRequest>();
        var bulletins = _fixture.Create<List<BulletinDto>>();
        var specification = _fixture.Create<SearchStringSpecification>(); // Replace with actual specification type

        _specificationBuilderMock.Setup(sb => sb.Build(request)).Returns(specification);
        _repositoryMock.Setup(repo => repo.GetBySpecificationWithPaginationAsync(specification, request.Take, request.Skip, It.IsAny<CancellationToken>()))
                       .ReturnsAsync(bulletins);

        var result = await _service.SearchBulletinsAsync(request, CancellationToken.None);

        result.ShouldBe(bulletins);
    }

    [Fact]
    public async Task GetByCategoryAsync_ShouldReturnBulletins()
    {
        var take = 10;
        var skip = 0;
        var categoryId = _fixture.Create<Guid>();
        var categories = _fixture.CreateMany<CategoryDto>().ToList();
        var categoriesIds = categories.Select(c => c.Id).ToList();
        var bulletins = _fixture.Create<List<BulletinDto>>();
        var specification = _fixture.Create<ByCategorySpecification>(); // Replace with actual specification type

        _categoryServiceMock.Setup(cs => cs.GetCategoryWithSubcategoriesAsync(categoryId, It.IsAny<CancellationToken>())).ReturnsAsync(categories);
        _specificationBuilderMock.Setup(sb => sb.Build(categoriesIds)).Returns(specification);
        _repositoryMock.Setup(repo => repo.GetBySpecificationWithPaginationAsync(specification, take, skip, It.IsAny<CancellationToken>()))
                       .ReturnsAsync(bulletins);

        var result = await _service.GetByCategoryAsync(take, skip, categoryId, CancellationToken.None);

        result.ShouldBe(bulletins);
    }

    [Fact]
    public async Task FindByIdAsync_ShouldReturnBulletin()
    {
        var id = _fixture.Create<Guid>();
        var bulletin = _fixture.Create<BulletinDto>();

        _repositoryMock.Setup(repo => repo.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(bulletin);

        var result = await _service.FindByIdAsync(id, CancellationToken.None);

        result.ShouldBe(bulletin);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnNewBulletinId()
    {
        var ownerId = _fixture.Create<Guid>();
        var request = _fixture.Create<CreateBulletinRequest>();
        var bulletin = _fixture.Build<BulletinDto>()
                              .With(b => b.OwnerId, ownerId)
                              .With(b => b.Id, Guid.NewGuid())
                              .With(b => b.CreatedAt, _fakeTimeProvider.GetUtcNow().UtcDateTime)
                              .Create();
        
        _repositoryMock.Setup(repo => repo.CreateAsync(bulletin, It.IsAny<CancellationToken>())).ReturnsAsync(It.IsAny<Guid>());

        var result = await _service.CreateAsync(ownerId, request, CancellationToken.None);

        result.ShouldBe(It.IsAny<Guid>());
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateBulletin()
    {
        var bulletinId = _fixture.Create<Guid>();
        var userId = _fixture.Create<Guid>();
        var categoryId = _fixture.Create<Guid>();
        var request = _fixture.Build<UpdateBulletinRequest>()
                              .With(r => r.Title, "New Title")
                              .With(r => r.Description, "New Description")
                              .With(r => r.CategoryId, categoryId)
                              .With(r => r.Price, 100)
                              .Create();
        var bulletin = _fixture.Build<BulletinDto>()
                              .With(b => b.Id, bulletinId)
                              .With(b => b.OwnerId, userId)
                              .Create();

        _repositoryMock.Setup(repo => repo.GetByIdAsync(bulletinId, It.IsAny<CancellationToken>())).ReturnsAsync(bulletin);
        _repositoryMock.Setup(repo => repo.UpdateAsync(bulletin, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        await _service.UpdateAsync(bulletinId, userId, request, CancellationToken.None);

        bulletin.Title.ShouldBe(request.Title);
        bulletin.Description.ShouldBe(request.Description);
        bulletin.CategoryId.ShouldBe(request.CategoryId.Value);
        bulletin.Price.ShouldBe(request.Price.Value);
    }
    
    [Fact]
    public void UpdateAsync_With_NotOwnerUser_Should_Throw_ForbiddenException()
    {
        var bulletinId = _fixture.Create<Guid>();
        var userId = _fixture.Create<Guid>();
        var categoryId = _fixture.Create<Guid>();
        var request = _fixture.Build<UpdateBulletinRequest>()
            .With(r => r.Title, "New Title")
            .With(r => r.Description, "New Description")
            .With(r => r.CategoryId, categoryId)
            .With(r => r.Price, 100)
            .Create();
        var bulletin = _fixture.Build<BulletinDto>()
            .With(b => b.Id, bulletinId)
            .With(b => b.OwnerId, Guid.NewGuid())
            .Create();

        _repositoryMock.Setup(repo => repo.GetByIdAsync(bulletinId, It.IsAny<CancellationToken>())).ReturnsAsync(bulletin);

        Should.Throw<ForbiddenException>(() => _service.UpdateAsync(bulletinId, userId, request, CancellationToken.None));
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveBulletin()
    {
        var bulletinId = _fixture.Create<Guid>();
        var userId = _fixture.Create<Guid>();
        var bulletin = _fixture.Build<BulletinDto>()
                              .With(b => b.Id, bulletinId)
                              .With(b => b.OwnerId, userId)
                              .Create();
        var user = _fixture.Build<UserDto>()
                           .With(u => u.Id, userId)
                           .With(u => u.Role, "Admin")
                           .Create();

        _repositoryMock.Setup(repo => repo.GetByIdAsync(bulletinId, It.IsAny<CancellationToken>())).ReturnsAsync(bulletin);
        _userServiceMock.Setup(us => us.GetUserByIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(user);
        _repositoryMock.Setup(repo => repo.DeleteAsync(bulletinId, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        await _service.DeleteAsync(bulletinId, userId, CancellationToken.None);

        _repositoryMock.Verify(repo => repo.DeleteAsync(bulletinId, It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task DeleteAsync_WithNoOwnerUser_Should_Throw_ForbiddenException()
    {
        var bulletinId = _fixture.Create<Guid>();
        var userId = _fixture.Create<Guid>();
        var bulletin = _fixture.Build<BulletinDto>()
            .With(b => b.Id, bulletinId)
            .With(b => b.OwnerId, userId)
            .Create();
        var user = _fixture.Build<UserDto>()
            .With(u => u.Id, Guid.NewGuid())
            .With(u => u.Role, "User")
            .Create();

        _repositoryMock.Setup(repo => repo.GetByIdAsync(bulletinId, It.IsAny<CancellationToken>())).ReturnsAsync(bulletin);
        _userServiceMock.Setup(us => us.GetUserByIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(user);

        Should.Throw<ForbiddenException>(() => _service.DeleteAsync(bulletinId, userId, CancellationToken.None));
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllBulletins()
    {
        var bulletins = _fixture.Create<List<BulletinDto>>();

        _repositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(bulletins);

        var result = await _service.GetAllAsync(CancellationToken.None);

        result.ShouldBe(bulletins);
    }

    [Fact]
    public async Task IsUserBulletinsOwnerAsync_ShouldReturnTrue()
    {
        var bulletinId = _fixture.Create<Guid>();
        var userId = _fixture.Create<Guid>();
        var bulletin = _fixture.Build<BulletinDto>()
                              .With(b => b.Id, bulletinId)
                              .With(b => b.OwnerId, userId)
                              .Create();

        _repositoryMock.Setup(repo => repo.GetByIdAsync(bulletinId, It.IsAny<CancellationToken>())).ReturnsAsync(bulletin);

        var result = await _service.IsUserBulletinsOwnerAsync(bulletinId, userId, CancellationToken.None);

        result.ShouldBeTrue();
    }

    [Fact]
    public async Task IsUserBulletinsOwnerAsync_ShouldReturnFalse()
    {
        var bulletinId = _fixture.Create<Guid>();
        var userId = _fixture.Create<Guid>();
        var bulletin = _fixture.Build<BulletinDto>()
                              .With(b => b.Id, bulletinId)
                              .With(b => b.OwnerId, Guid.NewGuid())
                              .Create();

        _repositoryMock.Setup(repo => repo.GetByIdAsync(bulletinId, It.IsAny<CancellationToken>())).ReturnsAsync(bulletin);

        var result = await _service.IsUserBulletinsOwnerAsync(bulletinId, userId, CancellationToken.None);

        result.ShouldBeFalse();
    }
}