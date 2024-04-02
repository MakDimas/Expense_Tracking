#nullable disable

using Expense_Tracking.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Expense_Tracking.Domain.ViewModel.User;

namespace Expense_Tracking.Areas.Identity.Pages.Account.Manage
{
    public class PersonalDataModel : PageModel
    {
        private readonly IUserService _user;

        public PersonalDataModel(IUserService user)
        {
            _user = user;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Required]
            [MinLength (2, ErrorMessage = "Your name must be longer")]
            [MaxLength (30, ErrorMessage = "Your name must be smaller")]
            public string Name { get; set; }

            [Required]
            [MinLength (2, ErrorMessage = "Your surname must be longer")]
            [MaxLength (30, ErrorMessage = "Your surname must be smaller")]
            public string Surname { get; set; }

            [Required]
            [Range (0, 499_999.99, ErrorMessage = "Your value is invalid")]
            public double DayLimit { get; set; }
        }

        public async Task<IActionResult> OnGet()
        {
            var user = await _user.GetCurrentUser();
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_user.GetCurrentUser().Result?.Id}'.");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _user.UpdateUser(new UpdateUserViewModel()
            {
                Name = Input.Name,
                Surname = Input.Surname,
                DayLimit = Input.DayLimit,
            });

            if (result.StatusCode == Domain.Enum.StatusCode.OK)
            {
                StatusMessage = "Your profile has been updated";
                return RedirectToPage();
            }
            else
            {
                StatusMessage = "Something was wrong";
                return Page();
            }
        }
    }
}