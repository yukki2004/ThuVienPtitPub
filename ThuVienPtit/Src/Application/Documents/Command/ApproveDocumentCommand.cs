using MediatR;

namespace ThuVienPtit.Src.Application.Documents.Command
{
    public class ApproveDocumentCommand : IRequest<bool>
    {
        public Guid document_id { get; set; }
    }
}
