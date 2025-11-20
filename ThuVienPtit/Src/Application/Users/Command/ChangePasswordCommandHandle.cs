using MediatR;
using ThuVienPtit.Src.Application.Users.Interface;

namespace ThuVienPtit.Src.Application.Users.Command
{
    public class ChangePasswordCommandHandle : IRequestHandler<ChangePasswordCommand, string>
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserRespository _userRespository;
        public ChangePasswordCommandHandle(IPasswordHasher passwordHasher, IUserRespository userRespository)
        {
            _passwordHasher = passwordHasher;
            _userRespository = userRespository;
        }
        public async Task<string> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRespository.GetUserByUserId(request.userid);
            if(user == null)
            {
                throw new KeyNotFoundException("Không tìm thấy tài khoản của bạn");
            }
            var oldPasword = user.password_hash;
            if(!_passwordHasher.Verify(request.oldPassword, oldPasword))
            {
                return "Mật khẩu cũ không đúng";
            }
            if(request.newPassword != request.confirmPassword)
            {
                return "Mật khẩu phải không được khác nhau";
            }
            user.password_hash = _passwordHasher.Hash(request.newPassword);
            user.updated_at = DateTime.UtcNow;
            await _userRespository.Update(user);
            return "Sửa mật khẩu thành công";

        }
    }
}
