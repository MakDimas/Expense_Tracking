#nullable disable

using Expense_Tracking.DAL.Interfaces;
using Expense_Tracking.Domain.Entity;
using Expense_Tracking.Domain.Response;
using Expense_Tracking.Domain.ViewModel._UserAccount;
using Expense_Tracking.Domain.ViewModel.User;
using Expense_Tracking.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Expense_Tracking.Service.Implementations
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly UserManager<User> _manager;
        private readonly HttpContext _context;
        private readonly IBaseRepository<User> _userRepository;

        public UserService(UserManager<User> user, IHttpContextAccessor ctx, ILogger<UserService> logger, IBaseRepository<User> repo)
        {
            _manager = user;
            _context = ctx.HttpContext;
            _logger = logger;
            _userRepository = repo;
        }

        // Получение текущего пользователя из контекста
        public async Task<User> GetCurrentUser() => await _manager.Users.Include(u => u.Expense).FirstOrDefaultAsync(u => u.UserName == _context.User.Identity.Name);

        // Обновление данных о тратах пользователя
        public async Task<IBaseResponse<UserAccountViewModel>> GetUserViewModel()
        {
            try
            {
                _logger.LogInformation("[UserService.GetUserViewModel]: Starting - {Now}", DateTime.Now);

                var user = await GetCurrentUser();

                if(user == null)
                {
                    _logger.LogInformation("[UserService.GetUserViewModel]: User was null");

                    return new BaseResponse<UserAccountViewModel>()
                    {
                        Description = "User was null",
                        StatusCode = Domain.Enum.StatusCode.UserNotFound
                    };
                }

                var userModel = new UserAccountViewModel()
                {
                    User = user,
                    Money = (decimal)user.DayLimit - user.Expense
                            .Where(x => x.Created.Date == DateTime.Today)
                            .Sum(x => x.Sum)
                };

                _logger.LogInformation("[UserService.GetUserViewModel]: UserAccountViewModel has been retrieved");

                return new BaseResponse<UserAccountViewModel>()
                {
                    Data = userModel,
                    StatusCode = Domain.Enum.StatusCode.OK
                };
            }
            catch (Exception e)
            {
                _logger.LogInformation(e, "[UserService.GetUserViewModel]: {Message}", e.Message);

                return new BaseResponse<UserAccountViewModel>()
                {
                    StatusCode = Domain.Enum.StatusCode.InternalServerError
                };
            }
        }

        // Обновление данных пользователя
        public async Task<IBaseResponse<User>> UpdateUser(UpdateUserViewModel model)
        {
            try
            {
                _logger.LogInformation("[UserService.UpdateUser]: Starting update: {Now}", DateTime.Now);

                var user = await _manager.GetUserAsync(_context.User);

                if (user == null)
                {
                    return new BaseResponse<User>()
                    {
                        Description = "User was null",
                        StatusCode = Domain.Enum.StatusCode.UserNotFound
                    };
                }

                user.Name = model.Name;
                user.Surname = model.Surname;
                user.DayLimit = model.DayLimit;

                await _userRepository.Update(user);

                _logger.LogInformation("[UserService.UpdateUser]: The user has been updated");

                return new BaseResponse<User>()
                {
                    StatusCode = Domain.Enum.StatusCode.OK,
                };
            }
            catch (Exception e)
            {
                _logger.LogInformation(e, "[UserService.UpdateUser]: {Message}", e.Message);

                return new BaseResponse<User>()
                {
                    StatusCode = Domain.Enum.StatusCode.InternalServerError
                };
            }
        }

        // Обновление суммы пользователя, потраченной за весь период
        public async Task<IBaseResponse<User>> CountUpdate()
        {
            try
            {
                _logger.LogInformation("[UserService.CountUpdate]: Count updating started - {Now}", DateTime.Now);

                var user = await GetCurrentUser();

                if (user == null)
                {
                    _logger.LogInformation("[UserService.CountUpdate]: User was null");

                    return new BaseResponse<User>()
                    {
                        Description = "User was null",
                        StatusCode = Domain.Enum.StatusCode.UserNotFound
                    };
                }

                user.Count = user.Expense.Sum(x => x.Sum);

                await _userRepository.Update(user);

                _logger.LogInformation("[UserService.CountUpdate]: Count was updated");

                return new BaseResponse<User>()
                {
                    StatusCode = Domain.Enum.StatusCode.OK
                };
            }
            catch (Exception e)
            {
                _logger.LogInformation(e, "[UserService.CountUpdate]: {Message}", e.Message);

                return new BaseResponse<User>()
                {
                    StatusCode = Domain.Enum.StatusCode.InternalServerError
                };
            }
        }
    }
}