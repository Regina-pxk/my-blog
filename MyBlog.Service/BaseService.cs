using MyBlog.IRespository;
using MyBlog.IService;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MyBlog.Service
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class, new()
    {
        public IBaseRespository<TEntity> _baseRespository;
        public async Task<bool> CreateAsync(TEntity entity)
        {
            return await _baseRespository.CreateAsync(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _baseRespository.DeleteAsync(id);
        }

        public async Task<bool> EditAsync(TEntity entity)
        {
            return await _baseRespository.EditAsync(entity);
        }

        public async Task<TEntity> FindAsync(int id)
        {
            return await _baseRespository.FindAsync(id);
        }

        public async Task<TEntity> FindAsync( Expression<Func<TEntity, bool>> func)
        {
            return await _baseRespository.FindAsync(func);
        }
        public async Task<List<TEntity>> QueryAsync()
        {
            return await _baseRespository.QueryAsync();
        }

        public async Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> func)
        {
            return await _baseRespository.QueryAsync(func);
        }

        public async Task<List<TEntity>> QueryAsync(int page, int size, RefAsync<int> total)
        {
            return await _baseRespository.QueryAsync(page, size, total);
        }

        public async Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> func, int page, int size, RefAsync<int> total)
        {
            return await _baseRespository.QueryAsync(func, page, size, total);
        }
    }
}
