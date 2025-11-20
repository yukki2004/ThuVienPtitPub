using FluentValidation;
using ThuVienPtit.Src.Application.Users.Command;

namespace ThuVienPtit.Src.Application.Users.Validators
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserValidator()
        {
            RuleFor(x => x.Username)
           .NotEmpty().WithMessage("Username không được để trống")
           .MinimumLength(3).WithMessage("Username phải ít nhất 3 ký tự");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email không được để trống")
                .EmailAddress().WithMessage("Email không hợp lệ");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password không được để trống")
                .MinimumLength(6).WithMessage("Password ít nhất 6 ký tự");
            RuleFor(x=>x.Name)
                .NotEmpty().WithMessage("Tên không được để trống");
        }
    }
}
