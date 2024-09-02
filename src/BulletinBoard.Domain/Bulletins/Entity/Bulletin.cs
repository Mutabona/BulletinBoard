﻿using BulletinBoard.Domain.Base;
using BulletinBoard.Domain.Files.Images.Entity;
using BulletinBoard.Domain.Users.Entity;

namespace BulletinBoard.Domain.Bulletins.Entity;

/// <summary>
/// Сущность объявления.
/// </summary>
public class Bulletin : BaseEntity
{
    /// <summary>
    /// Идентификатор владельца.
    /// </summary>
    public Guid OwnerId { get; set; }
    
    /// <summary>
    /// Сущность владельца.
    /// </summary>
    public User Owner { get; set; }
    
    /// <summary>
    /// Название объявления.
    /// </summary>
    public string Title { get; set; }
    
    /// <summary>
    /// Описание объявления.
    /// </summary>
    public string Description { get; set; }
    
    /// <summary>
    /// Фото из объявления.
    /// </summary>
    public ICollection<Image> Images { get; set; }
}