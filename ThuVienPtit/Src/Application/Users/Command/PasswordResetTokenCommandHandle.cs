using MediatR;
using ThuVienPtit.Src.Application.Users.Interface;
using ThuVienPtit.Src.Infrastructure.Users.Respository;

namespace ThuVienPtit.Src.Application.Users.Command
{
    public class PasswordResetTokenCommandHandle : IRequestHandler<PasswordResetTokenCommand,bool>
    {
        private readonly IUserRespository _userRespository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IPasswordResetTokenRespository _passwordResetTokenRespository;
        public PasswordResetTokenCommandHandle(IUserRespository userRespository, IPasswordHasher passwordHasher, IPasswordResetTokenRespository passwordResetTokenRespository)
        {
            _userRespository = userRespository;
            _passwordHasher = passwordHasher;
            _passwordResetTokenRespository = passwordResetTokenRespository;
        }
        public async Task<bool> Handle(PasswordResetTokenCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRespository.GetUserByEmail(request.email);
            if (user == null)
            {
                throw new Exception("không tồn tại user với gmail đấy");
            }
            var token = await _passwordResetTokenRespository.GetByTokenAsync(user.user_id, request.emailCode);
            if (token == null || token.used || token.expires_at < DateTime.UtcNow)
                return false;
            if(request.newPassword != request.confirmNewPassword)
                throw new Exception("Mật khẩu mới và xác nhận mật khẩu không khớp");
            user.password_hash = _passwordHasher.Hash(request.newPassword);
            await _userRespository.Update(user);
            token.used = true;
            await _passwordResetTokenRespository.Update(token);
            return true;
        }
    }
}
