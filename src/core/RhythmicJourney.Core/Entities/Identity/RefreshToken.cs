using System;
using System.ComponentModel.DataAnnotations;

namespace RhythmicJourney.Core.Entities.Identity;

/// <summary>
/// 'Refresh Token'-i temsil edir.
/// </summary>
public partial class RefreshToken
{
    [Key]
    public Guid      Id        { get; set; }

    public string    Token     { get; set; } /* 'Refresh Token'-i temsil edir. Class adiyla eyni ada sahib ola bilmir deye adi sadece 'Token'-dir. */
    public DateTime  ExpiresOn { get; set; }
    public DateTime  CreatedOn { get; set; }
    public DateTime? RevokedOn { get; set; }
    public bool      IsActive  { get; set; } /* Aktivdir/istifadededir/iwlekdir menasinda. Bu propertyni Refresh Tokeni revoke/deaktiv etmek ucun iwledecem. */

    /* Burada 'RefreshToken' ve 'AppUser' cedvelleri arasinda 'Foreign Key' elaqesini olacagini deyirem, lakin 'Foreign Key' ucun lazim olan sutunun avtomatik yaradilmamasi ucun manual bir wekilde ozum yaradiram(UserID). */
    public int       UserID    { get; set; }
    public AppUser   User      { get; set; } 
}

public partial class RefreshToken
{
    /// <summary>
    /// Bu metoda parametr olaraq verdiyimiz deyerlere sahib olan 'RefreshToken' sinif obyektini yaradir.
    /// </summary>
    /// <returns>Geriye 'RefreshToken' sinifinin yaratmiw oldugu obyektini dondurur.</returns>
    public static RefreshToken CreateObject(string refreshToken, DateTime expiresOn)
    {
        return new RefreshToken()
        {
            Token     = refreshToken,
            IsActive  = true,
            CreatedOn = DateTime.UtcNow,
            ExpiresOn = expiresOn
        };
    }
}