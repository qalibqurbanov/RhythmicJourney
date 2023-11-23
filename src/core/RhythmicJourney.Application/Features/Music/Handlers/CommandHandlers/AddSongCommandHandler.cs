using MediatR;
using System.Threading;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using RhythmicJourney.Application.Enums;
using RhythmicJourney.Core.Entities.Music;
using RhythmicJourney.Application.Extensions;
using RhythmicJourney.Application.Features.Music.Common;
using RhythmicJourney.Application.Features.Music.Commands;
using RhythmicJourney.Application.Contracts.Persistence.UnitOfWork.Abstractions;

namespace RhythmicJourney.Application.Features.Music.Handlers.CommandHandlers;

/// <summary>
/// Yeni musiqinin elave olunmasi sorgusunu reallawdiran handler.
/// </summary>
public class AddSongCommandHandler : IRequestHandler<AddSongCommand, SongResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHostingEnvironment _hostingEnvironment;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AddSongCommandHandler(IUnitOfWork unitOfWork, IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
    {
        this._unitOfWork = unitOfWork;
        this._hostingEnvironment = hostingEnvironment;
        this._httpContextAccessor = httpContextAccessor;
    }

    public async Task<SongResult> Handle(AddSongCommand request, CancellationToken cancellationToken)
    {
        request.DTO.SongFile.MoveFormFile(_hostingEnvironment, Uploads.Musics, out string songFileName);
        request.DTO.SongArt.MoveFormFile(_hostingEnvironment, Uploads.Arts, out string artFileName);

        Song song = new Song()
        {
            UploaderID = int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("UserID")),

            ArtistName = request.DTO.ArtistName,
            SongName = request.DTO.SongName,

            SongFileName = songFileName,
            SongArtName = artFileName
        };

        {
            _unitOfWork.SongRepository.Add(song);
            int affectedRowCount = await _unitOfWork.SaveChangesToDB_StandartDb();

            return await SongResult.SuccessAsync($"Added {affectedRowCount} data.");
        }
    }
}