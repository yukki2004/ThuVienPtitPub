using MediatR;
using ThuVienPtit.Src.Application.Users.DTOs.DtoRespone;
using ThuVienPtit.Src.Application.Users.Interface;
using ThuVienPtit.Src.Domain.Entities;

namespace ThuVienPtit.Src.Application.Users.Command
{
    public class RefreshTokenCommandHandle : IRequestHandler<RefreshTokenCommand, RefreshTokenDto>
    {
        private readonly IJwtTokenService jwtTokenService;
        private readonly IRefreshToken refreshTokenRepository;
        private readonly IUserRespository userRespository;
        public RefreshTokenCommandHandle(
            IJwtTokenService jwtTokenService,
            IRefreshToken refreshTokenRepository,
            IUserRespository userRespository)
        {
            this.jwtTokenService = jwtTokenService;
            this.refreshTokenRepository = refreshTokenRepository;
            this.userRespository = userRespository;
        }
        public async Task<RefreshTokenDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var tokenEntity = await refreshTokenRepository.GetByTokenAsync(request.refresh_token);
            if (tokenEntity == null || tokenEntity.revoked || tokenEntity.expires_at < DateTime.UtcNow)
                throw new Exception("Refresh token đã hết hạn");
            var user = tokenEntity.user;
            if (user == null)
                throw new Exception("Người dùng không tồn tại");
            var newAccessToken = jwtTokenService.GenerateToken(user);
            var newRefreshToken = jwtTokenService.GenerateRefreshToken();
            tokenEntity.revoked = true;
            if (tokenEntity.expires_at.Kind == DateTimeKind.Unspecified)
    {
        tokenEntity.expires_at = DateTime.SpecifyKind(tokenEntity.expires_at, DateTimeKind.Utc);
    }
    
    if (tokenEntity.created_at.Kind == DateTimeKind.Unspecified) // Nếu bạn có trường created_at
    {
        tokenEntity.created_at = DateTime.SpecifyKind(tokenEntity.created_at, DateTimeKind.Utc);
    }

    // Nếu entity có trường RevokedAt (thời điểm thu hồi), hãy gán luôn tại đây:
    // tokenEntity.revoked_at = DateTime.UtcNow; 

    await refreshTokenRepository.MarkAsUsedAsync(tokenEntity);
            var newTokenEntity = new refresh_tokens
            {
                token_id = Guid.NewGuid(),
                user_id = user.user_id,
                refresh_token = newRefreshToken,
                expires_at = DateTime.UtcNow.AddDays(30),
                created_at = DateTime.UtcNow,
                revoked = false
            };
            await refreshTokenRepository.AddAsync(newTokenEntity);
            return new RefreshTokenDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };

        }
    }
}
