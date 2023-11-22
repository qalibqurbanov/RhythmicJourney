using System;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
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

    [HttpGet("get-categories")]
    public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
    {
        GetCategoriesQuery query = new GetCategoriesQuery();
        CategoryResult result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    [HttpPost("get-songs-by-category")]
    public async Task<IActionResult> GetCategorySongs([FromBody] GetCategorySongsRequestDTO model, CancellationToken cancellationToken)
    {
        GetCategorySongsQuery query = new GetCategorySongsQuery(model);
        CategoryResult result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    [HttpPost("create-category")]
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

    [HttpPost("add-song-to-category")]
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

    [HttpPost("edit-category/{CategoryID}")]
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

    [HttpPost("delete-category/{CategoryID}")]
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
}