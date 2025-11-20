using MediatR;
using ThuVienPtit.Src.Application.Users.DTOs.DtoRespone;
using ThuVienPtit.Src.Application.Users.Interface;

namespace ThuVienPtit.Src.Application.Users.Queries
{
    public class UserLoginQueriesHandle : IRequestHandler<UserLoginQueries, LoginUserRespone>
    {
        private readonly IUserRespository userRespository;
        private readonly IJwtTokenService jwtTokenService;
        private readonly IPasswordHasher passwordHasher;
        private readonly IRefreshToken _refreshToken;
        public UserLoginQueriesHandle(IUserRespository userRespository, IJwtTokenService jwtTokenService, IPasswordHasher passwordHasher , IRefreshToken refreshToken)
        {
            this.userRespository = userRespository;
            this.jwtTokenService = jwtTokenService;
            this.passwordHasher = passwordHasher;
            _refreshToken = refreshToken;

        }
        public async Task<LoginUserRespone> Handle(UserLoginQueries request, CancellationToken cancellationToken)
        {
            var user = await userRespository.GetUserByEmailOrUsernameAsync(request.Login);
            if(user == null)
            {
                throw new Exception("User not found");
            }
            var passWord = user.password_hash;
            if (!passwordHasher.Verify(request.Password, passWord))
            {
                throw new Exception("Password is incorrect");
            }
            var accessToken  = jwtTokenService.GenerateToken(user);
            var refreshToken = jwtTokenService.GenerateRefreshToken();
            var userToken = new Domain.Entities.refresh_tokens
            {
                token_id = Guid.NewGuid(),
                user_id = user.user_id,
                refresh_token = refreshToken,
                revoked = false,
                created_at = DateTime.UtcNow,
                expires_at = DateTime.UtcNow.AddDays(30)
            };
            await _refreshToken.AddAsync(userToken);
            return new LoginUserRespone
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                userInfoDto = new UserInfoDto
                {
                    user_id = user.user_id,
                    username = user.username,
                    email = user.email,
                    name = user.name,
                    role = user.role,
                    img = user.img,
                    major = user.major,
                    login_type = user.login_type,
                    created_at = user.created_at,
                    updated_at = user.updated_at
                }
            };

        }
    }
}
