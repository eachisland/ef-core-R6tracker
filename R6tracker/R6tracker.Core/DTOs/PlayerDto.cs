namespace R6tracker.Core.DTOs;

public class PlayerDto
{
    public string Id { get; set; } = string.Empty;
    public string PlayerName { get; set; } = string.Empty;
    public string Platform { get; set; } = string.Empty;
    public int Level { get; set; }
    public int MatchesPlayed { get; set; }
    public int Kills { get; set; }
    public int Deaths { get; set; }
    public double KillDeathRatio { get; set; }
    public string Country { get; set; } = string.Empty;
    public string RankName { get; set; } = "Unranked";
    public DateTime CreatedAt { get; set; }
}