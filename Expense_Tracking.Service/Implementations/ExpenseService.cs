using Expense_Tracking.Domain.Entity;
using Expense_Tracking.Domain.Response;
using Expense_Tracking.Domain.ViewModel.Expense;
using Expense_Tracking.Service.Interfaces;
using Microsoft.Extensions.Logging;
using Expense_Tracking.DAL.Interfaces;
using Expense_Tracking.Domain.Enum;
using Expense_Tracking.Domain.ViewModel._UserAccount;

namespace Expense_Tracking.Service.Implementations
{
	public class ExpenseService : IExpenseService
	{
		private readonly IBaseRepository<Expense> _expRepository;
		private readonly ILogger<ExpenseService> _logger;
		private readonly IUserService _manager;

		public ExpenseService(ILogger<ExpenseService> log, IBaseRepository<Expense> repo, IUserService user)
        {
			_logger = log;
			_expRepository = repo;
			_manager = user;
        }

		// Создание Expense
        public async Task<IBaseResponse<Expense>> Create(CreateExpenseViewModel model)
		{
			try
			{
				_logger.LogInformation("[ExpenseService.Create]: Starting add new Expense - {Now}", DateTime.Now);

				var user = await _manager.GetCurrentUser();

				if (user == null)
				{
					_logger.LogInformation("[ExpenseService.Create]: User was null");

					return new BaseResponse<Expense>()
					{
						Description = "User was null",
						StatusCode = StatusCode.UserNotFound
					};
				}

				var expense = new Expense()
				{
					Sum = model.Sum,
					ExpenseType = model.ExpenseType,
					Created = DateTime.Now,
					User = user,
					UserId = user.Id.ToString()
				};

				await _expRepository.Create(expense);

				_logger.LogInformation("[ExpenseService.Create]: Expense added - {Now}", DateTime.Now);

				return new BaseResponse<Expense>()
				{
					Description = "Expense added",
					StatusCode = StatusCode.OK
				};
				
			}
			catch (Exception e)
			{
				_logger.LogInformation(e, "[ExpenseService.Create]: {Message}", e.Message);

				return new BaseResponse<Expense>()
				{
					StatusCode = StatusCode.InternalServerError
				};
			}
		}

		// Возвращает статистику
		public async Task<IBaseResponse<double[,]>> GetStatistic(FilterViewModel model)
		{
			try
			{
				_logger.LogInformation("[ExpenseService.GetStatistic]: Starting calculate statistic - {Now}", DateTime.Now);

				double[,] values = new double[6, 2];

				var user = await _manager.GetCurrentUser();

                if (user == null)
                {
					_logger.LogInformation("[ExpenseService.GetStatistic]: User was null");

                    return new BaseResponse<double[,]>()
                    {
                        Description = "User was null",
                        StatusCode = StatusCode.UserNotFound
                    };
                }

				double limit = 0;

				if (model.Created != 0 && model.Created != 1)
					limit = user.DayLimit * (model.Created + 1);

				else if (model.DateTime1 != DateTime.MinValue && model.DateTime2 != DateTime.MinValue)
					limit = user.DayLimit * (model.DateTime2.Subtract(model.DateTime1).Days);

				else
					limit = user.DayLimit;

                decimal food = user.Expense
					.Where(x => x.ExpenseType == ExpenseType.Food)
					.Where(x => model.Created == 0 || model.Created == 1? (model.DateTime1 == DateTime.MinValue || model.DateTime2 == DateTime.MinValue ? x.Created.Date == DateTime.Today.Date : x.Created >= model.DateTime1 && x.Created <= model.DateTime2) : x.Created >= DateTime.Today.AddDays(-model.Created) && x.Created <= DateTime.Today.AddDays(1))
					.Select(x => x.Sum)
					.Sum();

                decimal fun = user.Expense
					.Where(x => x.ExpenseType == ExpenseType.Fun)
					.Where(x => model.Created == 0 || model.Created == 1 ? (model.DateTime1 == DateTime.MinValue || model.DateTime2 == DateTime.MinValue ? x.Created.Date == DateTime.Today.Date : x.Created >= model.DateTime1 && x.Created <= model.DateTime2) : x.Created >= DateTime.Today.AddDays(-model.Created) && x.Created <= DateTime.Today.AddDays(1))
					.Select(x => x.Sum)
					.Sum();

				decimal clothes = user.Expense
					.Where(x => x.ExpenseType == ExpenseType.Clothes)
					.Where(x => model.Created == 0 || model.Created == 1 ? (model.DateTime1 == DateTime.MinValue || model.DateTime2 == DateTime.MinValue ? x.Created.Date == DateTime.Today.Date : x.Created >= model.DateTime1 && x.Created <= model.DateTime2) : x.Created >= DateTime.Today.AddDays(-model.Created) && x.Created <= DateTime.Today.AddDays(1))
					.Select(x => x.Sum)
					.Sum();

				decimal study = user.Expense
					.Where(x => x.ExpenseType == ExpenseType.Study)
					.Where(x => model.Created == 0 || model.Created == 1 ? (model.DateTime1 == DateTime.MinValue || model.DateTime2 == DateTime.MinValue ? x.Created.Date == DateTime.Today.Date : x.Created >= model.DateTime1 && x.Created <= model.DateTime2) : x.Created >= DateTime.Today.AddDays(-model.Created) && x.Created <= DateTime.Today.AddDays(1))
					.Select(x => x.Sum)
					.Sum();

				decimal medicine = user.Expense
					.Where(x => x.ExpenseType == ExpenseType.Medicine)
					.Where(x => model.Created == 0 || model.Created == 1 ? (model.DateTime1 == DateTime.MinValue || model.DateTime2 == DateTime.MinValue ? x.Created.Date == DateTime.Today.Date : x.Created >= model.DateTime1 && x.Created <= model.DateTime2) : x.Created >= DateTime.Today.AddDays(-model.Created) && x.Created <= DateTime.Today.AddDays(1))
					.Select(x => x.Sum)
					.Sum();

				decimal other = user.Expense
					.Where(x => x.ExpenseType == ExpenseType.Other)
					.Where(x => model.Created == 0 || model.Created == 1 ? (model.DateTime1 == DateTime.MinValue || model.DateTime2 == DateTime.MinValue ? x.Created.Date == DateTime.Today.Date : x.Created >= model.DateTime1 && x.Created <= model.DateTime2) : x.Created >= DateTime.Today.AddDays(-model.Created) && x.Created <= DateTime.Today.AddDays(1))
					.Select(x => x.Sum)
					.Sum();

				values[0, 0] = Math.Round(CalculatePercentage(food, limit), 2);
				values[1, 0] = Math.Round(CalculatePercentage(fun, limit), 2);
				values[2, 0] = Math.Round(CalculatePercentage(clothes, limit), 2);
				values[3, 0] = Math.Round(CalculatePercentage(study, limit), 2);
				values[4, 0] = Math.Round(CalculatePercentage(medicine, limit), 2);
				values[5, 0] = Math.Round(CalculatePercentage(other, limit), 2);

				values[0, 1] = Math.Round((double)food, 2);
				values[1, 1] = Math.Round((double)fun, 2);
                values[2, 1] = Math.Round((double)clothes, 2);
				values[3, 1] = Math.Round((double)study, 2);
				values[4, 1] = Math.Round((double)medicine, 2);
				values[5, 1] = Math.Round((double)other, 2);

				_logger.LogInformation("[ExpenseService.GetStatistic]: Statistic was calculated");

				return new BaseResponse<double[,]>()
				{
					Data = values,
					StatusCode = StatusCode.OK
				};
            }
			catch (Exception e)
			{
                _logger.LogInformation(e, "[ExpenseService.GetStatistic]: {Message}", e.Message);

                return new BaseResponse<double[,]>()
				{
					StatusCode = StatusCode.InternalServerError
				};
			}
		}

