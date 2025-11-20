namespace ThuVienPtit.Src.Application.Documents.Interface
{
    public interface IFileStorageService
    {
        Task<(string avtRelative, string docRelative)> AddAvtDocument(IFormFile fileAvt, IFormFile fileDoc, string semesterName, string courseName, string categoryName);
        Task DeleteFileDocument(string relativePath);
    }
}
