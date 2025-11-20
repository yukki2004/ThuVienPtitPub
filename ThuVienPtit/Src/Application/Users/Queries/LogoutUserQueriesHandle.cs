using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using ThuVienPtit.Src.Application.Users.Interface;
using ThuVienPtit.Src.Domain.Entities;
namespace ThuVienPtit.Src.Application.Users.Queries
{
    public class LogoutUserQueriesHandle : IRequestHandler<LogoutUserQueries, bool>
    {
        private readonly IRefreshToken refreshToken;
        private readonly IUserRespository userRespository;
        public LogoutUserQueriesHandle(IRefreshToken refreshToken, IUserRespository userRespository)
        {
            this.refreshToken = refreshToken;
            this.userRespository = userRespository;
        }
        public async Task<bool> Handle(LogoutUserQueries queries, CancellationToken cancellationToken)
        {
            var refresh_token = await refreshToken.GetByTokenAsync(queries.refresh_token);
            if(refresh_token == null)
            {
                return false;
            }
            refresh_token.revoked = true;
            await refreshToken.MarkAsUsedAsync(refresh_token);
            return true;

        }
    }
}
