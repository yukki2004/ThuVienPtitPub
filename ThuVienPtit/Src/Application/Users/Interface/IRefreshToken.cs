namespace ThuVienPtit.Src.Application.Users.Interface
{
    public interface IRefreshToken
    {
        Task AddAsync(Domain.Entities.refresh_tokens token);
        Task<Domain.Entities.refresh_tokens?> GetByTokenAsync(string token);
        Task MarkAsUsedAsync(Domain.Entities.refresh_tokens token);
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
