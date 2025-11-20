using Google.Apis.Auth;
using MediatR;
using Microsoft.Extensions.Configuration;
using ThuVienPtit.Src.Application.Users.DTOs.DtoRespone;
using ThuVienPtit.Src.Application.Users.Interface;
using ThuVienPtit.Src.Domain.Entities;

namespace ThuVienPtit.Src.Application.Users.Queries
{
    public class GoogleLoginQueriesHandle : IRequestHandler<GoogleLoginQueries, GoogleLoginResponeDto> 
    {
        private readonly IUserRespository _userRespository;
        private readonly IFileStorage fileStorage;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IRefreshToken refreshToken;
        private readonly string _config;
        public GoogleLoginQueriesHandle(IUserRespository userRespository, IFileStorage fileStorage, IWebHostEnvironment webHostEnvironment, IJwtTokenService jwtTokenService, IRefreshToken refreshToken, IConfiguration config)
        {
            _userRespository = userRespository;
            this.fileStorage = fileStorage;
            this.webHostEnvironment = webHostEnvironment;
            _jwtTokenService = jwtTokenService;
            this.refreshToken = refreshToken;
            _config = config["FileStorage:BaseUrl"] ?? throw new ArgumentNullException(nameof(config), "FileStorage:RootPath is not configured.");


        }
        public async Task<GoogleLoginResponeDto> Handle(GoogleLoginQueries queries, CancellationToken cancellationToken)
        {
            var  payload = await GoogleJsonWebSignature.ValidateAsync(queries.id_token);
            var userEmail = await _userRespository.GetUserByEmail(payload.Email);
            var user = await _userRespository.GetUserByGoogleId(payload.Subject);
            if (userEmail == null && user == null)
            {
                user = new Domain.Entities.users
                {
                    user_id = Guid.NewGuid(),
                    email = payload.Email,
                    username = payload.Email.Split('@')[0],
                    name = payload.Name,
                    google_id = payload.Subject,
                    created_at = DateTime.UtcNow,
                    updated_at = DateTime.UtcNow,
                    role = Domain.Enum.Role.student,
                    login_type = Domain.Enum.login_type.google,
                };
                await _userRespository.RegisterUser(user);
            } else if (user == null && userEmail != null)
            {
                throw new Exception("Email đã được đăng ký bằng phương thức khác");
            } else
            if(user == null)
            {
              throw new Exception("Lỗi khi đăng nhập với google");
            }
            if (!string.IsNullOrEmpty(payload.Picture) && string.IsNullOrEmpty(user.img))
            {
                var avtPath = await fileStorage.SaveGoogleAvtAsync(payload.Picture, user.user_id);
                user.img = avtPath;
                await _userRespository.Update(user);
            }
            var accessToken = _jwtTokenService.GenerateToken(user);
            var refrehToken = _jwtTokenService.GenerateRefreshToken();
            var token = new refresh_tokens
            {
                token_id = Guid.NewGuid(),
                user_id = user.user_id,
                refresh_token = refrehToken,
                created_at = DateTime.UtcNow,
                expires_at = DateTime.UtcNow.AddDays(7),
                revoked = false
            };
            await refreshToken.AddAsync(token);
            var UserDto = new UserInfoDto
            {
                user_id = user.user_id,
                email = user.email,
                username = user.username,
                name = user.name,
                major = user.major,
                img = (user.img == null) ? string.Empty : Path.Combine(_config, user.img).Replace("\\","/"),
                role = user.role,
                created_at = user.created_at,
                updated_at = user.updated_at,
                login_type = user.login_type
            };
            return new GoogleLoginResponeDto
            {
                access_token = accessToken,
                refresh_token = refrehToken,
                UserInfoDto = UserDto
            };

        }
    }
}
