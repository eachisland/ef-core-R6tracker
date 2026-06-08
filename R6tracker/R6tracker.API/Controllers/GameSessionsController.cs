using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using R6tracker.Core.DTOs;
using R6tracker.Core.Interfaces;

namespace R6tracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameSessionsController : ControllerBase
{
    private readonly IGameSessionService sessionService;
    private readonly ILogger<GameSessionsController> logger;

    public GameSessionsController(IGameSessionService sessionService, ILogger<GameSessionsController> logger)
    {
        this.sessionService = sessionService;
        this.logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var sessions = await sessionService.GetAllAsync();
        return Ok(sessions);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var session = await sessionService.GetByIdAsync(id);
        return Ok(session);
    }

    [HttpGet("player/{playerId}")]
    public async Task<IActionResult> GetByPlayer(string playerId)
    {
        var sessions = await sessionService.GetByPlayerIdAsync(playerId);
        return Ok(sessions);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateGameSessionDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await sessionService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Delete(string id)
    {
        await sessionService.DeleteAsync(id);
        return NoContent();
    }
}