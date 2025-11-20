using ThuVienPtit.Src.Application.Semesters.Interface;

namespace ThuVienPtit.Src.Infrastructure.Semesters.Service
{
    public class FileStorageService : IFileStorageService
    {
        private readonly string rootPaht;
        public FileStorageService(IConfiguration configuration)
        {
            rootPaht = configuration["FileStorage:RootPath"] ?? throw new ArgumentNullException(nameof(configuration), "FileStorage:RootPath is not configured.");
        }
        public Task  AddSemesterFolder(string semesterName)
        {
            var semesterFolder = Path.Combine(rootPaht, "Semesters", semesterName);
            if(!Directory.Exists(semesterFolder))
            {
                Directory.CreateDirectory(semesterFolder);
            }
            return Task.CompletedTask;
        }
        public Task DeleteSemesterFolder(string semesterName)
        {
            var semesterFolder = Path.Combine(rootPaht, "Semesters", semesterName);
            if (Directory.Exists(semesterFolder))
            {
                Directory.Delete(semesterFolder, true);
            }
            return Task.CompletedTask;
        }
    }
}
