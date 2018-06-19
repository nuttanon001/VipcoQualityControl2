using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace VipcoQualityControl.Services
{
    public interface IRepositoryMachine<TEntity> where TEntity : class
    {
        TEntity Get(string id, bool option = false);
        TEntity Get(int id, bool option = false);
        Task<TEntity> GetAsync(int id, bool option = false);
        Task<TEntity> GetAsync(string id, bool option = false);
        IQueryable<TEntity> GetAllAsQueryable();
        Task<ICollection<TEntity>> GetAllAsync(bool option = false);
        Task<ICollection<TEntity>> GetAllWithConditionAsync(Expression<Func<TEntity, bool>> match = null, bool option = false);
        TEntity Find(Expression<Func<TEntity, bool>> match, bool option = false);
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> match, bool option = false);
        ICollection<TEntity> FindAll(Expression<Func<TEntity, bool>> match, bool option = false);
        Task<ICollection<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> match, bool option = false);
        TEntity Add(TEntity nTEntity);
        Task<TEntity> AddAsync(TEntity nTEntity);
        Task<IEnumerable<TEntity>> AddAllAsync(IEnumerable<TEntity> nTEntityList);
        Task<TEntity> UpdateAsync(TEntity updated, int key);
        Task<TEntity> UpdateAsync(TEntity updated, string key);
        TEntity Update(TEntity updated, string key);
        TEntity Update(TEntity updated, int key);
        void Delete(int key);
        void Delete(string key);
        Task<int> DeleteAsync(int key);
        Task<int> DeleteAsync(string key);
        Task<int> CountAsync();
        int CountWithMatch(Expression<Func<TEntity, bool>> match);
        Task<int> CountWithMatchAsync(Expression<Func<TEntity, bool>> match);
        Task<bool> AnyDataAsync(Expression<Func<TEntity, bool>> match);
    }
}
