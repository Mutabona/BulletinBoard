namespace BulletinBoard.Domain.Base;

/// <summary>
/// Базовая сущность.
/// </summary>
public class BaseEntity
{
   /// <summary>
   /// Идентификатор.
   /// </summary>
   public Guid Id { get; set; }
   
   /// <summary>
   /// Дата создания сущности.
   /// </summary>
   public DateTime CreatedAt { get; set; }
}