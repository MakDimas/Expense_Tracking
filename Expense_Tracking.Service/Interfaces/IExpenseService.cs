using Expense_Tracking.Domain.Entity;
using Expense_Tracking.Domain.Response;
using Expense_Tracking.Domain.ViewModel._UserAccount;
using Expense_Tracking.Domain.ViewModel.Expense;

namespace Expense_Tracking.Service.Interfaces
{
    public interface IExpenseService
    {
        public Task<IBaseResponse<Expense>> Create(CreateExpenseViewModel model);

        public Task<IBaseResponse<double[,]>> GetStatistic(FilterViewModel model);

        public Task<IBaseResponse<decimal>> GetAllExpenses();

        public Task<IBaseResponse<CreateAndFilterViewModel>> GetLastExpense();

        public string CalculatePercValue(double value);
    }
}