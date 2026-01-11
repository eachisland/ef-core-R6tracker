namespace R6tracker.Core.Contracts.Common
{
    public interface IReadableAll<TReturnModel> where TReturnModel : class
    {
        List<TReturnModel> All();
        List<TReturnModel> AllAsNoTracking();
    }
}