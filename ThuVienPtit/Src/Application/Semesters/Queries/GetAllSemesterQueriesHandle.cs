using MediatR;
using System.Collections.Generic;
using ThuVienPtit.Src.Application.Interface;

namespace ThuVienPtit.Src.Application.Semesters.Queries
{
    public class GetAllSemesterQueriesHandle : IRequestHandler<GetAllSemesterQueries, List<DTOs.Respone.SemesterResponeDto>>
    {
        private readonly Interface.ISemesterRespository _semesterRespository;
        private readonly ICacheService cacheService;

        public GetAllSemesterQueriesHandle(Interface.ISemesterRespository semesterRespository, ICacheService cacheService)
        {
            _semesterRespository = semesterRespository;
            this.cacheService = cacheService;
        }
        public async Task<List<DTOs.Respone.SemesterResponeDto>> Handle(GetAllSemesterQueries request, CancellationToken cancellationToken)
        {
            var cacheKey = "thuvienptit:semester:all";
            var cacheData = await cacheService.GetAsync<List<DTOs.Respone.SemesterResponeDto>>(cacheKey);
            var semesters = await _semesterRespository.GetAllSemesterAsync();
            var semesterDtos = semesters.Select(s => new DTOs.Respone.SemesterResponeDto
            {
                semester_id = s.semester_id,
                name = s.name,
                year = s.year
            }).ToList();
            await cacheService.SetAsync(cacheKey, semesterDtos, TimeSpan.FromDays(1));
            return semesterDtos;
        }
    }
}
