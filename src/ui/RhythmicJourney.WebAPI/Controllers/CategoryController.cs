using System;
using MediatR;
using System.Linq;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using RhythmicJourney.Application.Features.Category.Common;
using RhythmicJourney.Application.Features.Category.Queries;
using RhythmicJourney.Application.Features.Category.Commands;
using RhythmicJourney.Application.Features.Category.Common.DTOs;

namespace RhythmicJourney.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "MODERATOR, ADMIN")]
public class CategoryController : ControllerBase
{
    private readonly IMediator _mediator;
    public CategoryController(IMediator mediator) => this._mediator = mediator;

    /// <summary>
    /// Kateqoriyalari siyahilamaq ucundur.
    /// </summary>
    /// <param name="cancellationToken">Emeliyyati legv etmek ucundur.</param>
    /// <returns>Geriye emeliyyatin nece yekunlawdigi ile elaqeli melumatlari ozunde saxlayan 'Result Object Design Pattern' implementasiyasi olan data dondurur.</returns>
    [HttpGet("get-categories")]
    [SwaggerOperation(Summary = "Kateqoriyalari siyahilamaq ucundur.", Description = "Bu endpoint vasitesile movcud kateqoriyalar elde olunur.")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    [SwaggerResponse(StatusCodes.Status403Forbidden)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
    {
        GetCategoriesQuery query = new GetCategoriesQuery();
        CategoryResult result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Kateqoriyadaki musiqileri siyahilamaq ucundur.
    /// </summary>
    /// <param name="model">Musiqileri siyahilanacaq kateqoriya.</param>
    /// <param name="cancellationToken">Emeliyyati legv etmek ucundur.</param>
    /// <returns>Geriye emeliyyatin nece yekunlawdigi ile elaqeli melumatlari ozunde saxlayan 'Result Object Design Pattern' implementasiyasi olan data dondurur.</returns>
    [HttpPost("get-songs-by-category")]
    [SwaggerOperation(Summary = "Kateqoriyadaki musiqileri siyahilamaq ucundur.", Description = "Bu endpoint vasitesile kateqoriyadaki musiqiler elde olunur.")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    [SwaggerResponse(StatusCodes.Status403Forbidden)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetCategorySongs([FromBody] GetCategorySongsRequestDTO model, CancellationToken cancellationToken)
    {
        GetCategorySongsQuery query = new GetCategorySongsQuery(model);
        CategoryResult result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Kateqoriya yaratmaq ucundur.
    /// </summary>
    /// <param name="model">Kateqoriya yaratmaq ucun daxil edilmiw melumatlar.</param>
    /// <param name="cancellationToken">Emeliyyati legv etmek ucundur.</param>
    /// <returns>Geriye emeliyyatin nece yekunlawdigi ile elaqeli melumatlari ozunde saxlayan 'Result Object Design Pattern' implementasiyasi olan data dondurur.</returns>
    [HttpPost("create-category")]
    [SwaggerOperation(Summary = "Kateqoriya yaratmaq ucundur.", Description = "Bu endpoint vasitesile yeni bir kateqoriya yaradilir.")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    [SwaggerResponse(StatusCodes.Status403Forbidden)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> AddCategory([FromBody] AddCategoryRequestDTO model, CancellationToken cancellationToken)
    {
        AddCategoryCommand command = new AddCategoryCommand(model);
        CategoryResult result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess) return Problem
        (
            statusCode: StatusCodes.Status400BadRequest,
            title: string.Join(Environment.NewLine, result.Errors.Select(error => error))
        );

        return Ok(result);
    }

    /// <summary>
    /// Kateqoriyanin melumatlari uzerinde deyiwiklik etmek ucundur.
    /// </summary>
    /// <param name="model">Uzerinde duzeliw olunan hazirki kateqoriyanin yeni melumatlari.</param>
    /// <param name="categoryIdentityDTO">Melumatlari deyiwdirilecek kateqoriya.</param>
    /// <param name="cancellationToken">Emeliyyati legv etmek ucundur.</param>
    /// <returns>Geriye emeliyyatin nece yekunlawdigi ile elaqeli melumatlari ozunde saxlayan 'Result Object Design Pattern' implementasiyasi olan data dondurur.</returns>
    [HttpPost("edit-category/{CategoryID}")]
    [SwaggerOperation(Summary = "Kateqoriyanin melumatlari uzerinde deyiwiklik etmek ucundur.", Description = "Bu endpoint vasitesile mueyyen bir kateqoriya uzerinde deyiwiklik olunur.")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    [SwaggerResponse(StatusCodes.Status403Forbidden)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> EditCategory([FromBody] EditCategoryRequestDTO model, [FromRoute] CategoryIdentityDTO categoryIdentityDTO, CancellationToken cancellationToken)
    {
        EditCategoryCommand command = new EditCategoryCommand(model, categoryIdentityDTO);
        CategoryResult result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess) return Problem
        (
            statusCode: StatusCodes.Status400BadRequest,
            title: string.Join(Environment.NewLine, result.Errors.Select(error => error))
        );

        return Ok(result);
    }

    /// <summary>
    /// Kateqoriyani silmek ucundur.
    /// </summary>
    /// <param name="categoryIdentityDTO">Silinecek kateqoriya.</param>
    /// <param name="cancellationToken">Emeliyyati legv etmek ucundur.</param>
    /// <returns>Geriye emeliyyatin nece yekunlawdigi ile elaqeli melumatlari ozunde saxlayan 'Result Object Design Pattern' implementasiyasi olan data dondurur.</returns>
    [HttpDelete("delete-category/{CategoryID}")]
    [SwaggerOperation(Summary = "Kateqoriyani silmek ucundur.", Description = "Bu endpoint vasitesile mueyyen bir kateqoriya silinir.")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    [SwaggerResponse(StatusCodes.Status403Forbidden)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> DeleteCategory([FromRoute] CategoryIdentityDTO categoryIdentityDTO, CancellationToken cancellationToken)
    {
        DeleteCategoryCommand command = new DeleteCategoryCommand(categoryIdentityDTO);
        CategoryResult result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess) return Problem
        (
            statusCode: StatusCodes.Status400BadRequest,
            title: string.Join(Environment.NewLine, result.Errors.Select(error => error))
        );

        return Ok(result);
    }

    /// <summary>
    /// Kateqoriyaya musiqi elave etmek ucundur.
    /// </summary>
    /// <param name="model">Kateqoriyaya elave edilmek istenen musiqinin melumatlari.</param>
    /// <param name="cancellationToken">Emeliyyati legv etmek ucundur.</param>
    /// <returns>Geriye emeliyyatin nece yekunlawdigi ile elaqeli melumatlari ozunde saxlayan 'Result Object Design Pattern' implementasiyasi olan data dondurur.</returns>
    [HttpPost("add-song-to-category")]
    [SwaggerOperation(Summary = "Kateqoriyaya musiqi elave etmek ucundur.", Description = "Bu endpoint vasitesile mueyyen bir kateqoriyaya her hansi bir musiqi elave olunur.")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    [SwaggerResponse(StatusCodes.Status403Forbidden)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> AddSongToCategory([FromBody] AddSongToCategoryRequestDTO model, CancellationToken cancellationToken)
    {
        AddSongToCategoryCommand command = new AddSongToCategoryCommand(model);
        CategoryResult result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess) return Problem
        (
            statusCode: StatusCodes.Status400BadRequest,
            title: string.Join(Environment.NewLine, result.Errors.Select(error => error))
        );

        return Ok(result);
    }
}