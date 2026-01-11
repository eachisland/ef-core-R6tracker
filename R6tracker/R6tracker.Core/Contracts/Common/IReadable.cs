namespace R6tracker.Core.Contracts.Common
{
    public interface IReadable<TReturnModel> where TReturnModel : class
    {
        TReturnModel GetById(string id);
    }
}