using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using RhythmicJourney.Application.Enums;
using RhythmicJourney.Application.Helpers;
using RhythmicJourney.Core.Entities.Music;
using RhythmicJourney.Application.Extensions;
using RhythmicJourney.Application.Features.Music.Common;
using RhythmicJourney.Application.Features.Music.Commands;
using RhythmicJourney.Application.Contracts.Persistence.UnitOfWork.Abstractions;

namespace RhythmicJourney.Application.Features.Music.Handlers.CommandHandlers;

/// <summary>
/// Musiqinin redakte olunmasi sorgusunu reallawdiran handler.
/// </summary>
public class EditSongCommandHandler : IRequestHandler<EditSongCommand, SongResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHostingEnvironment _hostingEnvironment;

    public EditSongCommandHandler(IUnitOfWork unitOfWork, IHostingEnvironment hostingEnvironment)
    {
        this._unitOfWork = unitOfWork;
        this._hostingEnvironment = hostingEnvironment;
    }

    public async Task<SongResult> Handle(EditSongCommand request, CancellationToken cancellationToken)
    {
        Song song = _unitOfWork.SongRepository.GetSongs(song => song.Id == request.songIdentityDTO.SongID).FirstOrDefault();
        {
            if (song == null)
            {
                return await SongResult.FailureAsync($"Song with ID {request.songIdentityDTO.SongID} not found!");
            }
        }

        {
            if (request.DTO.NewSongFile != null)
            {
                FileHelpers.RemoveFromUploads(song.SongFileName, _hostingEnvironment);

                request.DTO.NewSongFile.MoveFormFile(_hostingEnvironment, Uploads.Musics, out string NewSongFileName);

                song.SongFileName = NewSongFileName;
            }

            if (request.DTO.NewSongArt != null)
            {
                FileHelpers.RemoveFromArts(song.SongArtName, _hostingEnvironment);

                request.DTO.NewSongArt.MoveFormFile(_hostingEnvironment, Uploads.Arts, out string NewSongArtName);

                song.SongArtName = NewSongArtName;
            }

            song.ArtistName = request.DTO.NewArtistName ?? song.ArtistName;
            song.SongName = request.DTO.NewSongName ?? song.SongName;
        }

        {
            _unitOfWork.SongRepository.Edit(song);
            int affectedRowCount = await _unitOfWork.SaveChangesToDB_StandartDb();

            return await SongResult.SuccessAsync($"Updated {affectedRowCount} data.");
        }
    }
}