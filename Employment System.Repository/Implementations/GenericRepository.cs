using Employment_System.Domain.IRepositories;
using Employment_System.Domain.ISpecification;
using Employment_System.Repository.Data;
using Employment_System.Repository.Implementations.Specification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employment_System.Repository.Implementations
{
    public  class GenericRepository<T> : IGenericRepository<T> where T:class
    {
        private readonly EmpManageDbContext Context;
        public GenericRepository(EmpManageDbContext context)
        {
            Context = context;
        }

        public async Task<T> CreateAsync(T entity)
        {
            await Context.Set<T>().AddAsync(entity);
            Context.SaveChangesAsync();
            return entity;

        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await Context.Set<T>().FindAsync(id);
            if (entity == null) return false;

            // Remove the entity
            Context.Set<T>().Remove(entity);

            // Save changes asynchronously
            await Context.SaveChangesAsync();

            return true; // Deletion successful
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
            //{
            //    if (typeof(T) == typeof(Product)  // Mosken solved by specisificaiton Design Pattern  that use to build this query Context.Set<Product>().Include(T => T.ProductType).Include(P => P.ProductBrand).ToListAsync();!
            //       return await Context.Set<Product>().Include(T => T.ProductType).Include(P => P.ProductBrand).ToListAsync();

            //    return await Context.Set<T>().ToListAsync();
            //}

            => await Context.Set<T>().ToListAsync();

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)

            => await Context.Set<T>().FindAsync(id);


        public async Task<T> GetByIdWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public bool UpdateAsync(int id,T entity)
        {
            if (GetByIdAsync(id)!=null)
            {

                Context.Set<T>().Update(entity); // Update things which change 
                                                 // Context.Entity(entity).State = EntityState.Modified; // Update All Which Change and NotChange
                Context.SaveChanges();
                return true;
            }

            return false;

        }
        public  IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {

            return SpecificationEvalutions<T>.CreateQuery(Context.Set<T>(), spec);
        }


    }
}
