using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;
using ThuVienPtit.Src.Application.Interface;

namespace ThuVienPtit.Src.Infrastructure.Respository
{
    public class SlugHelper : ISlugHelper
    {
        public string GenerateSlug(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;
            text = text.Replace("đ", "d").Replace("Đ", "D");
            string normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();
            foreach (char c in normalizedString)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(c);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }
            string result = stringBuilder.ToString().Normalize(NormalizationForm.FormC);
            result = Regex.Replace(result, @"[^a-zA-Z0-9]+", "-");
            result = result.Trim('-');
            result = result.ToLowerInvariant();
            return result;
        }
    }
}
