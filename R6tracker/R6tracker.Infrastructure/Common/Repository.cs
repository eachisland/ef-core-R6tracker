using Microsoft.EntityFrameworkCore;
using R6tracker.Infrastructure.Data;

namespace R6tracker.Infrastructure.Common
{
    public class Repository : IRepository
    {
        private readonly R6trackerDbContext _context;

        public Repository(R6trackerDbContext context)
        {
            _context = context;
        }

        private DbSet<T> DbSet<T>() where T : class
        {
            return _context.Set<T>();
        }

        public void Add<T>(T entity) where T : class
        {
            DbSet<T>().Add(entity);
        }

        public IQueryable<T> All<T>() where T : class
        {
            return DbSet<T>();
        }

        public IQueryable<T> AllAsNoTracking<T>() where T : class
        {
            return DbSet<T>().AsNoTracking();
        }

        public void Delete<T>(string id) where T : class
        {
            T entity = GetById<T>(id);
            DbSet<T>().Remove(entity);
        }

        public T GetById<T>(string id) where T : class
        {
            T? result = DbSet<T>().Find(id);

            if (result == null)
            {
                throw new NullReferenceException();
            }

            return result;
        }

        public void Update<T>(T entity) where T : class
        {
            DbSet<T>().Update(entity);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
