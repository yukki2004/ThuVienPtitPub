using FluentValidation;
using ThuVienPtit.Src.Application.Users.Command;

namespace ThuVienPtit.Src.Application.Users.Validators
{

        public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
        {
            public UpdateUserValidator()
            {
                RuleFor(x => x.userName)
               .NotEmpty().WithMessage("Username không được để trống")
               .MinimumLength(3).WithMessage("Username phải ít nhất 3 ký tự");

                RuleFor(x => x.major)
                    .MinimumLength(10).WithMessage("Chuyên ngành phải ít nhất 10 ký tự");
                RuleFor(x => x.name)
                    .NotEmpty().WithMessage("Tên không được để trống")
                    .MinimumLength(6).WithMessage("Tên ít nhất 10 ký tự");
            }
        }
    
}
