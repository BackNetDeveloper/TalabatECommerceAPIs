using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreDbContext context;

        public GenericRepository(StoreDbContext context)
        {
            this.context = context;
        }
        public async Task<IReadOnlyList<T>> GetAllAsync()
      => await context.Set<T>().ToListAsync();

        public async Task<T> GetByIdAsync(int id)
        => await context.Set<T>().FindAsync(id);
        public void Add(T entity)
        =>context.Set<T>().Add(entity);
        public void Update(T entity)
       => context.Set<T>().Update(entity);

        public void Delete(T entity)
         => context.Set<T>().Remove(entity);

      
        public async Task<T> GetEntityBySpecifications(ISpecifications<T> specifications)
        => await ApplySpecification(specifications).FirstOrDefaultAsync();

        public async Task<IReadOnlyList<T>> ListAsync(ISpecifications<T> specifications)
         => await ApplySpecification(specifications).ToListAsync();
        private IQueryable<T> ApplySpecification(ISpecifications<T> specifications)
            => SpecificationEvaluator<T>.GetQuery(context.Set<T>().AsQueryable(), specifications);

        public async Task<int> CountAsync(ISpecifications<T> specifications)
        => await ApplySpecification(specifications).CountAsync();
    }
}
