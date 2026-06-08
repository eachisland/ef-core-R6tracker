using System.Collections.Generic;

namespace R6tracker.Infrastructure.Data.Models;

public class R6Map
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public ICollection<GameSession> GameSessions { get; set; } = new List<GameSession>();
}