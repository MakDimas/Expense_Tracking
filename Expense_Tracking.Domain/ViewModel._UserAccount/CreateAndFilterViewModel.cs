using Expense_Tracking.Domain.ViewModel.Expense;

namespace Expense_Tracking.Domain.ViewModel._UserAccount
{
    public class CreateAndFilterViewModel
    {
        public CreateExpenseViewModel? Create { get; set; }

        public FilterViewModel? Filter { get; set; }

        public double[,] Results { get; set; } = new double[6, 2];
    }
}