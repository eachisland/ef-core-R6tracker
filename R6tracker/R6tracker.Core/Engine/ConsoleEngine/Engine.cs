using System;
using R6tracker.Core.Models.Player;
using R6tracker.Infrastructure.Data.Models;
using R6Tracker.Core.Contracts;

namespace R6tracker.Core.Engine
{
    public class ConsoleEngine
    {
        private readonly IR6PlayerService _playerService;

        public ConsoleEngine(IR6PlayerService playerService)
        {
            _playerService = playerService;
        }

        public void Run()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== R6 TRACKER ===");
                Console.WriteLine("1. Add player");
                Console.WriteLine("2. View all players");
                Console.WriteLine("3. View player by ID");
                Console.WriteLine("4. Update player");
                Console.WriteLine("5. Remove player");
                Console.WriteLine("0. Exit");

                Console.Write("> ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1": AddPlayer(); break;
                    case "2": ListPlayers(); break;
                    case "3": ViewPlayerById(); break;
                    case "4": UpdatePlayer(); break;
                    case "5": RemovePlayer(); break;
                    case "0": return;
                    default:
                        Console.WriteLine("Invalid option.");
                        Console.ReadKey();
                        break;
                }
            }
        }
        public void AddPlayer()
        {
            Console.Write("Player name: ");
            string name = Console.ReadLine() ?? string.Empty;

            Console.Write("Platform: ");
            string platform = Console.ReadLine() ?? string.Empty;

            Console.Write("Country: ");
            string country = Console.ReadLine() ?? string.Empty;

            var model = new R6PlayerFormViewModel
            {
                PlayerName = name,
                Platform = platform,
                Country = country
            };

            _playerService.Add(model);

            Console.WriteLine("Player added!");
            Console.ReadKey();
        }


        public void RemovePlayer()
        {
            var players = _playerService.All();
            if (players == null || players.Count == 0)
            {
                Console.WriteLine("No players found.");
            }
            else
            {
                foreach (var p in players)
                {
                    Console.WriteLine($"ID: {p.Id} | Name: {p.PlayerName} | Platform: {p.Platform} | Country: {p.Country} | Level: {p.Level}");
                }
            }
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        private void UpdatePlayer()
        {
            Console.Clear();
            Console.WriteLine("=== Update Player ===");

            Console.Write("Enter Player ID to update: ");
            string? id = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(id))
            {
                Console.WriteLine("Invalid ID.");
                Console.ReadKey();
                return;
            }

            R6Player player;
            try
            {
                player = _playerService.GetById(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"Current Name: {player.PlayerName}");
            Console.Write("New Name (leave empty to keep current): ");
            string? name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name))
            {
                player.PlayerName = name;
            }

            Console.WriteLine($"Current Platform: {player.Platform}");
            Console.Write("New Platform (leave empty to keep current): ");
            string? platform = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(platform))
            {
                player.Platform = platform;
            }

            Console.WriteLine($"Current Country: {player.Country}");
            Console.Write("New Country (leave empty to keep current): ");
            string? country = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(country))
            {
                player.Country = country;
            }

            Console.WriteLine($"Current Level: {player.Level}");
            Console.Write("New Level (leave empty to keep current): ");
            string? levelInput = Console.ReadLine();
            if (int.TryParse(levelInput, out int level))
            {
                player.Level = level;
            }

            Console.WriteLine($"Current Matches Played: {player.MatchesPlayed}");
            Console.Write("New Matches Played (leave empty to keep current): ");
            string? matchesInput = Console.ReadLine();
            if (int.TryParse(matchesInput, out int matches))
            {
                player.MatchesPlayed = matches;
            }

            Console.WriteLine($"Current Kills: {player.Kills}");
            Console.Write("New Kills (leave empty to keep current): ");
            string? killsInput = Console.ReadLine();
            if (int.TryParse(killsInput, out int kills))
            {
                player.Kills = kills;
            }

            Console.WriteLine($"Current Deaths: {player.Deaths}");
            Console.Write("New Deaths (leave empty to keep current): ");
            string? deathsInput = Console.ReadLine();
            if (int.TryParse(deathsInput, out int deaths))
            {
                player.Deaths = deaths;
            }

            if (player.Deaths == 0)
            {
                player.KillDeathRatio = player.Kills;
            }
            else
            {
                player.KillDeathRatio = (double)player.Kills / player.Deaths;
            }

            try
            {
                _playerService.Update(player);
                Console.WriteLine("Player updated successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Update failed: {ex.Message}");
            }

            Console.ReadKey();
        }
        public void ListPlayers()
        {
            var players = _playerService.AllAsNoTracking();

            if (players.Count == 0)
            {
                Console.WriteLine("No players found.");
                Console.ReadKey();
                return;
            }

            foreach (var player in players)
            {
                Console.WriteLine($"ID: {player.Id}");
                Console.WriteLine($"Name: {player.PlayerName}");
                Console.WriteLine($"Platform: {player.Platform}");
                Console.WriteLine($"Country: {player.Country}");
                Console.WriteLine($"Level: {player.Level}");
                Console.WriteLine($"Matches: {player.MatchesPlayed}");
                Console.WriteLine($"Kills: {player.Kills}");
                Console.WriteLine($"Deaths: {player.Deaths}");
                Console.WriteLine(new string('-', 30)); 
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private void ViewPlayerById()
        {
            Console.Clear();
            Console.WriteLine("=== View Player by ID ===");

            Console.Write("Enter Player ID: ");
            string? id = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(id))
            {
                Console.WriteLine("Invalid ID.");
                Console.ReadKey();
                return;
            }

            R6Player player;
            try
            {
                player = _playerService.GetById(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"Player Name: {player.PlayerName}");
            Console.WriteLine($"Platform: {player.Platform}");
            Console.WriteLine($"Country: {player.Country}");
            Console.WriteLine($"Level: {player.Level}");
            Console.WriteLine($"Matches Played: {player.MatchesPlayed}");
            Console.WriteLine($"Kills: {player.Kills}");
            Console.WriteLine($"Deaths: {player.Deaths}");
            Console.WriteLine($"K/D Ratio: {player.KillDeathRatio:F2}");

            Console.ReadKey();
        }

    }
}
