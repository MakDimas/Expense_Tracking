using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Expense_Tracking.Service.Implementations
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(email, htmlMessage);
        }

        // Отправка сообщения на почту для подтверждения регистрации
        static Task Execute(string email, string htmlMessage)
        {
            var apiKey = Environment.GetEnvironmentVariable("SEND_GRID_KEY");

            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("valet5783@gmail.com", "ExpenseApp");
            var subject = "ExpenseApp Message";
            var to = new EmailAddress(email, "ExpenseApp");
            var plainTextContent = "For your verification";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlMessage);

            return client.SendEmailAsync(msg);
        }
    }
}