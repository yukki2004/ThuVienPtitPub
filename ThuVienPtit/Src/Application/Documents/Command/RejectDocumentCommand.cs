using MediatR;

namespace ThuVienPtit.Src.Application.Documents.Command
{
    public class RejectDocumentCommand : IRequest<bool>
    {
        public Guid document_id { get; set; }
    }
}
