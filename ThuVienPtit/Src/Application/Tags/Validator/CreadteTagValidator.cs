using FluentValidation;
using ThuVienPtit.Src.Application.Tags.Command;

namespace ThuVienPtit.Src.Application.Tags.Validator
{
    public class CreadteTagValidator : AbstractValidator<CreateTagCommand>
    {
        public CreadteTagValidator()
        {
            RuleFor(x => x.name)
                .NotEmpty().WithMessage("Tag name không được trống.")
                .MaximumLength(100).WithMessage("Tag name không được quá 100 ký tự");
            RuleFor(x => x.description)
                .MaximumLength(500).WithMessage("Description không được quá 500 ký tự");
        }
    }
}
