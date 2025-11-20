using MediatR;
using ThuVienPtit.Src.Application.Users.DTOs.DtoRespone;
using ThuVienPtit.Src.Application.Users.Interface;

namespace ThuVienPtit.Src.Application.Users.Queries
{
    public class GetDocumentPendingUserQueriesHandle : IRequestHandler<GetDocumentPendingUserQueries, GetDocumentUser>
    {
        private readonly IUserRespository _userRespository;
        private readonly string baseUrl;
        public GetDocumentPendingUserQueriesHandle(IUserRespository userRespository, IConfiguration configuration)
        {
            _userRespository = userRespository;
            baseUrl = configuration["FileStorage:BaseUrl"] ?? "";
        }
        public async Task<GetDocumentUser> Handle(GetDocumentPendingUserQueries request, CancellationToken cancellationToken)
        {
            var documents = await _userRespository.GetUserInfoByIdPendingAsync(request.user_id, request.pageNumber, cancellationToken);
            if (documents.Item5 == null || documents.Item5.Count == 0)
            {
                return new GetDocumentUser
                {
                    TotalItems = documents.totalItems,
                    PageNumber = request.pageNumber,
                    PageSize = 0,
                    TotalPage = documents.totalPage,
                    Documents = new List<DTOs.Entities.PubAndPenDocUserDTO.DocumentDto>()
                };

            }
            var respone = documents.Item5;
            var totalItem = documents.totalItems;
            var pageNumber = documents.pageNumber;
            var totalPage = documents.totalPage;
            var pageSize = documents.pageSize;
            return new GetDocumentUser
            {
                TotalItems = totalItem,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPage = totalPage,
                Documents = respone
            };
        }
    }
}