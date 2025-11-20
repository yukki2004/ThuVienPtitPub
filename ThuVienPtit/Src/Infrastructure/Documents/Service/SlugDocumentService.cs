using System.Text.RegularExpressions;
using System.Text;
using ThuVienPtit.Src.Application.Documents.Interface;

namespace ThuVienPtit.Src.Infrastructure.Documents.Service
{
    public class SlugDocumentService : ISlugDocumentService
    {
        public string GenerateSlug(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return Guid.NewGuid().ToString();
            input = input.Trim().ToLowerInvariant();

            input = input.Replace("đ", "d").Replace("Đ", "D");

            string normalized = input.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (var c in normalized)
            {
                var unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }
            input = sb.ToString().Normalize(NormalizationForm.FormC);
            input = Regex.Replace(input, @"[^a-z0-9\s-]", " ");
            input = Regex.Replace(input, @"\s+", "-").Trim('-');
            string slug = $"{input}-{Guid.NewGuid()}";

            return slug;
        }
    }
}
