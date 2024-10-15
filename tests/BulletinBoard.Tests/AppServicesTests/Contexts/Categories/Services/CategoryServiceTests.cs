using System.Text.Json;
using AutoFixture;
using AutoMapper;
using BulletinBoard.AppServices.Contexts.Categories.Repositories;
using BulletinBoard.AppServices.Contexts.Categories.Services;
using BulletinBoard.AppServices.Exceptions;
using BulletinBoard.ComponentRegistrar.MapProfiles;
using BulletinBoard.Contracts.Categories;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Time.Testing;
using Moq;
using Shouldly;

namespace BulletinBoard.Tests.AppServicesTests.Contexts.Categories.Services;

public class CategoryServiceTests
{
    private readonly IFixture _fixture;
    private readonly Mock<ICategoryRepository> _repositoryMock;
    private readonly Mock<IDistributedCache> _cacheMock;
    private readonly Mock<ILogger<CategoryService>> _loggerMock;
    private readonly IMapper _mapper;
    private readonly CategoryService _categoryService;
    private readonly FakeTimeProvider _timeProvider;
    private readonly CancellationToken _token;
    private const string key = "all_categories";

    public CategoryServiceTests()
    {
        var configurationProvider = new MapperConfiguration(delegate (IMapperConfigurationExpression configure)
        {
            configure.AddProfiles(new List<Profile>
            {
                new CategoryProfile(),
            });
        });
        
        _mapper = configurationProvider.CreateMapper();
        _fixture = new Fixture();
        _repositoryMock = new Mock<ICategoryRepository>();
        _cacheMock = new Mock<IDistributedCache>();
        _loggerMock = new Mock<ILogger<CategoryService>>();
        _timeProvider = new FakeTimeProvider();
        _token = new CancellationTokenSource().Token;
        _categoryService = new CategoryService(_repositoryMock.Object, _cacheMock.Object, _loggerMock.Object, _mapper, _timeProvider);
    }

    [Fact]
    public async Task CreateCategoryAsync_Should_Create_Category()
    {
        // Arrange
        var createCategoryRequest = _fixture.Create<CreateCategoryRequest>();
        _timeProvider.SetUtcNow(DateTime.UtcNow);
        var now = _timeProvider.GetUtcNow().UtcDateTime;

        _repositoryMock.Setup(x => x.AddCategoryAsync(It.IsAny<CategoryDto>(), _token)).ReturnsAsync(It.IsAny<Guid>());
        _cacheMock.Setup(x => x.RemoveAsync(key, _token)).Returns(Task.CompletedTask);

        // Act
        var result = await _categoryService.CreateCategoryAsync(createCategoryRequest, _token);

        // Assert
        result.ShouldBe(It.IsAny<Guid>());
        _repositoryMock.Verify(x => x.AddCategoryAsync(It.Is<CategoryDto>(c => c.Name == createCategoryRequest.Name), _token), Times.Once);
        _repositoryMock.Verify(x => x.AddCategoryAsync(It.Is<CategoryDto>(c => c.CreatedAt == now), _token), Times.Once);
        _cacheMock.Verify(x => x.RemoveAsync(key, _token), Times.Once);
    }

    [Fact]
    public async Task GetCategoryByIdAsync_Should_Return_Category()
    {
        // Arrange
        var categoryId = _fixture.Create<Guid>();
        var category = _fixture.Create<CategoryDto>();
        _repositoryMock.Setup(x => x.GetByIdAsync(categoryId, _token)).ReturnsAsync(category);

        // Act
        var result = await _categoryService.GetCategoryByIdAsync(categoryId, _token);

        // Assert
        result.ShouldBe(category);
    }

    [Fact]
    public async Task DeleteCategoryAsync_Should_Delete_Category()
    {
        // Arrange
        var categoryId = _fixture.Create<Guid>();

        // Act
        await _categoryService.DeleteCategoryAsync(categoryId, _token);

        // Assert
        _repositoryMock.Verify(x => x.DeleteAsync(categoryId, _token), Times.Once);
        _cacheMock.Verify(x => x.RemoveAsync(key, _token), Times.Once);
    }

    [Fact]
    public async Task GetCategoryWithSubcategoriesAsync_Should_Return_Categories_With_Subcategories()
    {
        // Arrange
        var categoryId = _fixture.Create<Guid>();
        var categories = _fixture.Create<ICollection<CategoryDto>>();
        _repositoryMock.Setup(x => x.GetCategoryWithSubcategoriesAsync(categoryId, _token)).ReturnsAsync(categories);

        // Act
        var result = await _categoryService.GetCategoryWithSubcategoriesAsync(categoryId, _token);

        // Assert
        result.ShouldBe(categories);
    }

    // [Fact]
    // public async Task GetAllCategoriesAsync_Should_Return_Categories_From_Cache()
    // {
    //     // Arrange
    //     var categories = _fixture.Create<ICollection<CategoryDto>>();
    //     var serializedCategories = JsonSerializer.Serialize(categories);
    //     _cacheMock.Setup(x => x.GetStringAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(serializedCategories);
    //
    //     // Act
    //     var result = await _categoryService.GetAllCategoriesAsync(_token);
    //
    //     // Assert
    //     result.ShouldBe(categories);
    //     _cacheMock.Verify(x => x.GetStringAsync(key, _token), Times.Once);
    //     _repositoryMock.Verify(x => x.GetAllCategoriesAsync(_token), Times.Never);
    // }
    
    [Fact]
    public async Task GetAllCategoriesAsync_Should_Return_Categories_From_Repository()
    {
        // Arrange
        var categories = _fixture.Create<ICollection<CategoryDto>>();
        //_cacheMock.Setup(x => x.GetStringAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync((string)null);
        _repositoryMock.Setup(x => x.GetAllCategoriesAsync(_token))
            .ReturnsAsync(categories);
    
        // Act
        var result = await _categoryService.GetAllCategoriesAsync(_token);
    
        // Assert
        result.ShouldBe(categories);
        _repositoryMock.Verify(x => x.GetAllCategoriesAsync(_token), Times.Once);
        var serializedCategories = JsonSerializer.Serialize(categories);
        /*_cacheMock.Verify(
            x => x.SetStringAsync(key, serializedCategories, It.IsAny<DistributedCacheEntryOptions>(), _token),
            Times.Once);*/
    }

    [Fact]
    public async Task IsCategoryExistsAsync_Should_Return_True_When_Category_Exists()
    {
        // Arrange
        var categoryId = _fixture.Create<Guid>();
        var category = _fixture.Create<CategoryDto>();
        _repositoryMock.Setup(x => x.GetByIdAsync(categoryId, _token)).ReturnsAsync(category);

        // Act
        var result = await _categoryService.IsCategoryExistsAsync(categoryId, _token);

        // Assert
        result.ShouldBeTrue();
    }

    [Fact]
    public async Task IsCategoryExistsAsync_Should_Return_False_When_Category_Does_Not_Exist()
    {
        // Arrange
        var categoryId = _fixture.Create<Guid>();
        _repositoryMock.Setup(x => x.GetByIdAsync(categoryId, _token)).ThrowsAsync(new EntityNotFoundException());

        // Act
        var result = await _categoryService.IsCategoryExistsAsync(categoryId, _token);

        // Assert
    }
}
