using Expense_Tracking.Domain.Enum;

namespace Expense_Tracking.Domain.Entity
{
    public class Expense
    {
        public int Id { get; set; }

        public decimal Sum { get; set; }

        public ExpenseType ExpenseType { get; set; }

        public DateTime Created { get; set; }

        public string? UserId { get; set; }

        public User? User { get; set; }
    }
}