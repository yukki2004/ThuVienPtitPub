using MediatR;
using ThuVienPtit.Src.Application.Users.Interface;
using ThuVienPtit.Src.Domain.Entities;

namespace ThuVienPtit.Src.Application.Users.Command
{
    public class ForgotPasswordCommandHandle : IRequestHandler<ForgotPasswordCommand, Unit>
    {
        private readonly IUserRespository _userRespository;
        private readonly IPasswordResetTokenRespository _passwordResetTokenRespository;
        private readonly IEmailSenderService _emailSenderService;
        public ForgotPasswordCommandHandle(IUserRespository userRespository, IPasswordResetTokenRespository passwordResetTokenRespository, IEmailSenderService emailSenderService)
        {
            _userRespository = userRespository;
            _passwordResetTokenRespository = passwordResetTokenRespository;
            _emailSenderService = emailSenderService;
        }
        public async Task<Unit> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRespository.GetUserByEmail(request.Email);
            if(user == null)
            {
                throw new Exception("Không tìm thấy người dùng với email đã cung cấp");
            }
            var oldTokens = await _passwordResetTokenRespository.GetUnusedTokensByUserIdAsync(user.user_id, cancellationToken);
            foreach (var token in oldTokens)
            {
                token.used = true;
            }
            await _passwordResetTokenRespository.SaveChangesAsync(cancellationToken);
            var tokenResetPassword = new password_reset_tokens
            {
                token_id = Guid.NewGuid(),
                user_id = user.user_id,
                reset_code = new Random().Next(100000, 999999).ToString(),
                created_at = DateTime.UtcNow,
                expires_at = DateTime.UtcNow.AddMinutes(5),
                used = false
            };
            await _passwordResetTokenRespository.AddAsync(tokenResetPassword);
            await _emailSenderService.SendEmailAsync(user.email, _emailSenderService.GenerateVerificationEmailHtml(tokenResetPassword.reset_code));
            return Unit.Value;
        }

    }
}
