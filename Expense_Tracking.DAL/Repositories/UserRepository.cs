using Expense_Tracking.DAL.Interfaces;
using Expense_Tracking.Domain.Entity;

namespace Expense_Tracking.DAL.Repositories
{
    public class UserRepository : IBaseRepository<User>
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext ctx)
        {
            _dbContext = ctx;
        }

        public async Task Create(User entity)
        {
            await _dbContext.Users.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(User entity)
        {
            _dbContext.Users.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public IQueryable<User> GetAll() => _dbContext.Users;

        public async Task<User> Update(User entity)
        {
            _dbContext.Users.Update(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }
    }
}