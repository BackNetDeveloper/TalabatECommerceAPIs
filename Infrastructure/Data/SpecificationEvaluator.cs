using Core.Entities;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class SpecificationEvaluator<TEntity> where TEntity :BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputquery ,ISpecifications<TEntity> specifications)
        {
            var query = inputquery;
            if (specifications.Condition != null)
            query = query.Where(specifications.Condition);

            if (specifications.OrderBy != null)
                query = query.OrderBy(specifications.OrderBy);

            if (specifications.OrderByDesending != null)
                query = query.OrderByDescending(specifications.OrderByDesending);

            if (specifications.IsPagingEnabled)
                query = query.Skip(specifications.Skip).Take(specifications.Take);

            query = specifications.Includes.Aggregate(query, (current, include) => current.Include(include));
            return query;
        }
    }
}
