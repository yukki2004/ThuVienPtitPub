using MediatR;
using ThuVienPtit.Src.Application.Semesters.DTOs.Respone;
using ThuVienPtit.Src.Application.Semesters.Interface;

namespace ThuVienPtit.Src.Application.Semesters.Command
{
    public class UpdateSemesterCommandHandle : IRequestHandler<UpdateSemesterCommand, SemesterResponeDto>
    {
        private readonly ISemesterRespository semesterRepository;
        public UpdateSemesterCommandHandle(ISemesterRespository semesterRepository)
        {
            this.semesterRepository = semesterRepository;
        }
        public async Task<SemesterResponeDto> Handle(UpdateSemesterCommand command, CancellationToken cancellationToken)
        {
            var existingSemester = await semesterRepository.GetSemesterById(command.semester_id);
            if (existingSemester == null)
            {
                throw new Exception("Semester not found");
            }
            existingSemester.name = command.name;
            existingSemester.year = command.year;
            await semesterRepository.UpdateSemesterAsync(existingSemester);
            var respone = new SemesterResponeDto
            {
                semester_id = existingSemester.semester_id,
                name = existingSemester.name,
                year = existingSemester.year
            };
            return respone;
        }
    }
}
