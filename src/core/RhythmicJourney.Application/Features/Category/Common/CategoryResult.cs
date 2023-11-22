using System.Threading.Tasks;
using System.Collections.Generic;
using RhythmicJourney.Core.Entities.Music;
using Kateqoriya = RhythmicJourney.Core.Entities.Music.Category;

namespace RhythmicJourney.Application.Features.Category.Common;

/// <summary>
/// Netice olaraq kateqoriya ile elaqeli dondureceyimiz uygun neticeni ('Result Object Design Pattern'-in implementasiyasi olan) bu sinif vasitesile dondururuk.
/// </summary>
public partial class CategoryResult
{
    private CategoryResult() { /* Awagidaki spesifik neticeleri temsil eden metodlarin cagirilmasini isteyirem deye bu konstruktoru 'private' vasitesile gizledirem */ }

    /// <summary>
    /// Netice olaraq kateqoriyanin detallarini dondurmek isteyirikse bu overloadi iwledirik.
    /// </summary>
    public static Task<CategoryResult> SuccessAsync(List<Kateqoriya> categories) => Task.FromResult(new CategoryResult() { Categories = categories, IsSuccess = true });

    /// <summary>
    /// Netice olaraq kateqoriyanin detallarini dondurmek isteyirikse bu overloadi iwledirik.
    /// </summary>
    public static Task<CategoryResult> SuccessAsync(List<Song> songs) => Task.FromResult(new CategoryResult() { Songs = songs, IsSuccess = true });

    /// <summary>
    /// Netice olaraq sadece mesaj dondurmek isteyirikse bu overloadi iwledirik.
    /// </summary>
    public static Task<CategoryResult> SuccessAsync(string message) => Task.FromResult(new CategoryResult() { Message = message, IsSuccess = true });

    /// <summary>
    /// Netice olaraq baw vermiw xetani dondurmek isteyirikse bu overloadi iwledirik.
    /// </summary>
    public static Task<CategoryResult> FailureAsync(List<string> error) => Task.FromResult(new CategoryResult() { Errors = error, IsSuccess = false });

    /// <summary>
    /// Netice olaraq baw vermiw xeta haqqinda mesaj dondurmek isteyirikse bu overloadi iwledirik.
    /// </summary>
    public static Task<CategoryResult> FailureAsync(string message) => Task.FromResult(new CategoryResult() { Message = message, IsSuccess = false });
}

public partial class CategoryResult
{
    public List<Kateqoriya> Categories { get; set; }
    public List<Song> Songs { get; set; }

    public string Message { get; private set; }

    public bool IsSuccess { get; private set; }
    public List<string> Errors { get; private set; }
}