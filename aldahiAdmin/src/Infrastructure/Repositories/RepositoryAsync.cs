using FirstCall.Application.Features.Products.Queries.GetAllPaged;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Domain.Contracts;
using FirstCall.Domain.Entities.Products;
using FirstCall.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstCall.Infrastructure.Repositories
{
    public class RepositoryAsync<T, TId> : IRepositoryAsync<T, TId> where T : AuditableEntity<TId>
    {
        private readonly BlazorHeroContext _dbContext;

        public RepositoryAsync(BlazorHeroContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<T> Entities => _dbContext.Set<T>().Where(x => !x.IsDeleted).OrderByDescending(x => x.Id);

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbContext.Set<T>().AddRangeAsync(entities);
        }

        public Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbContext
                .Set<T>()
                .ToListAsync();
        }

        //public async Task<List<GetAllPagedProductsResponse>> FromProductSqlRaw(string sql)
        //{
        //    //return _dbContext.Set<T>().FromSqlRaw(sql, parameters);

        //    using var connection = _dbContext.Database.GetDbConnection();
        //    await connection.OpenAsync();

        //    using var command = connection.CreateCommand();
        //    command.CommandText = sql;

        //    using var reader = await command.ExecuteReaderAsync();

        //    var results = new List<GetAllPagedProductsResponse>();

        //    while (await reader.ReadAsync())
        //    {
        //        var product = new GetAllPagedProductsResponse
        //        {
        //            Id = reader.GetInt32(reader.GetOrdinal("Id")),
        //            NameAr = reader.GetString(reader.GetOrdinal("NameAr")),
        //            NameEn = reader.GetString(reader.GetOrdinal("NameEn")),
        //            Price = reader.GetDecimal(reader.GetOrdinal("Price")),
        //             KindNameAr = reader.GetString(reader.GetOrdinal("KindNameAr")),
        //             KindNameEn = reader.GetString(reader.GetOrdinal("KindNameEn")),
        //            // Add other properties as needed
        //        };

        //        results.Add(product);
        //    }


        //    return results;

        //}


        public async Task<List<Dictionary<string, object>>> FromSqlRaw(string sql)
        {
            //return _dbContext.Set<T>().FromSqlRaw(sql, parameters);

            using var connection = _dbContext.Database.GetDbConnection();
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = sql;

            using var reader = await command.ExecuteReaderAsync();

            var results = new List<Dictionary<string, object>>();

            while (await reader.ReadAsync())
            {
                var row = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row[reader.GetName(i)] = reader.GetValue(i);
                }
                results.Add(row);
            }

            return results;

        }

        public async Task<T> GetByIdAsync(TId id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> GetPagedResponseAsync(int pageNumber, int pageSize)
        {
            return await _dbContext
                .Set<T>()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public Task UpdateAsync(T entity)
        {
            T exist = _dbContext.Set<T>().Find(entity.Id);
            _dbContext.Entry(exist).CurrentValues.SetValues(entity);
            return Task.CompletedTask;
        }
    }
}