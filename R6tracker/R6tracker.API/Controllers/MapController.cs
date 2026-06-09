using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using R6tracker.Core.DTOs;
using R6tracker.Core.Interfaces;

namespace R6tracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MapsController : ControllerBase
{
    private readonly IMapService mapService;
    private readonly ILogger<MapsController> logger;

    public MapsController(IMapService mapService, ILogger<MapsController> logger)
    {
        this.mapService = mapService;
        this.logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var maps = await mapService.GetAllAsync();
        return Ok(maps);
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        var maps = await mapService.GetActiveAsync();
        return Ok(maps);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var map = await mapService.GetByIdAsync(id);
        return Ok(map);
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Create([FromBody] CreateMapDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await mapService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateMapDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await mapService.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Delete(int id)
    {
        await mapService.DeleteAsync(id);
        return NoContent();
    }
}