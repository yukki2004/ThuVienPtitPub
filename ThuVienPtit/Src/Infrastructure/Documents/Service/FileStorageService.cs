using ThuVienPtit.Src.Application.Documents.Interface;

namespace ThuVienPtit.Src.Infrastructure.Documents.Service
{
    public class FileStorageService : IFileStorageService
    {
        private readonly string _rootPath;
        public FileStorageService(IConfiguration configuration)
        {
            _rootPath = configuration["FileStorage:RootPath"] ?? throw new Exception("không tìm thây thư mục gốc");
        }
        public async Task<(string avtRelative, string docRelative)> AddAvtDocument(IFormFile fileAvt, IFormFile fileDoc, string semesterName, string courseName, string categoryName)
        {
            string folderGuid = Guid.NewGuid().ToString();
            var folderPath = Path.Combine(_rootPath, "Semesters", semesterName, courseName, categoryName, folderGuid);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var avtFileName = $"avt_{fileAvt.FileName}";
            var avtFilePath = Path.Combine(folderPath, avtFileName);
            using (var stream = new FileStream(avtFilePath, FileMode.Create))
            {
                await fileAvt.CopyToAsync(stream);
            }

            var docFileName = fileDoc.FileName;
            var docFilePath = Path.Combine(folderPath, docFileName);
            using (var stream = new FileStream(docFilePath, FileMode.Create))
            {
                await fileDoc.CopyToAsync(stream);
            }

            var relativeAvt = Path.Combine("Semesters", semesterName, courseName, categoryName, folderGuid, avtFileName).Replace("\\", "/");
            var relativeDoc = Path.Combine("Semesters", semesterName, courseName, categoryName, folderGuid, docFileName).Replace("\\", "/");

            return (relativeAvt, relativeDoc);
        }

        public Task DeleteFileDocument(string relativePath)
        {
            var Folder = Path.GetDirectoryName(relativePath).Replace("/","\\");
            var fullPath = Path.Combine(_rootPath, Folder);
            if(!Directory.Exists(fullPath))
            {
                throw new Exception("File không tồn tại");
            }
            if (Directory.Exists(fullPath))
            {
                Directory.Delete(fullPath, true);
            }
            return Task.CompletedTask;
        }
    }
}
