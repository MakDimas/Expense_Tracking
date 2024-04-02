using Expense_Tracking.Domain.Entity;
using Expense_Tracking.Domain.Response;
using Expense_Tracking.Domain.ViewModel._UserAccount;
using Expense_Tracking.Domain.ViewModel.User;

namespace Expense_Tracking.Service.Interfaces
{
    public interface IUserService
    {
        public Task<User?> GetCurrentUser();

        public Task<IBaseResponse<UserAccountViewModel>> GetUserViewModel();

        public Task<IBaseResponse<User>> UpdateUser(UpdateUserViewModel model);

        public Task<IBaseResponse<User>> CountUpdate();
    }
}