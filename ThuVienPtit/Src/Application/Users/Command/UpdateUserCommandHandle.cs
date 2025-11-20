using MediatR;
using ThuVienPtit.Src.Application.Users.DTOs.DtoRespone;
using ThuVienPtit.Src.Application.Users.Interface;

namespace ThuVienPtit.Src.Application.Users.Command
{
    public class UpdateUserCommandHandle : IRequestHandler<UpdateUserCommand, UpdateUserResponeDto>
    {
        private readonly IUserRespository userRespository;
        private readonly IFileStorage fileStorage;
        private readonly string baseUrl;
        public UpdateUserCommandHandle(IUserRespository userRespository, IFileStorage fileStorage, IConfiguration configuration)
        {
            this.userRespository = userRespository;
            this.fileStorage = fileStorage;
            baseUrl = configuration["FileStorage:BaseUrl"] ?? throw new ArgumentNullException(nameof(configuration), "FileStorage:RootPath is not configured.");
        }
        public async Task<UpdateUserResponeDto> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            var user = await userRespository.GetUserByUserId(command.userid);
            if (user == null)
            {
                throw new KeyNotFoundException("User không tồn tại trong database");
            }
            if (command.stream == null)
            {
                user.img = null;
            } else
            {
                var avtPath = await fileStorage.SaveUserAvtAsync(command.stream, command.imageName, user.user_id);
                user.img = avtPath;
            }                
            user.username = command.userName;
            user.name = command.name;
            user.updated_at = DateTime.UtcNow;
            user.major = command.major;
            await userRespository.Update(user);
            var respone = new UpdateUserResponeDto
            {
                userid = user.user_id,
                name = user.name,
                username = user.username,
                update_at = DateTime.UtcNow,
                create_at = DateTime.UtcNow,
                email = user.email,
                major = user.major,
                imgurl = (user.img == null) ? null : Path.Combine(baseUrl, user.img).Replace("\\", "/"),
                login_Type = user.login_type,
                role = user.role,
                google_id = user.google_id
            }; 
            return respone;
        }
    }
}
