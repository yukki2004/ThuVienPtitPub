using MediatR;
using ThuVienPtit.Src.Application.DashBoard.DTOs;
using ThuVienPtit.Src.Application.DashBoard.Interface;

namespace ThuVienPtit.Src.Application.DashBoard.Queries
{
    public class GetAdminStatisticsQueriesHandle : IRequestHandler<GetAdminStatisticsQueries, DashboardDto>
    {
        private readonly IDashBoardRespository dashBoardRespository;
        public GetAdminStatisticsQueriesHandle(IDashBoardRespository dashBoardRespository)
        {
            this.dashBoardRespository = dashBoardRespository;
        }
        public async Task<DashboardDto> Handle(GetAdminStatisticsQueries queries, CancellationToken cancellationToken)
        {
            var respone = await dashBoardRespository.GetStatDashboard();
            return respone;
        }
    }
}
