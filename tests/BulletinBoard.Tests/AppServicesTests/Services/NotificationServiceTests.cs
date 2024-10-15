using AutoFixture;
using BulletinBoard.AppServices.Contexts.Bulletins.Services;
using BulletinBoard.AppServices.Contexts.Comments.Services;
using BulletinBoard.AppServices.Contexts.Users.Services;
using BulletinBoard.AppServices.Services;
using BulletinBoard.Contracts.Bulletins;
using BulletinBoard.Contracts.Comments;
using BulletinBoard.Contracts.Emails;
using BulletinBoard.Contracts.Users;
using MassTransit;
using Moq;

namespace BulletinBoard.Tests.AppServicesTests.Services;

public class NotificationServiceTests
{
    private readonly Fixture _fixture;
    private readonly Mock<IUserService> _userServiceMock;
    private readonly Mock<IBulletinService> _bulletinServiceMock;
    private readonly Mock<ICommentService> _commentServiceMock;
    private readonly Mock<IPublishEndpoint> _publishEndpointMock;
    private readonly NotificationService _service;

    public NotificationServiceTests()
    {
        _fixture = new Fixture();
        _userServiceMock = new Mock<IUserService>();
        _bulletinServiceMock = new Mock<IBulletinService>();
        _commentServiceMock = new Mock<ICommentService>();
        _publishEndpointMock = new Mock<IPublishEndpoint>();

        _service = new NotificationService(
            _userServiceMock.Object,
            _bulletinServiceMock.Object,
            _commentServiceMock.Object,
            _publishEndpointMock.Object);
    }

    [Fact]
    public async Task ConsumeCommentAdded_ShouldPublishEmail()
    {
        var commentAdded = _fixture.Create<CommentAdded>();
        var comment = _fixture.Build<CommentDto>()
            .With(c => c.Id, commentAdded.Id)
            .Create();
        var bulletin = _fixture.Build<BulletinDto>()
            .With(b => b.Id, comment.BulletinId)
            .Create();
        var bulletinOwner = _fixture.Build<UserDto>()
            .With(u => u.Id, bulletin.OwnerId)
            .Create();

        _commentServiceMock.Setup(cs => cs.GetCommentByIdAsync(commentAdded.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(comment);
        _bulletinServiceMock.Setup(bs => bs.FindByIdAsync(comment.BulletinId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(bulletin);
        _userServiceMock.Setup(us => us.GetUserByIdAsync(bulletin.OwnerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(bulletinOwner);

        var contextMock = new Mock<ConsumeContext<CommentAdded>>();
        contextMock.SetupGet(c => c.Message).Returns(commentAdded);

        await _service.Consume(contextMock.Object);

        _publishEndpointMock.Verify(pe => pe.Publish(It.Is<SendEmail>(email =>
            email.Receiver == bulletinOwner.Email &&
            email.Subject == $"Добавлен отзыв к объявлению: {bulletin.Title}" &&
            email.Text == $"Текст отзыва: {comment.Text}"
        ), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ConsumeUserRegistred_ShouldPublishEmail()
    {
        var userRegistred = _fixture.Create<UserRegistred>();

        var contextMock = new Mock<ConsumeContext<UserRegistred>>();
        contextMock.SetupGet(c => c.Message).Returns(userRegistred);

        await _service.Consume(contextMock.Object);

        _publishEndpointMock.Verify(pe => pe.Publish(It.Is<SendEmail>(email =>
            email.Receiver == userRegistred.Email &&
            email.Subject == "Регистрация в BulletinBoard" &&
            email.Text == "Спасибо за регистрацию в BulletinBoard! Если это не вы - соболезнуем."
        ), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ConsumeUserLoggedIn_ShouldPublishEmail()
    {
        var userLoggedIn = _fixture.Create<UserLoggedIn>();

        var contextMock = new Mock<ConsumeContext<UserLoggedIn>>();
        contextMock.SetupGet(c => c.Message).Returns(userLoggedIn);

        await _service.Consume(contextMock.Object);

        _publishEndpointMock.Verify(pe => pe.Publish(It.Is<SendEmail>(email =>
            email.Receiver == userLoggedIn.Email &&
            email.Subject == "Вход в BulletinBoard" &&
            email.Text == "По вышей почте выполнен вход в BulletinBoard, если это не вы - соболезнуем."
        ), It.IsAny<CancellationToken>()), Times.Once);
    }
}