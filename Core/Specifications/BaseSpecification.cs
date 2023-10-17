using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class BaseSpecification<T> : ISpecifications<T>
    {
        public BaseSpecification(Expression<Func<T, bool>> condition)
        {
            Condition = condition;
        }
        public Expression<Func<T, bool>> Condition { get; }

        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();

        protected void AddInclude(Expression<Func<T, object>> IncludeExpression)
        => Includes.Add(IncludeExpression);
        public Expression<Func<T, object>> OrderBy { get;private set; }

        protected void AddOrderBy(Expression<Func<T, object>> OrderByExpression)
        => OrderBy  = OrderByExpression;
        public Expression<Func<T, object>> OrderByDesending { get;private set; }

        protected void AddOrderByDesending(Expression<Func<T, object>> OrderByDesendingExpression)
        => OrderByDesending = OrderByDesendingExpression;

        public int Take { get; private set; }

        public int Skip { get; private set; }

        public bool IsPagingEnabled { get; private set; }

        protected void ApplyPaging( int skip,int take )
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }
    }
}
