using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirstCall.Domain.Contracts;
using FirstCall.Domain.Entities.Products;

namespace FirstCall.Application.Interfaces.Repositories
{
    public interface IRepositoryAsync<T, in TId> where T : class, IEntity<TId>
    {
        IQueryable<T> Entities { get; }

        Task<T> GetByIdAsync(TId id);

        Task<List<T>> GetAllAsync();

        Task<List<T>> GetPagedResponseAsync(int pageNumber, int pageSize);


        //IQueryable<Product> FromProductSqlRaw(string sql, params object[] parameters);

        Task<List<Dictionary<string, object>>> FromSqlRaw(string sql);

        Task<T> AddAsync(T entity);

        Task AddRangeAsync(IEnumerable<T> entities);
        Task DeleteRangeAsync(IEnumerable<T> entities);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);
    }
}