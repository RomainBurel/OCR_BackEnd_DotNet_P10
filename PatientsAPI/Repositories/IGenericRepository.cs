namespace PatientsAPI.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        public Task<T> GetById(int id);

        public Task<bool> Exists(int id);

        public Task<IEnumerable<T>> GetAll();

        public Task Add(T entity);

        public Task Update(T entity);

        public Task Delete(int id);
    }
}
