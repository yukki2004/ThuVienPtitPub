using FluentValidation;
using ThuVienPtit.Src.Application.Courses.Command;

namespace ThuVienPtit.Src.Application.Courses.Validator
{
    public class CreateCourseValidator : AbstractValidator<CreateCourseCommand>
    {
        public CreateCourseValidator()
        {
            RuleFor(x => x.name)
                .NotEmpty().WithMessage("Tên khóa học khoogn được để trống")
                .MinimumLength(5).WithMessage("Tên khóa học phải ít nhất 5 ký tự");
            RuleFor(x => x.description)
                .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters");
            RuleFor(x => x.credits)
                .GreaterThan(0).WithMessage("Số tín chỉ phải lớn hơn 0");
            RuleFor(x => x.semester_id)
                .NotEmpty().WithMessage("Semester ID không được để trống");
            RuleFor(x => x.category_id)
                .GreaterThan(0).WithMessage("Category ID phải lớn hơn 0");
        }
    }
}
