using System;
using Microsoft.AspNetCore.Identity;
using RhythmicJourney.Core.Entities.Base.Common;

namespace RhythmicJourney.Core.Entities.Base;

/// <summary>
/// Kimliyi olan entitylere lazimi ozellikleri qazandiran base tipdir.
/// </summary>
public abstract class BaseEntity : IdentityUser<int>, /*IBaseEntity, */ICustomEntity, IAuditEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}