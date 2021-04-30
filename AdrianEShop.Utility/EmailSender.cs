using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdrianEShop.Utility
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailOptions emailOptions;

        public EmailSender(IOptions<EmailOptions> options)
        {
            emailOptions = options.Value;
        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(emailOptions.SendGridKey, subject, htmlMessage, email);
        }

        private async Task Execute(string sendGridKey, string subject, string message, string email)
        {
            var client = new SendGridClient(sendGridKey);
            var from = new EmailAddress("ady_viper_cena@yahoo.com", "Example User");          
            var to = new EmailAddress("adrianrcotuna@gmail.com", "Example User"); //uses email parameter         
            var msg = MailHelper.CreateSingleEmail(from, to, subject, message, "");
            var response = await client.SendEmailAsync(msg);
        }
    }
}
