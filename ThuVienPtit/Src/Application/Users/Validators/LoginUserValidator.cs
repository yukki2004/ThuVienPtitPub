using FluentValidation;
using ThuVienPtit.Src.Application.Users.Queries;

namespace ThuVienPtit.Src.Application.Users.Validators
{
    public class LoginUserValidator : AbstractValidator<UserLoginQueries>
    {
        public LoginUserValidator()
        {
            RuleFor(x => x.Login)
           .NotEmpty().WithMessage("Thông tin đăng nhập không được để trống")
           .MinimumLength(3).WithMessage("thông tin đăng nhập phải ít nhất 3 ký tự");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password không được để trống")
                .MinimumLength(6).WithMessage("Password ít nhất 6 ký tự");
        }

    }
}
