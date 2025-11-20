using FluentValidation;
using ThuVienPtit.Src.Application.Semesters.Command;
using ThuVienPtit.Src.Application.Users.Queries;

namespace ThuVienPtit.Src.Application.Semesters.Validators
{
    public class CreadteSemesterValidator : AbstractValidator<CreateSemesterCommand>
    {
        public CreadteSemesterValidator()
        {
            RuleFor(x => x.name)
                .NotEmpty().WithMessage("Tên học kỳ không được để trống")
                .MinimumLength(3).WithMessage("Tên học kỳ phải ít nhất 3 ký tự");
            RuleFor(x => x.year)
                .NotEmpty().WithMessage("Năm học không được để trống")
                .GreaterThan(0).WithMessage("Năm học phải lớn hơn 0");
        }
    }
}
