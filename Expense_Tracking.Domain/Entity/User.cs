using Microsoft.AspNetCore.Identity;

namespace Expense_Tracking.Domain.Entity
{
    public class User : IdentityUser
    {
        public string? Name { get; set; }

        public string? Surname { get; set; }

        public double DayLimit { get; set; }

        public decimal Count { get; set; }

        public ICollection<Expense> Expense { get; set; } = new List<Expense>();
    }
}