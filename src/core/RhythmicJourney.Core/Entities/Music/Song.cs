using RhythmicJourney.Core.Entities.Identity;

namespace RhythmicJourney.Core.Entities.Music;

/// <summary>
/// Her bir musiqini temsil edir.
/// </summary>
public class Song
{
    public int      Id          { get; set; }
    public string   ArtistName  { get; set; }
    public string   SongName    { get; set; }
                               
    public AppUser  Uploader    { get; set; }
                               
    public string   SongFileURL { get; set; }
    public string   SongArtURL  { get; set; }
                               
    public bool     IsDeleted   { get; set; }
                               
    public int      CategoryId  { get; set; }
    public Category Category    { get; set; }
}