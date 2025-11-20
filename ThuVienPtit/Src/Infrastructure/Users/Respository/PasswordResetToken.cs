using Microsoft.EntityFrameworkCore;
using ThuVienPtit.Src.Application.Users.Interface;
using ThuVienPtit.Src.Infrastructure.Data;
namespace ThuVienPtit.Src.Infrastructure.Users.Respository
{
    public class PasswordResetToken : IPasswordResetTokenRespository
    {
        private readonly AppDataContext _context;
        public PasswordResetToken(AppDataContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Domain.Entities.password_reset_tokens entity)
        {
            await _context.password_reset_tokens.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Domain.Entities.password_reset_tokens>> GetUnusedTokensByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _context.password_reset_tokens
                .Where(t => t.user_id == userId && !t.used)
                .ToListAsync(cancellationToken);
        }
        public async Task<Domain.Entities.password_reset_tokens?> GetByTokenAsync(Guid userid, string token)
        {
            return await _context.password_reset_tokens
                .FirstOrDefaultAsync(t => t.user_id == userid && t.reset_code == token && !t.used && t.expires_at > DateTime.UtcNow);
        }
        public async Task Update(Domain.Entities.password_reset_tokens entity)
        {
            _context.password_reset_tokens.Update(entity);
            await _context.SaveChangesAsync();
        }
        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }


    }
}
