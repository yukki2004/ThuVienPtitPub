using ThuVienPtit.Src.Application.DashBoard.DTOs;

namespace ThuVienPtit.Src.Application.DashBoard.Interface
{
    public interface IDashBoardRespository
    {
        Task<DashboardDto> GetStatDashboard();
    }
}
