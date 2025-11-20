using ThuVienPtit.Src.Domain.Entities;
using ThuVienPtit.Src.Infrastructure.Users.Respository;

namespace ThuVienPtit.Src.Application.Users.Interface
{
    public interface IPasswordResetTokenRespository
    {
        Task AddAsync(Domain.Entities.password_reset_tokens entity);
        Task<List<Domain.Entities.password_reset_tokens>> GetUnusedTokensByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task SaveChangesAsync(CancellationToken cancellationToken);
        Task<password_reset_tokens?> GetByTokenAsync(Guid userid, string token);
        Task Update(password_reset_tokens entity);
    }

}
