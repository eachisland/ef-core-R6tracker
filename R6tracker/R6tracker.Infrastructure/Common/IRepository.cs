namespace R6tracker.Infrastructure.Common
{
    public interface IRepository : IDisposable
    {
        IQueryable<T> All<T>() where T : class;
        IQueryable<T> AllAsNoTracking<T>() where T : class;
        Task AddAsync<T>(T entity) where T : class;
        void Add<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        void Delete<T>(string id) where T : class;
        Task<int> SaveChangesAsync();
        int SaveChanges();
    }
}