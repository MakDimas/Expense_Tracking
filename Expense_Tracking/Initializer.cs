using Expense_Tracking.DAL.Interfaces;
using Expense_Tracking.DAL.Repositories;
using Expense_Tracking.Domain.Entity;
using Expense_Tracking.Service.Implementations;
using Expense_Tracking.Service.Interfaces;

namespace Expense_Tracking
{
    public static class Initializer
    {
        // Добавление репозиториев
        public static void InitializeRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBaseRepository<User>, UserRepository>();
            services.AddScoped<IBaseRepository<Expense>, ExpenseRepository>();
        }

        // Добавление сервисов
        public static void InitializeServices(this IServiceCollection services)
        {
            services.AddScoped<IExpenseService, ExpenseService>();
            services.AddScoped<IUserService, UserService>();
        }
    }
}