using MediatR;
using ThuVienPtit.Src.Application.Interface;
using ThuVienPtit.Src.Application.Semesters.DTOs.Respone;
using ThuVienPtit.Src.Application.Semesters.Interface;
using ThuVienPtit.Src.Domain.Entities;

namespace ThuVienPtit.Src.Application.Semesters.Command
{
    public class CreateSemesterCommandHandle : IRequestHandler<CreateSemesterCommand, SemesterResponeDto>
    {
        private readonly ISemesterRespository semesterRepository;
        private readonly IFileStorageService fileStorageService;
        private readonly ICacheService cacheService;
        public CreateSemesterCommandHandle(ISemesterRespository semesterRepository, IFileStorageService fileStorageService, ICacheService cacheService)
        {
            this.semesterRepository = semesterRepository;
            this.fileStorageService = fileStorageService;
            this.cacheService = cacheService;
        }
        public async Task<SemesterResponeDto> Handle(CreateSemesterCommand command, CancellationToken cancellationToken)
        {
            var newSemester = new semesters
            {
                semester_id = new Guid(),
                name = command.name,
                year = command.year
            };
            await semesterRepository.AddSemesterAsync(newSemester);
            await fileStorageService.AddSemesterFolder(newSemester.name);
            var respone = new SemesterResponeDto
            {
                semester_id = newSemester.semester_id,
                name = newSemester.name,
                year = newSemester.year
            };
            var cacheKey = "thuvienptit:semester:all";
            await cacheService.RemoveAsync(cacheKey);
            return respone;

        }
    }
}
