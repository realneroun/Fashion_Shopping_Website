namespace shoppingcart.Areas.Admin.Repositories
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message); //hàm gửi email
    }
}
