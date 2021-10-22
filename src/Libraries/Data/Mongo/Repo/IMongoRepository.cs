using Data.Mongo.Collections;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Mongo.Repo
{
    public interface IMongoRepository<T> where T : MongoBaseDocument
    {
        List<T> GetAll();
        Task<T> GetByIdAsync(string id);
        
        Task<T> GetAsync(Expression<Func<T, bool>> predicate);
       
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

        Task AddAsync(T entity);
    
    }
}
