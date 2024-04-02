using System.ComponentModel.DataAnnotations;

namespace Expense_Tracking.Domain.Enum
{
    public enum CreatedFilter
    {
        [Display (Name = "None")]
        None = 0,

        [Display (Name = "Today")]
        Today = 1,

        [Display (Name = "Last 3 days")]
        Days = 2,

        [Display (Name = "Last week")]
        Week = 6,

        [Display (Name = "Last month")]
        Month = 29,

        [Display (Name = "Last year")]
        Year = 364
    }
}