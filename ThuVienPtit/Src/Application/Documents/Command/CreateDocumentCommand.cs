using MediatR;
using ThuVienPtit.Src.Application.Documents.DTOs.Respone;

namespace ThuVienPtit.Src.Application.Documents.Command
{
    public class CreateDocumentCommand : IRequest<DocumentResponeDto>
    {
        public string title { get; set; } = null!;
        public string? description { get; set; }
        public IFormFile avtDocument { get; set; } = null!;
        public IFormFile fileDocument { get; set; } = null!;
        public Guid course_id { get; set; }
        public Guid semester_id { get; set; }
        public string category { get; set; } = null!;
        public Guid user_id { get; set; } 
        public string userRole { get; set; } = null!;
        public List<int> tag_ids { get; set; } = new List<int>();

    }
}
