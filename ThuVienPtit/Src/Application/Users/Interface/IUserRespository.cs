using ThuVienPtit.Src.Application.Users.DTOs.DtoRespone;
using ThuVienPtit.Src.Domain.Entities;

namespace ThuVienPtit.Src.Application.Users.Interface
{
    public interface IUserRespository
    {
        Task RegisterUser(users user);
        Task<users?> GetUserByEmail(string email);
        Task<users?> GetUserByGoogleId(string googleid);
        Task<users?> GetUserByUserId(Guid userid);
        Task Update(users user);
        Task<users?> GetUserByEmailOrUsernameAsync(string login);
        Task<(int totalItems, int pageNumber, int pageSize, int totalPage, List<Application.Users.DTOs.Entities.PubAndPenDocUserDTO.DocumentDto>)> GetUserInfoByIdPendingAsync(Guid userId, int pageNumber, CancellationToken cancellationToken);
        Task<(int totalItems, int pageNumber, int pageSize, int totalPage, List<Application.Users.DTOs.Entities.PubAndPenDocUserDTO.DocumentDto>)> GetUserInfoByIdPublishAsync(Guid userId, int pageNumber, CancellationToken cancellationToken);
        Task DeleteUserAsync(Guid userId);
        Task<(int totalItems, int pageNumber, int pageSize, int totalPage, List<users>)> GetAllUserAsync(int pageNumber);
    }
}
