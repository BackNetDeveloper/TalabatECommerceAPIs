using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext context;
        private Hashtable _Repositories;

        public UnitOfWork(StoreDbContext context)
        {
            this.context = context;
        }
        public async Task<int> Complete()
        =>await context.SaveChangesAsync();

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            if(_Repositories is null)
                _Repositories = new Hashtable();
            var TEntityName = typeof(TEntity).Name;
            if (!_Repositories.ContainsKey(TEntityName))
            {
                var RepositoryType = typeof(GenericRepository<>);
                var ReposiroryInstance = Activator.CreateInstance(RepositoryType.MakeGenericType(typeof(TEntity)),context);
                _Repositories.Add(TEntityName, ReposiroryInstance);
            }
            return (IGenericRepository<TEntity>)_Repositories[TEntityName];
        }
    }
}
