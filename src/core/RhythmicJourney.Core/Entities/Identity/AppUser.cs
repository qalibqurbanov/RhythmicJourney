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