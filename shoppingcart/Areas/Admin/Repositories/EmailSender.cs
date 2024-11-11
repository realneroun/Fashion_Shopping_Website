using System.Net.Mail;
using System.Net;

namespace shoppingcart.Areas.Admin.Repositories
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true, //bật bảo mật
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("haudtps34871@fpt.edu.vn", "mdlgdkphruxlzoef")
            };

            return client.SendMailAsync(
                new MailMessage(from: "haudtps34871@fpt.edu.vn",
                                to: email,
                                subject,
                                message
                                ));
        }
    }

}
