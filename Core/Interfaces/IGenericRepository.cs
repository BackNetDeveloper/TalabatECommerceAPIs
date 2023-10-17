using Core.Entities;
using Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IGenericRepository<T> where T :BaseEntity
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<T> GetEntityBySpecifications(ISpecifications<T> specifications);
        Task<IReadOnlyList<T>> ListAsync(ISpecifications<T> specifications);
        Task<int> CountAsync(ISpecifications<T> specifications);
    }
}
