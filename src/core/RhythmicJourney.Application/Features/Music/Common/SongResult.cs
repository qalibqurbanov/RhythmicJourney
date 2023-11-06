using System.Threading.Tasks;
using System.Collections.Generic;
using RhythmicJourney.Core.Entities.Music;

namespace RhythmicJourney.Application.Features.Music.Common;

/// <summary>
/// Netice olaraq musiqiyle elaqeli dondureceyimiz uygun neticeni ('Result Object Design Pattern'-in implementasiyasi olan) bu sinif vasitesile dondururuk.
/// </summary>
public partial class SongResult
{
    private SongResult() { /* Awagidaki spesifik neticeleri temsil eden metodlarin cagirilmasini isteyirem deye bu konstruktoru 'private' vasitesile gizledirem */ }

    /// <summary>
    /// Netice olaraq musiqinin detallarini dondurmek isteyirikse bu overloadi iwledirik.
    /// </summary>
    public static Task<SongResult> SuccessAsync(List<Song> songs) => Task.FromResult(new SongResult() { IsSuccess = true, Songs = songs });

    /// <summary>
    /// Netice olaraq sadece mesaj dondurmek isteyirikse bu overloadi iwledirik.
    /// </summary>
    public static Task<SongResult> SuccessAsync(string message) => Task.FromResult(new SongResult() { Message = message, IsSuccess = true });

    /// <summary>
    /// Netice olaraq baw vermiw xetani dondurmek isteyirikse bu overloadi iwledirik.
    /// </summary>
    public static Task<SongResult> FailureAsync(List<string> errors) => Task.FromResult(new SongResult() { IsSuccess = false, Errors = errors });
}

public partial class SongResult
{
    public List<Song> Songs { get; set; }

    public string Message { get; private set; }

    public bool IsSuccess { get; private set; }
    public List<string> Errors { get; private set; }
}