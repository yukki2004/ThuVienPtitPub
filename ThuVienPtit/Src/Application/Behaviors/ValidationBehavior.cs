using FluentValidation;
using MediatR;
namespace ThuVienPtit.Src.Application.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            // 1. Kiểm tra có validator nào đăng ký cho TRequest không
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                // 2. Chạy tất cả validator và gom lỗi
                var failures = _validators
                    .Select(v => v.Validate(context))
                    .SelectMany(r => r.Errors)
                    .Where(f => f != null)
                    .ToList();

                // 3. Nếu có lỗi, ném ValidationException
                if (failures.Count > 0)
                {
                    throw new FluentValidation.ValidationException(failures);
                }
            }

            // 4. Nếu hợp lệ, gọi handler tiếp theo
            return await next();
        }
    }
}
