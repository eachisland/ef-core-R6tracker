using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using R6tracker.Core.DTOs;
using R6tracker.Core.Interfaces;
using System.Security.Claims;

namespace R6tracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayersController : ControllerBase
{
    private readonly IPlayerService playerService;
    private readonly ILogger<PlayersController> logger;

    public PlayersController(IPlayerService playerService, ILogger<PlayersController> logger)
    {
        this.playerService = playerService;
        this.logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string search = "",
        [FromQuery] string platform = "")
    {
        var players = await playerService.GetAllAsync(search, platform);
        return Ok(players);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var player = await playerService.GetByIdAsync(id);

        if (player == null)
            return NotFound(new { message = "Player not found" });

        return Ok(player);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreatePlayerDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        try
        {
            var result = await playerService.CreateAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(string id, [FromBody] CreatePlayerDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var updated = await playerService.UpdateAsync(id, dto, userId);

        if (!updated)
            return Forbid();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var isAdmin = User.IsInRole("Administrator");
        var deleted = await playerService.DeleteAsync(id, userId, isAdmin);

        if (!deleted)
            return NotFound(new { message = "Player not found" });

        return NoContent();
    }
}