using Microsoft.AspNetCore.Mvc;
using R6tracker.Core.Interfaces;

namespace R6tracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RanksController : ControllerBase
{
    private readonly IRankService rankService;

    public RanksController(IRankService rankService)
    {
        this.rankService = rankService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var ranks = await rankService.GetAllAsync();
        return Ok(ranks);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var rank = await rankService.GetByIdAsync(id);
        return Ok(rank);
    }
}