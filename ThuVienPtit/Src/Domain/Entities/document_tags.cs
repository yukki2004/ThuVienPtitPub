namespace ThuVienPtit.Src.Domain.Entities
{
    public class document_tags
    {
        public Guid document_id { get; set; }
        public int tag_id { get; set; }

        // navigation
        public documents Document { get; set; } = null!;
        public tags Tag { get; set; } = null!;
    }
}
