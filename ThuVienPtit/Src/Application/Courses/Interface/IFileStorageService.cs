namespace ThuVienPtit.Src.Application.Courses.Interface
{
    public interface IFileStorageService
    {
        Task CreateCourseFolder(string name, string semesterName);
        Task DeleteCourseFolder(string name, string semesterName);
        Task RenameCourseFolder(string oldName, string newName, string semesterName, string newSemesterName);
    }
}
