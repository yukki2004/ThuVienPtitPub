using System.Text;
using ThuVienPtit.Src.Application.Users.Interface;

namespace ThuVienPtit.Src.Infrastructure.Users.Service
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _senderName;
        private readonly string _senderEmail;
        private readonly string _username;
        private readonly string _password;

        public EmailSenderService(IConfiguration configuration)
        {
            _smtpServer = configuration["EmailSettings:SmtpServer"] ?? throw new ArgumentNullException("SMTP server is not configured.");
            _smtpPort = int.Parse(configuration["EmailSettings:SmtpPort"] ?? "587");
            _senderName = configuration["EmailSettings:SenderName"] ?? "AniNews";
            _senderEmail = configuration["EmailSettings:SenderEmail"] ?? throw new ArgumentNullException("Sender email is not configured.");
            _username = configuration["EmailSettings:Username"] ?? throw new ArgumentNullException("SMTP username is not configured.");
            _password = configuration["EmailSettings:Password"] ?? throw new ArgumentNullException("SMTP password is not configured.");
        }

        public async Task SendEmailAsync(string toEmail, string body)
        {
            using var client = new System.Net.Mail.SmtpClient(_smtpServer, _smtpPort)
            {
                Credentials = new System.Net.NetworkCredential(_username, _password),
                EnableSsl = true
            };

            var mailMessage = new System.Net.Mail.MailMessage
            {
                From = new System.Net.Mail.MailAddress(_senderEmail, _senderName),
                Body = body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(toEmail);

            await client.SendMailAsync(mailMessage);
        }

        //sinh ra HTML thân thiện để gửi cho người dùng
        public string GenerateVerificationEmailHtml(string verificationCode)
        {
            var html = new StringBuilder();

            html.Append($@"
                <html>
                <head>
                    <style>
                        body {{
                            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                            background-color: #f5f7fa;
                            color: #333;
                            text-align: center;
                            padding: 40px;
                        }}
                        .container {{
                            background: #fff;
                            border-radius: 10px;
                            box-shadow: 0 2px 8px rgba(0,0,0,0.1);
                            max-width: 400px;
                            margin: auto;
                            padding: 30px;
                        }}
                        .code {{
                            font-size: 32px;
                            letter-spacing: 6px;
                            font-weight: bold;
                            color: #0078ff;
                            background: #eef5ff;
                            padding: 12px 0;
                            border-radius: 8px;
                            margin: 20px 0;
                        }}
                        p {{
                            font-size: 16px;
                            line-height: 1.6;
                        }}
                        .footer {{
                            margin-top: 20px;
                            font-size: 13px;
                            color: #777;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h2>Mã xác nhận đặt lại mật khẩu</h2>
                        <p>Xin chào,</p>
                        <p>Đây là mã xác nhận để bạn đặt lại mật khẩu tài khoản của mình:</p>
                        <div class='code'>{verificationCode}</div>
                        <p>Mã có hiệu lực trong 5 phút. Vui lòng không chia sẻ mã này với người khác.</p>
                        <div class='footer'>© {DateTime.Now.Year} AniNews - Bảo mật cho bạn là ưu tiên hàng đầu của chúng tôi.</div>
                    </div>
                </body>
                </html>
            ");

            return html.ToString();
        }
    }
}
