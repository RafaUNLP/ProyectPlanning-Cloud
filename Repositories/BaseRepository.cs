using System.Linq.Expressions;
using backend.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories
{
    public class BaseRepository<TEntity> where TEntity : class//idatarep?
    {
        protected readonly AppDbContext _context;
        public BaseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsyncWithIncludes(string includes = "")
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();
            foreach (var include in includes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(include);
            }
            return await query.ToListAsync();
        }

        public async Task<TEntity?> GetAsync(Guid id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity?> GetAsync(string nombre)
        {
            return await _context.Set<TEntity>().FindAsync(nombre);
        }

        public async Task<TEntity?> GetAsyncWithIncludes(Guid id, string includes = "")
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();
            foreach (var include in includes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(include);
            }
            return await query.FirstOrDefaultAsync(entity => EF.Property<Guid>(entity, "Id") == id);
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity dbEntity, TEntity entity)
        {
            _context.Entry(dbEntity).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            bool result = false;
            var entity = await _context.Set<TEntity>().FindAsync(id);
            if (entity != null)
            {
                _context.Set<TEntity>().Remove(entity);
                result = true;
            }
            await _context.SaveChangesAsync();

            return result;
        }

        public async Task<IEnumerable<TEntity>> FilterAsync(
            Expression<Func<TEntity, bool>>? filtro = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includes = "")
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();
            if (filtro != null)
            {
                query = query.Where(filtro);
            }
            foreach (var include in includes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(include);
            }
            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> FilterAsyncPaginated(
            Expression<Func<TEntity, bool>>? filtro = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includes = "",
            int page = 0,
            int count = 10)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();
            if (filtro != null)
            {
                query = query.Where(filtro);
            }
            foreach (var include in includes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(include);
            }
            if (page >= 0 && count >= 0)
            {
                query = query.Skip(page * count).Take(count);
            }
            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            return await query.ToListAsync();
        }

        public async Task<bool> Exist(Expression<Func<TEntity, bool>> filtro)
        {
            return await _context.Set<TEntity>().AnyAsync(filtro);
        }

    }
}