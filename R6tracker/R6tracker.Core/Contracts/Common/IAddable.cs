namespace R6tracker.Core.Contracts.Common
{
    public interface IAddable<TEntity> where TEntity : class
    {
        void Add(TEntity entity);
    }
}