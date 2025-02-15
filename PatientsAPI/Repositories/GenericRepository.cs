using Microsoft.EntityFrameworkCore;
using PatientsAPI.Data;

namespace PatientsAPI.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            this._context = context;
            this._dbSet = this._context.Set<T>();
        }

        public async Task<T> GetById(int id)
        {
            return await this._dbSet.FindAsync(id);
        }

        public async Task<bool> Exists(int id)
        {
            return await this._dbSet.FindAsync(id) != null;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await this._dbSet.ToListAsync();
        }

        public async Task Add(T entity)
        {
            await this._dbSet.AddAsync(entity);
            await this._context.SaveChangesAsync();
        }

        public async Task Update(T entity)
        {
            this._context.Entry(entity).State = EntityState.Modified;
            await this._context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var entity = await this._context.Set<T>().FindAsync(id);
            this._context.Set<T>().Remove(entity);
            await this._context.SaveChangesAsync();
        }
    }
}
