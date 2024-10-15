using AutoMapper;
using BulletinBoard.AppServices.Contexts.Bulletins.Services;
using BulletinBoard.AppServices.Contexts.Comments.Repositories;
using BulletinBoard.AppServices.Contexts.Comments.Services;
using BulletinBoard.Contracts.Comments;
using BulletinBoard.Contracts.Emails;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using AutoFixture;
using BulletinBoard.ComponentRegistrar.MapProfiles;
using BulletinBoard.Contracts.Bulletins;
using MassTransit.Transports;
using Microsoft.Extensions.Time.Testing;

namespace BulletinBoard.Tests.AppServicesTests.Contexts.Comments.Services;



public class CommentServiceTests
{
    private readonly IFixture _fixture;
    private readonly Mock<ICommentRepository> _repositoryMock;
    private readonly Mock<ILogger<CommentService>> _loggerMock;
    private readonly CancellationToken _token;
    private readonly IMapper _mapper;
    private readonly Mock<IPublishEndpoint> _publishEndpointMock;
    private readonly Mock<IBulletinService> _bulletinServiceMock;
    private readonly CommentService _commentService;
    private readonly FakeTimeProvider _fakeTimeProvider;

    public CommentServiceTests()
    {
        
        var configurationProvider = new MapperConfiguration(delegate (IMapperConfigurationExpression configure)
        {
            configure.AddProfiles(new List<Profile>
            {
                new CommentProfile(),
            });
        });
        
        _fixture = new Fixture();
        _repositoryMock = new Mock<ICommentRepository>();
        _loggerMock = new Mock<ILogger<CommentService>>();
        _mapper = configurationProvider.CreateMapper();
        _publishEndpointMock = new Mock<IPublishEndpoint>();
        _bulletinServiceMock = new Mock<IBulletinService>();
        _fakeTimeProvider = new FakeTimeProvider();
        _commentService = new CommentService(_repositoryMock.Object, _loggerMock.Object, _mapper,
            _publishEndpointMock.Object, _fakeTimeProvider, _bulletinServiceMock.Object);
        
        _token = new CancellationTokenSource().Token;
    }

    [Fact]
    public async Task AddCommentAsync_Should_Call_RepositoryMethod_And_PublishEndpoint_Async()
    {
        // Arrange
        var bulletinId = _fixture.Create<Guid>();
        var authorId = _fixture.Create<Guid>();
        var request = _fixture.Create<AddCommentRequest>();
        _fakeTimeProvider.SetUtcNow(DateTime.UtcNow);
        var createdAt = _fakeTimeProvider.GetUtcNow().UtcDateTime;
        _bulletinServiceMock.Setup(x => x.FindByIdAsync(bulletinId, _token))
            .ReturnsAsync(new BulletinDto());
        _repositoryMock.Setup(x => x.AddCommentAsync(It.IsAny<CommentDto>(), _token))
            .ReturnsAsync(It.IsAny<Guid>());

        // Act
        var result = await _commentService.AddCommentAsync(bulletinId, authorId, request, _token);

        // Assert
        result.ShouldBe(It.IsAny<Guid>());
        _repositoryMock.Verify(x => x.AddCommentAsync(It.Is<CommentDto>(c => c.BulletinId == bulletinId), _token),
            Times.Once);
        _repositoryMock.Verify(x => x.AddCommentAsync(It.Is<CommentDto>(c => c.AuthorId == authorId), _token),
            Times.Once);
        _repositoryMock.Verify(x => x.AddCommentAsync(It.Is<CommentDto>(c => c.CreatedAt == createdAt), _token),
            Times.Once);
        _publishEndpointMock.Verify(x => x.Publish(It.IsAny<CommentAdded>(), _token),
            Times.Once);
    }

    [Fact]
    public async Task DeleteCommentAsync_Should_Call_RepositoryMethod_Async()
    {
        // Arrange
        var commentId = _fixture.Create<Guid>();

        // Act
        await _commentService.DeleteCommentAsync(commentId, _token);

        // Assert
        _repositoryMock.Verify(x => x.DeleteCommentAsync(commentId, _token), Times.Once);
    }

    [Fact]
    public async Task GetByBulletinIdAsync_Should_Return_Comments()
    {
        // Arrange
        var bulletinId = _fixture.Create<Guid>();
        var comments = _fixture.Create<ICollection<CommentDto>>();
        _bulletinServiceMock.Setup(x => x.FindByIdAsync(bulletinId, _token))
            .ReturnsAsync(new BulletinDto());
        _repositoryMock.Setup(x => x.GetCommentsByBulletinIdAsync(bulletinId, _token))
            .ReturnsAsync(comments);

        // Act
        var result = await _commentService.GetByBulletinIdAsync(bulletinId, _token);

        // Assert
        _bulletinServiceMock.Verify(x => x.FindByIdAsync(bulletinId, _token), Times.Once);
        _repositoryMock.Verify(x => x.GetCommentsByBulletinIdAsync(bulletinId, _token), Times.Once);
        result.ShouldBe(comments);
    }

    [Fact]
    public async Task GetCommentByIdAsync_Should_Return_Comment()
    {
        // Arrange
        var commentId = _fixture.Create<Guid>();
        var comment = _fixture.Create<CommentDto>();
        _repositoryMock.Setup(x => x.GetCommentByIdAsync(commentId, _token))
            .ReturnsAsync(comment);

        // Act
        var result = await _commentService.GetCommentByIdAsync(commentId, _token);

        // Assert
        _repositoryMock.Verify(x => x.GetCommentByIdAsync(commentId, _token), Times.Once);
        result.ShouldBe(comment);
    }
}
