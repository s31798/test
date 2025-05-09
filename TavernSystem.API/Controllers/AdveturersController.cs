using Microsoft.AspNetCore.Mvc;
using TavernSystem.Models.DTOs;
using WebApplication1;

namespace TavernSystem.Controllers;

[ApiController]
[Route("api/")]
public class AdventurerController : ControllerBase
{

    private readonly IAdventurerService _adventurerService;
    public AdventurerController(IAdventurerService adventurerService)
    {
        _adventurerService = adventurerService;
    }
    [HttpGet("Adventurers")]
    public async Task<IResult> GetAllAdventurersAsync(CancellationToken cancellationToken)
    {
        var result = await _adventurerService.GetAllAdventurersAsync(cancellationToken);
        return Results.Ok(result);
    }
    [HttpGet("Adventurers/{id}")]
    public async Task<IResult> GetAllAdventurersAsync(int id, CancellationToken cancellationToken)
    {
        var result = await _adventurerService.GetAdventurerAsync(id,cancellationToken);
        return Results.Ok(result);
    }

    [HttpPost("Adventurers")]
    public async Task<IResult> InsertAdventurer([FromBody] InsertAdventurerDTO adventurer, CancellationToken cancellationToken)
    {
        try
        { 
            _adventurerService.InsertAdventurer(adventurer, cancellationToken);
            return Results.Ok();
        }
        catch (Exception e)
        {
            return Results.BadRequest(e);
        }
       
    }
}