		// Рассчитывает сумму всех трат за весь период
		public async Task<IBaseResponse<decimal>> GetAllExpenses()
		{
			try
			{
                _logger.LogInformation("[ExpenseService.GetAllExpenses]: GetAllExpenses started - {Now}", DateTime.Now);

                var user = await _manager.GetCurrentUser();

				if (user == null)
				{
					_logger.LogInformation("[ExpenseService.GetAllExpenses]: User was null");

                    return new BaseResponse<decimal>()
                    {
                        Description = "User was null",
                        StatusCode = StatusCode.UserNotFound
                    };
                }

				var allExpenses = user.Expense.Sum(x => x.Sum);

				_logger.LogInformation("[ExpenseService.GetAllExpenses]: Value for all time was calculated");

				return new BaseResponse<decimal>()
				{
					Data = allExpenses,
					StatusCode = StatusCode.OK
				};
			}
			catch (Exception e)
			{
                _logger.LogInformation(e, "[ExpenseService.GetAllExpenses]: {Message}", e.Message);

                return new BaseResponse<decimal>()
                {
                    StatusCode = StatusCode.InternalServerError
                };
            }
		}

		// Возвращает последнюю трату
		public async Task<IBaseResponse<CreateAndFilterViewModel>> GetLastExpense()
		{
			try
			{
                _logger.LogInformation("[ExpenseService.GetLastExpense]: GetLastExpense started - {Now}", DateTime.Now);

				var user = await _manager.GetCurrentUser();

				if (user == null)
				{
					_logger.LogInformation("[ExpenseService.GetLastExpense]: User was null");

					return new BaseResponse<CreateAndFilterViewModel>()
					{
						Description = "User was null",
						StatusCode = StatusCode.UserNotFound
					};
				}

				var lastExpense = user.Expense.Select(x => new CreateAndFilterViewModel()
				{
					Create = new CreateExpenseViewModel()
					{
						Sum = x.Sum,
						ExpenseType = x.ExpenseType,
						Created = x.Created
					}
				}).OrderByDescending(x => x.Create?.Created).FirstOrDefault();

				_logger.LogInformation("[ExpenseService.GetLastExpense]: Last expense have been received");

				return new BaseResponse<CreateAndFilterViewModel>()
				{
					Data = lastExpense,
					StatusCode = StatusCode.OK
				};
            }
			catch (Exception e)
			{
                _logger.LogInformation(e, "[ExpenseService.GetLastExpense]: {Message}", e.Message);

				return new BaseResponse<CreateAndFilterViewModel>()
				{
					StatusCode = StatusCode.InternalServerError
				};
            }
		}

		// Вычисление целочисленного значения процента в виде текста (Для задания значения диаграмме)
		public string CalculatePercValue(double value) => (int)value < 10 ? ".0" + (int)value : (int)value > 100 ? "1" : "." + (int)value;

		// Вычисление процента траты от дневного лимита
        public static double CalculatePercentage(decimal number, double total)
        {
            if (total == 0)
            {
                throw new DivideByZeroException("Total value can`t be zero.");
            }

            return (double)(number / (decimal)total) * 100;
        }
    }
}