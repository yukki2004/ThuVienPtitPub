using Microsoft.EntityFrameworkCore;
using ThuVienPtit.Src.Application.Users.Interface;
using ThuVienPtit.Src.Domain.Entities;
using ThuVienPtit.Src.Infrastructure.Data;

namespace ThuVienPtit.Src.Infrastructure.Users.Respository
{
    public class RefreshToken : IRefreshToken
    {
        private readonly AppDataContext _context;
        public RefreshToken(AppDataContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Domain.Entities.refresh_tokens refreshToken)
        {
            await _context.refresh_tokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
        }

        public async Task<refresh_tokens?> GetByTokenAsync(string token) =>
            await _context.refresh_tokens
              .Include(rt => rt.user)
              .FirstOrDefaultAsync(rt => rt.refresh_token == token);


        public async Task MarkAsUsedAsync(Domain.Entities.refresh_tokens refreshToken)
        {
            _context.refresh_tokens.Update(refreshToken);
            await _context.SaveChangesAsync();
        }
        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

    }
}
