using System;
using System.Threading.Tasks;
using RhythmicJourney.Persistence.Contexts;
using Microsoft.Extensions.DependencyInjection;
using RhythmicJourney.Application.Contracts.Persistence.UnitOfWork.Abstractions;
using RhythmicJourney.Application.Contracts.Persistence.Repositories.Abstractions.Music;
using RhythmicJourney.Application.Contracts.Persistence.Repositories.Abstractions.Identity;

namespace RhythmicJourney.Persistence.Contracts.UnitOfWork.Concretes;

/// <summary>
/// Concrete Repository siniflerini ozunde saxlayir, bundan elave DB ile elaqeli her bir emeliyyatin anliq olaraq DB-ya yansidilmasi evezine butun emeliyyatlari toplayaraq bir butun cem weklinde tek bir transaction uzerinden/daxilinde reallawdirmagi hedefleyen (ve belece DB-ya olan yuku de azaltmiw olan bir) funksionalliga('SaveChangesToDB_{StandartDb/IdentityDb}') sahibdir.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly IServiceProvider _serviceProvider;

    private readonly RhythmicJourneyStandartDbContext _rhythmicJourneyStandartDbContext;
    private readonly RhythmicJourneyIdentityDbContext _rhythmicJourneyIdentityDbContext;

    public UnitOfWork(IServiceProvider serviceProvider, RhythmicJourneyStandartDbContext rhythmicJourneyStandartDbContext, RhythmicJourneyIdentityDbContext rhythmicJourneyIdentityDbContext)
    {
        this._serviceProvider = serviceProvider;

        this._rhythmicJourneyStandartDbContext = rhythmicJourneyStandartDbContext;
        this._rhythmicJourneyIdentityDbContext = rhythmicJourneyIdentityDbContext;
    }

    #region IUnitOfWork
    /// <summary>
    /// Standart DB ile elaqeli entity-lere edilmiw deyiwiklikleri tek bir transaction daxilindeStandart DB-ya gonderib execute eden funksiyadir.
    /// </summary>
    public async Task<int> SaveChangesToDB_StandartDb() => this._rhythmicJourneyStandartDbContext.SaveChanges();

    /// <summary>
    /// Identity DB ile elaqeli entity-lere edilmiw deyiwiklikleri tek bir transaction daxilinde Identity DB-ya gonderib execute eden funksiyadir.
    /// </summary>
    public async Task<int> SaveChangesToDB_IdentityDb() => this._rhythmicJourneyIdentityDbContext.SaveChanges();
    #endregion IUnitOfWork

    #region Lazy Loading
    /*
        * Burada 'Lazy Loading' tetbiq edirem, yeni lazim oldu-olmadi Repository-lere HEAP-de yer ayrilmayacaq (eger Repositoryleri yuxarida 'UnitOfWork' konstruktorunda initializasiya etseydim, her 'UnitOfWork' obyektinin yaradiliwinda lazim oldu-olmadi her bir Repository ucun de bir obyekt yaradilacaq idi). Awagidaki public getter propertyler sayesinde yalniz lazim olan, yeni muraciet olunan Repositoryleri initialize edirik, meselen: yalniz 'IRefreshTokenRepository' cagirilanda yeni bir 'RefreshTokenRepository' orneyi IoC Containerdan elde olunaraq verilecek '_refreshTokenRepository'-a ve elecede her bir diger repo, yalniz uygun public property cagirilanda initialize edilecek Repositorymiz.
    */

    /// <summary>
    /// IoC Containerdan teleb olunan(T) tipde bir obyekt elde edir.
    /// </summary>
    /// <typeparam name="TRepository">IoC Containerdan ne tipli obyekt teleb edirik.</typeparam>
    /// <param name="member">IoC Containerdan elde olunmuw obyekt hara set olunsun ve ya bawqa sozle hara verilsin/menimsedilsin.</param>
    /// <returns>Geriye, IoC Containerdan elde etdiyi obyekti dondurur.</returns>
    private TRepository GetServiceFrom_IoC<TRepository>(TRepository member)
    {
        /* 'member' nulldursa, IoC-den 'TRepository' orneyi elde ederek menimset 'member'-a ve geri dondur ('member'-i), eks halda(yeni, 'member' null deyilse) 'member' geri dondurulsun: */
        return member ??= _serviceProvider.GetService<TRepository>();
    }

    /* Burada 'IBaseRepository'-ni yaratmiram, cunki hec bir yerde bir bawa 'IBaseRepository'-ni iwletmirem, 'IBaseRepository'-deki metodlari awagidaki child interfeysler vasitesile cagiriram. */
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IRoleRepository         _roleRepository;
    private readonly IUserRepository         _userRepository;
    private readonly ICategoryRepository     _categoryRepository;
    private readonly ISongRepository         _songRepository;

    public IRefreshTokenRepository RefreshTokenRepository { get { return GetServiceFrom_IoC(this._refreshTokenRepository); } }
    public IRoleRepository         RoleRepository         { get { return GetServiceFrom_IoC(this._roleRepository);         } }
    public IUserRepository         UserRepository         { get { return GetServiceFrom_IoC(this._userRepository);         } }
    public ICategoryRepository     CategoryRepository     { get { return GetServiceFrom_IoC(this._categoryRepository);     } }
    public ISongRepository         SongRepository         { get { return GetServiceFrom_IoC(this._songRepository);         } }
    #endregion Lazy Loading
}