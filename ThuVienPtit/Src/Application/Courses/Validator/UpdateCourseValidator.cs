using FluentValidation;
using ThuVienPtit.Src.Application.Courses.Command;

namespace ThuVienPtit.Src.Application.Courses.Validator
{
    public class UpdateCourseValidator : AbstractValidator<UpdateCourseCommand>
    {
        public UpdateCourseValidator() {
            RuleFor(x => x.description)
                .MaximumLength(1000).WithMessage("Mô tả khóa học không được vượt quá 1000 ký tự");
            RuleFor(x => x.credits)
                .NotEmpty().WithMessage("Số tín chỉ không được để trống")
                .GreaterThan(0).WithMessage("Số tín chỉ phải lớn hơn 0");
            RuleFor(x => x.category_id)
                .NotEmpty().WithMessage("ID danh mục không được để trống")
                .GreaterThan(0).WithMessage("ID danh mục phải lớn hơn 0");
          
        }
    }
}
