namespace ThuVienPtit.Src.Domain.Entities
{
    public class semesters
    {
        public Guid semester_id { get; set; }
        public string name { get; set; } = null!;
        public int year { get; set; }

        //navigation
        public ICollection<courses> courses { get; set; } = new List<courses>();
    }
}
