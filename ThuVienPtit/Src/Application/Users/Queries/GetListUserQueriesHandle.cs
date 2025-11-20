using MediatR;
using ThuVienPtit.Src.Application.Users.DTOs.DtoRespone;
using ThuVienPtit.Src.Application.Users.Interface;

namespace ThuVienPtit.Src.Application.Users.Queries
{
    public class GetListUserQueriesHandle : IRequestHandler<GetListUserQueries, ListUserDto>
    {
        private readonly IUserRespository userRespository;
        private readonly string baseUrl;

        public GetListUserQueriesHandle(IUserRespository userRespository, IConfiguration configuration)
        {
            this.userRespository = userRespository;
            baseUrl = configuration["FileStorage:BaseUrl"] ?? "";
        }

        public async Task<ListUserDto> Handle(GetListUserQueries queries, CancellationToken cancellationToken)
        {
            var userList = await userRespository.GetAllUserAsync(queries.pageNumber);
            var respone = new List<UserInfoDto>();
            foreach (var user in userList.Item5)
            {
                respone.Add(new UserInfoDto
                {
                    user_id = user.user_id,
                    username = user.username,
                    name = user.name,
                    img = ((user.img != null) && (user.img != "")) ? $"{baseUrl}/{user.img}" : string.Empty,
                    email = user.email,
                    created_at = user.created_at,
                    updated_at = user.updated_at,
                    login_type = user.login_type,
                    major = user.major,
                    role = user.role,
                    google_id = user.google_id,
                });
            }
            var res = new ListUserDto
            {
                pageNumber = queries.pageNumber,
                pageSize = userList.pageSize,
                totalItems = userList.totalItems,
                totalPage = userList.totalPage,
                users = respone,
            };
            return res;
        }
    }
}
