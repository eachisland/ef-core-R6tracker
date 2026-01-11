using R6tracker.Core.Contracts.Common;
using R6tracker.Core.Models.Player;
using R6tracker.Infrastructure.Data.Models;


namespace R6Tracker.Core.Contracts
{
    public interface IR6PlayerService : 
        IAddable<R6PlayerFormViewModel>,
        IUpdateble<R6Player>,
        IReadable<R6Player>,
        IReadableAll<R6Player>,
        IRemovable
    {
        
    }
}