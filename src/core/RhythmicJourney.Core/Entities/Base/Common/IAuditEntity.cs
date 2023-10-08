using System;

namespace RhythmicJourney.Core.Entities.Base.Common;

/// <summary>
/// Audit meqsedile iwledilecek olan ozellikleri saxlayan base tipdir.
/// </summary>
public interface IAuditEntity
{
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}