using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Bulky_Book.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        //Get class T passing an id
        T Get(int id);
        //Get all elements 
        IEnumerable<T> GetAll(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null
            );
        //Get the first entity that satisfied the expression.
        T GetFirstOrDefault(
            Expression<Func<T, bool>> filter = null,
            string includeProperties = null
            );
        //Add an entity to the collection of elements
        void Add(T entity);
        //Remove an entity of the collection of elements (id)
        void Remove(int id);
        //Remove an entity of the collection of elements
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
