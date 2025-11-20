using FluentValidation;
using ThuVienPtit.Src.Application.Documents.Command;

namespace ThuVienPtit.Src.Application.Documents.Validator
{
    public class CreateDocumentValidator :AbstractValidator<CreateDocumentCommand>
    {
        public CreateDocumentValidator()
        {
            RuleFor(x => x.title)
                .NotEmpty().WithMessage("Tiêu đề không được để trống")
                .MinimumLength(10).WithMessage("Tiêu đề tối thiểu phải 10 ký tự");
            RuleFor(x => x.description)
                .MaximumLength(1000).WithMessage("Mô tả không được vượt quá 1000 ký tự");
            RuleFor(x => x.category)
                 .NotEmpty().WithMessage("Danh mục không được để trống")
                 .MaximumLength(100).WithMessage("Danh mục không được vượt quá 100 ký tự")
                 .Must(BeAValidCategory)
                 .WithMessage("Danh mục phải là một trong: 'Giáo trình', 'Slide', 'Đề thi', 'Tài liệu khác'");
            RuleFor(x => x.course_id)
                .NotEmpty().WithMessage("Khóa học không được để trống");
            RuleFor(x => x.semester_id)
                .NotEmpty().WithMessage("Học kỳ không được để trống");
            RuleFor(x => x.avtDocument)
                .NotNull().WithMessage("Ảnh đại diện tài liệu không được để trống");
            RuleFor(x => x.fileDocument)
                .NotNull().WithMessage("File tài liệu không được để trống");
            RuleFor(x=> x.tag_ids)
                .NotNull().WithMessage("Danh sách tag không được để trống")
                  .Must(list => list.Any())
                  .WithMessage("Danh sách tài liệu phải có ít nhất một phần tử.");

        }
        private bool BeAValidCategory(string category)
        {
            var allowedCategories = new[] { "Giáo trình", "Slide", "Đề thi", "Tài liệu khác" };
            return allowedCategories.Contains(category);
        }
    }
}
