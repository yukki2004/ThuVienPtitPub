namespace ThuVienPtit.Src.Application.Users.Interface
{
    public interface IEmailSenderService
    {
        Task SendEmailAsync(string toEmail, string body);
        string GenerateVerificationEmailHtml(string verificationCode);
    }
}
