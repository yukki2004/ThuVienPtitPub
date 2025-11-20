using MediatR;
using ThuVienPtit.Src.Application.Users.Interface;

namespace ThuVienPtit.Src.Application.Users.Command
{
    public class DeleteUserCommandHandle : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly IUserRespository _userRespository;
        private readonly IFileStorage _fileStorage;
        public DeleteUserCommandHandle(IUserRespository userRespository, IFileStorage fileStorage)
        {
            _userRespository = userRespository;
            _fileStorage = fileStorage;
        }
        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRespository.GetUserByUserId(request.user_id);
            if(user == null)
            {
                throw new Exception("User not found");
            }
            await _fileStorage.DeleteAvtUser(request.user_id);
            await _userRespository.DeleteUserAsync(request.user_id);
            return true;
        }
    }
}
