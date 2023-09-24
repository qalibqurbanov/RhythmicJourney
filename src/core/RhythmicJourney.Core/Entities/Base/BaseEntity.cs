using System;
using Microsoft.AspNetCore.Identity;

namespace RhythmicJourney.Core.Entities.Base;

/// <summary>
/// Kimliyi olan entitylere lazimi ozellikleri qazandiran base tipdir.
/// </summary>
public abstract class BaseEntity : IdentityUser<int>, IBaseEntity, IAuditEntity
{
    public override int Id { get; set; }

    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}