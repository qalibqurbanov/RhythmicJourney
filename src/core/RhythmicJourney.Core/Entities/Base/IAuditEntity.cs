using System;

namespace RhythmicJourney.Core.Entities.Base;

public interface IAuditEntity
{
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}