namespace ThuVienPtit.Src.Application.Users.Interface
{
    public interface IFileStorage
    {
        Task<string> SaveUserAvtAsync(Stream? stream, string? filename, Guid userId);
        Task<string> SaveGoogleAvtAsync(string imageUrl, Guid userId);
        Task DeleteAvtUser(Guid userId);
    }
}
