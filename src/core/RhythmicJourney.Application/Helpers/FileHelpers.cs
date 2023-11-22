﻿using System.IO;
using Microsoft.AspNetCore.Hosting;
using RhythmicJourney.Application.Enums;

namespace RhythmicJourney.Application.Helpers;

/// <summary>
/// Fayl ile elaqeli komekci funksionalliqlari saxlayir.
/// </summary>
public static class FileHelpers
{
    /// <summary>
    /// Userin upload etdiyi musiqi ve artworkleri yukleyeceyim papkamin adini saxlayir.
    /// </summary>
    private readonly static string FolderName = "uploads";

    /// <summary>
    /// 'Uploads/Musics' papkasindaki fayli silir
    /// </summary>
    /// <param name="fileName">Silinmesini istediyimiz faylin adi.</param>
    /// <param name="env">Appin run olundugu muhiti temsil eden tip.</param>
    public static void RemoveFromUploads(string fileName, IHostingEnvironment env)
    {
        string targetFile = Path.Combine(env.ContentRootPath, FolderName, nameof(Uploads.Musics), fileName);

        if (File.Exists(targetFile))
        {
            File.Delete(targetFile);
        }
    }

    /// <summary>
    /// 'Uploads/Arts' papkasindaki fayli silir
    /// </summary>
    /// <param name="fileName">Silinmesini istediyimiz faylin adi.</param>
    /// <param name="env">Appin run olundugu muhiti temsil eden tip.</param>
    public static void RemoveFromArts(string fileName, IHostingEnvironment env)
    {
        string targetFile = Path.Combine(env.ContentRootPath, FolderName, nameof(Uploads.Arts), fileName);

        if (File.Exists(targetFile))
        {
            File.Delete(targetFile);
        }
    }
}