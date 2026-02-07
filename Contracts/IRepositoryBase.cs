using System.Linq.Expressions;

namespace Contracts
{
    public interface IRepositoryBase<T>
    {
        //Generic repository that will provide the CRUD methods.
        //As a result, all the methods can be called upon any repository class in our project.
        IQueryable<T> FindAll(bool trackChanges); 
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges); 
        void Create(T entity); 
        void Update(T entity); 
        void Delete(T entity);
    }
}
