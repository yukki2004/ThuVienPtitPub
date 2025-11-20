namespace ThuVienPtit.Src.Application.Semesters.Interface
{
    public interface IFileStorageService
    {
        Task AddSemesterFolder(string semesterName);    
        Task DeleteSemesterFolder(string semesterName);
    }
}
