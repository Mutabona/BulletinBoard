using BulletinBoard.AppServices.Contexts.Bulletins.Services;
using BulletinBoard.AppServices.Contexts.Comments.Services;
using BulletinBoard.AppServices.Contexts.Users.Services;
using BulletinBoard.Contracts.Emails;
using MassTransit;

namespace BulletinBoard.AppServices.Services;

/// <summary>
/// Сервис нотификации пользователей.
/// </summary>
/// <param name="userService">Сервис для работы с пользователями.</param>
/// <param name="bulletinService">Сервис для работы с объявлениями.</param>
/// <param name="commentService">Сервис для работы с комментариями.</param>
/// <param name="publishEndpoint">Отправитель сообщений в шину.</param>
public class NotificationService(IUserService userService, IBulletinService bulletinService, ICommentService commentService, IPublishEndpoint publishEndpoint) : IConsumer<CommentAdded>, IConsumer<UserRegistred>, IConsumer<UserLoggedIn>
{
    public async Task Consume(ConsumeContext<CommentAdded> context)
    {
        var comment = await commentService.GetCommentByIdAsync(context.Message.Id, context.CancellationToken);
        var bulletin = await bulletinService.FindByIdAsync(comment.BulletinId, context.CancellationToken);
        var bulletinOwner = await userService.GetUserByIdAsync(bulletin.OwnerId, context.CancellationToken);
        
        await publishEndpoint.Publish<SendEmail>(new
        {
            receiver = bulletinOwner.Email,
            subject = "Добавлен отзыв к объявлению: " + bulletin.Title,
            text = "Текст отзыва: " + comment.Text,
        }, context.CancellationToken);
    }

    public async Task Consume(ConsumeContext<UserRegistred> context)
    {
        await publishEndpoint.Publish<SendEmail>(new
        {
            receiver = context.Message.Email,
            subject = "Регистрация в BulletinBoard",
            text = "Спасибо за регистрацию в BulletinBoard! Если это не вы - соболезнуем.",
        }, context.CancellationToken);
    }

    public async Task Consume(ConsumeContext<UserLoggedIn> context)
    {
        await publishEndpoint.Publish<SendEmail>(new
        {
            receiver = context.Message.Email,
            subject = "Вход в BulletinBoard",
            text = "По вышей почте выполнен вход в BulletinBoard, если это не вы - соболезнуем.",
        }, context.CancellationToken);
    }
}