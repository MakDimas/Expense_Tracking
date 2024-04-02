using System.ComponentModel.DataAnnotations;

namespace Expense_Tracking.Domain.Enum
{
    public enum ExpenseType
    {
        [Display(Name = "Food")]
        Food = 1,

        [Display(Name = "Fun")]
        Fun = 2,

        [Display(Name = "Clothes")]
        Clothes = 3,

        [Display(Name = "Study")]
        Study = 4,

        [Display(Name = "Medicine")]
        Medicine = 5,

        [Display(Name = "Other")]
        Other = 6
    }
}