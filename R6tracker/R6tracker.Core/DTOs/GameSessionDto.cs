namespace R6tracker.Core.DTOs;

public class GameSessionDto
{
    public string Id { get; set; } = string.Empty;
    public DateTime DatePlayed { get; set; }
    public int Kills { get; set; }
    public int Deaths { get; set; }
    public string Result { get; set; } = string.Empty;
    public string Map { get; set; } 
    public string PlayerId { get; set; } = string.Empty;
    public string PlayerName { get; set; } = string.Empty;
}