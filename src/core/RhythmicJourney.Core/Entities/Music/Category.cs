using System.Collections.Generic;

namespace RhythmicJourney.Core.Entities.Music;

/// <summary>
/// Her bir kateqoriyani temsil edir.
/// </summary>
public class Category
{
    public int               Id    { get; set; }
    public string            Name  { get; set; }

    public ICollection<Song> Songs { get; set; }
}