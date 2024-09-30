using Employment_System.Domain.ISpecification;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace TaskEF.Specifications
{
    public class SpacificationEvaluation<TEntity> where TEntity : class
    {
        // inputQuery reperesnt Context.set<TEntity>()  
        public static IQueryable<TEntity> CreateQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec)
        {
            var query = inputQuery; //Context.set<TEntity>() 
                

            if (spec.Criteria != null)
            {  // p => p.id = 10

                query = query.Where(spec.Criteria);
            }

            // Context.Set<Product>().Include(T => T.ProductType)
            // Context.Set<Product>().Include(T => T.ProductType).Include(P => P.ProductBrand)
            query = spec.Includes.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));

            return query;
        }
    }
}
