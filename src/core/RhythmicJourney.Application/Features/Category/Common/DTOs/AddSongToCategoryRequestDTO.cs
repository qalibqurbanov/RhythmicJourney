namespace RhythmicJourney.Application.Features.Category.Common.DTOs;

/// <summary>
/// Kateqoriyaya musiqi elave etme sorgusu ucun hansi kateqoriyaya hansi musiqinin elave olunacagini temsil edir.
/// </summary>
/// <param name="SongID">Uzerinde iwleyeceyimiz musiqi.</param>
/// <param name="CategoryID">Uzerinde iwleyeceyimiz kateqoriya.</param>
public record AddSongToCategoryRequestDTO(int SongID, int CategoryID);