using MediatR;
using ThuVienPtit.Src.Application.Interface;
using ThuVienPtit.Src.Application.Semesters.Interface;

namespace ThuVienPtit.Src.Application.Semesters.Command
{
    public class DeleteSemesterCommandHandle : IRequestHandler<DeleteSemesterCommand, bool>
    {
        private readonly ISemesterRespository semesterRepository;
        private readonly IFileStorageService fileStorageService;
        private readonly ICacheService cacheService;
        public DeleteSemesterCommandHandle(ISemesterRespository semesterRepository, IFileStorageService fileStorageService, ICacheService cacheService)
        {
            this.semesterRepository = semesterRepository;
            this.fileStorageService = fileStorageService;
            this.cacheService = cacheService;
        }
        public async Task<bool> Handle(DeleteSemesterCommand command, CancellationToken cancellationToken)
        {
            var existingSemester = await semesterRepository.GetSemesterById(command.semester_id);
            if (existingSemester == null)
            {
                throw new Exception("Semester not found");
            }
            await semesterRepository.DeleteSemesterAsync(existingSemester.semester_id);
            await fileStorageService.DeleteSemesterFolder(existingSemester.name);
            var cacheKey = "thuvienptit:semester:all";
            await cacheService.RemoveAsync(cacheKey);
            return true;
        }
    }
}
