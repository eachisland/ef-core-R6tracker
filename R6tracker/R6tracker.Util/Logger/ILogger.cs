namespace R6tracker.Util.Logger
{
    public interface ILogger<TService> where TService : class
    {
        void LogDebug<TObject>(LogModel model, TObject? @object) where TObject : class;
        void LogInfo(LogModel model);
        void LogWarning(LogModel model);
        void LogError(LogModel model);
        void LogCritical(LogModel model);
    }
}
