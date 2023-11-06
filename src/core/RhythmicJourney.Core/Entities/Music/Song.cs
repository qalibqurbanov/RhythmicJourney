namespace RhythmicJourney.Core.Entities.Music;

/// <summary>
/// Her bir musiqini temsil edir.
/// </summary>
public class Song
{
    public int      Id           { get; set; }
    public string   ArtistName   { get; set; }
    public string   SongName     { get; set; }

    public int      UploaderID   { get; set; }

    public string   SongFileName { get; set; }
    public string   SongArtName  { get; set; }

    public bool     IsDeleted    { get; set; }

    public int?      CategoryId  { get; set; }
    public Category? Category    { get; set; }
}