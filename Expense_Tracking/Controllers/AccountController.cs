using Expense_Tracking.Domain.ViewModel._UserAccount;
using Expense_Tracking.Domain.ViewModel.Expense;
using Expense_Tracking.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Expense_Tracking.Controllers
{
	public class AccountController : Controller
	{
		private readonly IUserService _currentUser;
		private readonly IExpenseService _exp;

		public AccountController(IExpenseService serv, IUserService user)
		{
			_currentUser = user;
			_exp = serv;
		}

		// Показ главной страницы аккаунта
		public async Task<IActionResult> UserAccount()
		{
			var lastExpense = await _exp.GetLastExpense();

			if (lastExpense.Data != null && lastExpense.StatusCode == Domain.Enum.StatusCode.OK)
			{
				return View(lastExpense.Data.Create);
			}

			return View();
		}

		// Обработка создания Expense + обновление элемента "lastExpenseBlock"
		[HttpPost]
		public async Task<IActionResult> AddExpense(CreateExpenseViewModel model)
		{
			if (ModelState.IsValid)
			{
				var response = await _exp.Create(model);

				if (response.StatusCode == Domain.Enum.StatusCode.OK)
				{
					var countUpdated = await _currentUser.CountUpdate();

					if (countUpdated.StatusCode == Domain.Enum.StatusCode.OK)
					{
						var partialModel = await _currentUser.GetUserViewModel();

						if(partialModel.Data != null && partialModel.StatusCode == Domain.Enum.StatusCode.OK)
						{
							return PartialView("_UserAccount", partialModel.Data);
						}
					}
				}
			}

			return BadRequest(model);
		}

		// Страница "/Account/Statistic"
		public async Task<IActionResult> Statistic()
		{
			var lastExpense = await _exp.GetLastExpense();

			if (lastExpense.Data != null && lastExpense.StatusCode == Domain.Enum.StatusCode.OK)
			{
				var statistic = await _exp.GetStatistic(new FilterViewModel());

				if (statistic.Data != null && statistic.StatusCode == Domain.Enum.StatusCode.OK)
				{
					lastExpense.Data.Results = statistic.Data;

                    return View(lastExpense.Data);
                }
			}

            return BadRequest();
		}

        // Обработка фильтра страницы "/Account/Statistic"
        [HttpPost]
		public async Task<IActionResult> StatisticFilter(CreateAndFilterViewModel model)
		{
            var lastExpense = await _exp.GetLastExpense();

			if(lastExpense.Data != null && lastExpense.StatusCode == Domain.Enum.StatusCode.OK)
			{
				if(model.Filter != null)
				{
					var statistic = await _exp.GetStatistic(model.Filter);

					if (statistic.Data != null && statistic.StatusCode == Domain.Enum.StatusCode.OK)
					{
						lastExpense.Data.Results = statistic.Data;

						return PartialView("_DiagramPartialView", lastExpense.Data);
					}
				}
			}

			return BadRequest();
        }
    }
}