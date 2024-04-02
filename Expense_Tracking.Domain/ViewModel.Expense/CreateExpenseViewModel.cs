using Expense_Tracking.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace Expense_Tracking.Domain.ViewModel.Expense
{
    public class CreateExpenseViewModel
    {
        [Range(0.01, 499_999.99, ErrorMessage = "Your sum is invalid")]
        public decimal Sum { get; set; }

        public ExpenseType ExpenseType { get; set; }

        public DateTime Created { get; set; }
    }
}