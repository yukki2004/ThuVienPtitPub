using MediatR;
using Microsoft.EntityFrameworkCore.Migrations;
using ThuVienPtit.Src.Application.Users.DTOs.DtoRespone;
using ThuVienPtit.Src.Application.Users.Interface;

namespace ThuVienPtit.Src.Application.Users.Queries
{
    public class GetUserInfoQueriesHandle : IRequestHandler<GetUserInfoQueries, UserInfoDto>
    {
        private readonly IUserRespository _userRespository;
        private readonly string baseUrl;
        public GetUserInfoQueriesHandle(IUserRespository userRespository, IConfiguration configuration)
        {
            _userRespository = userRespository;
            baseUrl = configuration["FileStorage:BaseUrl"] ?? "";
        }
        public async Task<UserInfoDto> Handle(GetUserInfoQueries request, CancellationToken cancellationToken)
        {
            var user = await _userRespository.GetUserByUserId(request.user_id);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            var userInfoDto = new UserInfoDto
            {
                user_id = user.user_id,
                name = user.name,
                email = user.email,
                username = user.username,
                role = user.role,
                major = user.major,
                img = (user.img != null) ? Path.Combine(baseUrl,user.img).Replace("\\","/") : string.Empty,
                login_type = user.login_type,
                google_id = user.google_id,
                created_at = user.created_at,
                updated_at = user.updated_at
            };
            return userInfoDto;
        }
    }
}
