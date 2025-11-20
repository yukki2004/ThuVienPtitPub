namespace ThuVienPtit.Src.Application.Helpers
{
    public static class CacheVersionHelper
    {
        public static readonly Dictionary<string, string> Map = new(StringComparer.OrdinalIgnoreCase)
        {
            { "Giáo trình", "giao-trinh" },
            { "Slide", "slide" },
            { "Đề thi", "de-thi" },
            { "Tài liệu khác", "tai-lieu-khac" }
        };
    }
}
