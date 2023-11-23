using System.Threading.Tasks;
using RhythmicJourney.Application.Contracts.Persistence.Repositories.Abstractions.Music;
using RhythmicJourney.Application.Contracts.Persistence.Repositories.Abstractions.Identity;

namespace RhythmicJourney.Application.Contracts.Persistence.UnitOfWork.Abstractions;

/// <summary>
/// Concrete Repository siniflerini ozunde saxlayir, bundan elave DB ile elaqeli her bir emeliyyatin anliq olaraq DB-ya yansidilmasi evezine butun emeliyyatlari toplayaraq bir butun cem weklinde tek bir transaction uzerinden/daxilinde reallawdirmagi hedefleyen (ve belece DB-ya olan yuku de azaltmiw olan bir) funksionalliqlarin imzalarini saxlayir.
/// </summary>
public interface IUnitOfWork
{
    IRefreshTokenRepository RefreshTokenRepository { get; }
    IRoleRepository         RoleRepository         { get; }
    IUserRepository         UserRepository         { get; }
    ICategoryRepository     CategoryRepository     { get; }
    ISongRepository         SongRepository         { get; }

    Task<int> SaveChangesToDB_StandartDb();
    Task<int> SaveChangesToDB_IdentityDb();
}