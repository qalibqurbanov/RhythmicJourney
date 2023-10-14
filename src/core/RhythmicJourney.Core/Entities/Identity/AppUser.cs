using System.Collections.Generic;
using RhythmicJourney.Core.Entities.Base;

namespace RhythmicJourney.Core.Entities.Identity;

/// <summary>
/// Her hansi bir istifadecini temsil edir.
/// </summary>
public partial class AppUser : BaseEntity
{
    public AppUser()
    {
        this.RefreshTokens = new HashSet<RefreshToken>();
    }

    public ICollection<RefreshToken>? RefreshTokens { get; set; }
    /*
        * Her userin 1 Refresh Token-e sahib oldugunu bilirik, yaxwi bes 1 user niye Multiple Refresh Tokene sahib ola biler?
            ___________
            > Problem : user API-a muxtelif vasitelerden(bawqa ferqli bir brauzerden ve ya umumiyyetle Telefon, Komputer ve s. kimi cihazdan) baglana biler, eger cihazinin birinde Refresh Token yenilense diger cihazlarindaki Refresh Token artiq kecersiz olacaq (cunki, her bir userin cemi 1 Refresh Tokeni var) ve hemin diger cihazlarinda olan Access Tokenin vaxti qurtaran kimi user profilinden atilacaq (cunki, user kecerli Refresh Tokene sahib deyil ki, Access Tokenin de yenilesin) ve tebii ki, atildiqdan sonra gerek yeniden login olsun. Autentifikasiya sistemini bu cur qurmaq ise youtube, sosial webekeler ve s. kimi userin eyni anda muxtelif cihazlarda logged-in qalmali oldugu applerde problemdir.
            ___________
            > Helli   : Azca yuxarida dediyim sebebe gore sistemi bir userin birden cox Refresh Tokene sahib ola bileceyi wekilde qururuq ki, user birden cox cihazda aktiv bir wekilde profilinde saxlanila bilinsin. Refresh Tokeni revoke/deaktiv eden zaman ise, "/renew-tokens" endpointimize hansi Refresh Tokenle edilibse request sirf hemin Refresh Tokeni revoke/deaktiv edirik.
    */
}

public partial class AppUser
{
    /// <summary>
    /// Bu metoda parametr olaraq verdiyimiz deyerlere sahib olan 'User' sinif obyektini yaradir.
    /// </summary>
    /// <returns>Geriye 'RefreshToken' sinifinin yaratmiw oldugu obyektini dondurur.</returns>
    public static AppUser CreateUser(string firstName, string lastName, string email, string userName)
    {
        return new AppUser()
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            UserName = userName
        };
    }
}