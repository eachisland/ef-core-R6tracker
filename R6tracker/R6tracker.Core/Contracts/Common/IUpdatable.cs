namespace R6tracker.Core.Contracts.Common
{
    public interface IUpdateble<TUpdateModel> where TUpdateModel : class
    {
        void Update(TUpdateModel model);
    }
}