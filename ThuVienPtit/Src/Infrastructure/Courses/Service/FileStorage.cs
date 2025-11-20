using ThuVienPtit.Src.Application.Courses.Interface;

namespace ThuVienPtit.Src.Infrastructure.Courses.Service
{
    public class FileStorage : IFileStorageService
    {
        private readonly string rootPath;
        public FileStorage(IConfiguration configuration)
        {
            rootPath = configuration["FileStorage:RootPath"] ?? throw new ArgumentNullException("FileStorage:RootPath is not configured");
        }

        public async Task CreateCourseFolder(string name, string semesterName)
        {
            var path = Path.Combine(rootPath,"Semesters", semesterName, name);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var curriculumPath = Path.Combine(path, "Giáo trình");
            var slidePath = Path.Combine(path, "Slide");
            var examPath = Path.Combine(path, "Đề thi");
            var otherPath = Path.Combine(path, "Tài liệu khác");
            if (!Directory.Exists(curriculumPath))
            {
                Directory.CreateDirectory(curriculumPath);
            }
            if (!Directory.Exists(slidePath))
            {
                Directory.CreateDirectory(slidePath);
            }
            if (!Directory.Exists(examPath))
            {
                Directory.CreateDirectory(examPath);
            }
            if (!Directory.Exists(otherPath))
            {
                Directory.CreateDirectory(otherPath);
            }
            await Task.CompletedTask;
        }
        public  Task DeleteCourseFolder(string name, string semesterName)
        {
            var coursePath = Path.Combine(rootPath, "Semesters", semesterName, name);
            if (Directory.Exists(coursePath))
            {
                Directory.Delete(coursePath, true);
            }
            return Task.CompletedTask;
        }
        public Task RenameCourseFolder(string oldName, string newName, string odlSemesterName, string newSemesterName)
        {
            string oldPath = Path.Combine(rootPath, "Semesters", odlSemesterName, oldName);
            string newPath = Path.Combine(rootPath, "Semesters", newSemesterName, newName);
            if (!Directory.Exists(oldPath))
            {
                throw new Exception($"không tìm thấy thư mục gốc, {oldPath}");
            }
            if (Directory.Exists(newPath))
            {
                throw new Exception("thư mục mới đã tồn tại");
            }
            Directory.Move(oldPath, newPath);
            return Task.CompletedTask;
        }
    }
}
