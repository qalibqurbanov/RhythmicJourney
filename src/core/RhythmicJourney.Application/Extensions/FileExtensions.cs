using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using RhythmicJourney.Application.Enums;

namespace RhythmicJourney.Application.Extensions;

/// <summary>
/// Fayllarla iwlemek ile elaqeli extension metodlari saxlayir.
/// </summary>
public static class FileExtensions
{
    /// <summary>
    /// Form-a yuklenen wekli 'Web Root (wwwroot)' papkasina yerlewdirir.
    /// </summary>
    /// <param name="file">Form-dan gelen wekil.</param>
    /// <param name="fileName">Metoddan kenara gonderilecek wekil adi (+ Daha sonra bu weklin adini hemin bu metoddan kenarda yaxalayaraq DB-ya qeyd edecem).</param>
    public static void MoveFormFile(this IFormFile file, IHostingEnvironment env, Uploads fileType, out string fileName)
    {
        /* Faylin ozunu yerlewdirirem 'Web Root (wwwroot)' papkasina: */
        string targetPath = Path.Combine(env.ContentRootPath, $"uploads//{fileType}");

        /* Ilk once metoddan cole gondereceyimiz fayl adini temizleyirem: */
        fileName = string.Empty;

        /* Eyni adli fayl hedef pathde movcuddursa, faylin adini editleyecem: */
        if (File.Exists(Path.Combine(targetPath, file.FileName)))
        {
            /* {FaylinUzantisizEslAdi}-_-image{RandomReqem}{HazirkiTick}{FaylinUzantisi} */
            fileName = $"{file.FileName.Remove(file.FileName.LastIndexOf('.'))}-_-{fileType}{new Random().Next(100, int.MaxValue)}{DateTime.Now.Ticks}{Path.GetExtension(file.FileName)}";
        }
        else
        {
            /* {FaylinEslAdiUzantisiIleBirlikde} */
            fileName = file.FileName;
        }

        targetPath = Path.Combine(targetPath, fileName);

        using (FileStream FS = new FileStream(targetPath, FileMode.Create))
        {
            file.CopyTo(FS);
        }
    }
}