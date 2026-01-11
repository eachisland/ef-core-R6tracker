using System;
using Microsoft.EntityFrameworkCore;
using R6tracker.Core.Engine;
using R6tracker.Core.Services;
using R6tracker.Infrastructure.Common;
using R6tracker.Infrastructure.Data;
using R6tracker.Util.Logger;

namespace R6tracker
{
    class Program
    {
        static void Main(string[] args)
        {
            ILogger<Program> logger = new Logger<Program>();

            logger.LogInfo(new LogModel
            {
                Message = "R6Tracker application started",
                ServiceName = nameof(Program),
                MethodName = nameof(Main)
            });

            var context = new R6trackerDbContext();
            var repo = new Repository(context);
            var playerService = new R6PlayerService(repo);
            var engine = new ConsoleEngine(playerService);

            engine.Run();
        }
    }
}
