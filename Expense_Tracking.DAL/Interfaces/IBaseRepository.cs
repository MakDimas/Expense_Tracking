namespace Expense_Tracking.DAL.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task Create(T entity);

        IQueryable<T> GetAll();

        Task<T> Update(T entity);

        Task Delete(T entity);
    }
}