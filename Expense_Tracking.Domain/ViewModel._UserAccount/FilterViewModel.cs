namespace Expense_Tracking.Domain.ViewModel._UserAccount
{
    public class FilterViewModel
    {
        public int Created { get; set; }

        public DateTime DateTime1 { get; set; }

        public DateTime DateTime2 { get; set; }

        public bool[] Check { get; set; } = new bool[6];
    }
}