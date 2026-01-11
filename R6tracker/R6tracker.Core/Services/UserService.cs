using R6tracker.Core.Contracts;
using R6tracker.Core.Models.Player;
using R6tracker.Infrastructure.Common;
using R6tracker.Infrastructure.Data.Models;
using R6tracker.Core.Exceptions;
using R6Tracker.Core.Contracts;

namespace R6tracker.Core.Services
{
    public class R6PlayerService : IR6PlayerService
    {
        private readonly IRepository _repo;

        public R6PlayerService(IRepository repo)
        {
            _repo = repo;
        }
        public void Add(R6PlayerFormViewModel entity)
        {
            List<R6Player> players = _repo.AllAsNoTracking<R6Player>()
                .Where(p => p.PlayerName == entity.PlayerName &&
                            p.Platform == entity.Platform)
                .ToList();

            if (players.Count > 0)
            {
                throw new InvalidOperationException("Player already exists.");
            }

            R6Player player = new()
            {
                PlayerName = entity.PlayerName,
                Platform = entity.Platform,
                Level = entity.Level,
                MatchesPlayed = entity.MatchesPlayed,
                Kills = entity.Kills,
                Deaths = entity.Deaths,
                Country = entity.Country
            };

            _repo.Add(player);
            _repo.SaveChanges();
        }
        public R6Player GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new NullReferenceException();
            }

            R6Player player = _repo.GetById<R6Player>(id);

            if (player == null)
            {
                throw new NotFoundException();
            }

            return player;
        }
        public bool Remove(string id)
        {
            try
            {
                _repo.Delete<R6Player>(id);
            }
            catch (NullReferenceException)
            {
                return false;
            }

            return true;
        }
        public void Update(R6Player model)
        {
            _repo.Update(model);
            _repo.SaveChanges();
        }
        public List<R6Player> All()
        {
            return _repo.All<R6Player>().ToList();
        }

        public List<R6Player> AllAsNoTracking()
        {
            return _repo.AllAsNoTracking<R6Player>().ToList();
        }
    }
}