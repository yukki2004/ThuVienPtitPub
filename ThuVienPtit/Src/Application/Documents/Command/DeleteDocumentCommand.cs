using MediatR;

namespace ThuVienPtit.Src.Application.Documents.Command
{
    public class DeleteDocumentCommand : IRequest<bool>
    {
        public Guid document_id { get; set; }
        public Guid user_id { get; set; }
        public string user_role { get; set; } = null!;
    }
}
