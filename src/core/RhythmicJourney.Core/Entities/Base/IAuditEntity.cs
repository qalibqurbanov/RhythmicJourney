using System;

namespace RhythmicJourney.Core.Entities.Base;

/// <summary>
/// Audit meqsedile iwledilecek olan ozellikleri saxlayir.
/// </summary>
public interface IAuditEntity
{
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}