namespace R6tracker.Infrastructure.Common
{
    public interface IRepository
    {
        IQueryable<T> All<T>() where T : class;
        IQueryable<T> AllAsNoTracking<T>() where T : class;
        T GetById<T>(string id) where T : class;
        void Add<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void Delete<T>(string id) where T : class;
        void SaveChanges();
    }
}
