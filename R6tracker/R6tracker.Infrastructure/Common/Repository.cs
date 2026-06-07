using Microsoft.EntityFrameworkCore;
using R6tracker.Infrastructure.Data;

namespace R6tracker.Infrastructure.Common
{
    public class Repository : IRepository
    {
        private readonly DbContext _context;

        public Repository(R6trackerDbContext context)
        {
            _context = context;
        }

        private DbSet<T> DbSet<T>() where T : class => _context.Set<T>();

        public IQueryable<T> All<T>() where T : class
            => DbSet<T>();

        public IQueryable<T> AllAsNoTracking<T>() where T : class
            => DbSet<T>().AsNoTracking();

        public async Task AddAsync<T>(T entity) where T : class
            => await DbSet<T>().AddAsync(entity);

        public void Add<T>(T entity) where T : class
            => DbSet<T>().Add(entity);

        public void Update<T>(T entity) where T : class
            => DbSet<T>().Update(entity);

        public void Delete<T>(T entity) where T : class
            => DbSet<T>().Remove(entity);

        public void Delete<T>(string id) where T : class
        {
            var entity = DbSet<T>().Find(id);
            if (entity == null) throw new NullReferenceException($"Entity with id {id} not found.");
            DbSet<T>().Remove(entity);
        }

        public async Task<int> SaveChangesAsync()
            => await _context.SaveChangesAsync();

        public int SaveChanges()
            => _context.SaveChanges();

        public void Dispose() => _context.Dispose();
    }
}