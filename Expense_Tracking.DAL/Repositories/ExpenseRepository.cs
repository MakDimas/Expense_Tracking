using Expense_Tracking.DAL.Interfaces;
using Expense_Tracking.Domain.Entity;

namespace Expense_Tracking.DAL.Repositories
{
    public class ExpenseRepository : IBaseRepository<Expense>
    {
        private readonly AppDbContext _dbContext;

        public ExpenseRepository(AppDbContext ctx)
        {
            _dbContext = ctx;
        }

        public async Task Create(Expense entity)
        {
            await _dbContext.Expenses.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(Expense entity)
        {
            _dbContext.Expenses.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public IQueryable<Expense> GetAll() => _dbContext.Expenses;

        public async Task<Expense> Update(Expense entity)
        {
            _dbContext.Expenses.Update(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }
    }
}