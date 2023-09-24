﻿namespace RhythmicJourney.Application.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// String-in bow olub-olmadigini yoxlayir.
    /// </summary>
    /// <param name="sourceString">Yoxlanilacaq string.</param>
    /// <returns>string bowdursa 'true', eks halda 'false' dondurur.</returns>
    public static bool IsEmpty(this string sourceString)
    {
        if (string.IsNullOrEmpty(sourceString) || string.IsNullOrWhiteSpace(sourceString)) return true;
        else return false;
    }
}