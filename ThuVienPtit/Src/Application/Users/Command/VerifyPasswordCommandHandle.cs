using MediatR;
using ThuVienPtit.Src.Application.Users.Interface;

namespace ThuVienPtit.Src.Application.Users.Command
{
    public class VerifyPasswordCommandHandle : IRequestHandler<VerifyPasswordCommand, bool>
    {
        private readonly IUserRespository _userRespository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IPasswordResetTokenRespository _passwordResetTokenRespository;
        public VerifyPasswordCommandHandle(IUserRespository userRespository, IPasswordHasher passwordHasher, IPasswordResetTokenRespository passwordResetTokenRespository)
        {
            _userRespository = userRespository;
            _passwordHasher = passwordHasher;
            _passwordResetTokenRespository = passwordResetTokenRespository;
        }
        public async Task<bool> Handle(VerifyPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRespository.GetUserByEmail(request.email);
            if (user == null)
            {
                throw new Exception("không tồn tại user với gmail đấy");
            }
            var token = await _passwordResetTokenRespository.GetByTokenAsync(user.user_id, request.emailCode);
            if(token == null)
            {
                return false;
            }
            return true;
        }
    }
    
}
