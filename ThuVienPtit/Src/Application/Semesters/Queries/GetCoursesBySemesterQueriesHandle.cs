using MediatR;
using StackExchange.Redis;
using ThuVienPtit.Src.Application.Interface;
using ThuVienPtit.Src.Application.Semesters.Interface;

namespace ThuVienPtit.Src.Application.Semesters.Queries
{
    public class GetCoursesBySemesterQueriesHandle : IRequestHandler<GetCourseBySemesterQueries, Semesters.DTOs.Respone.GetFullCouresSemester.SemesterDto>
    {
        private readonly ISemesterRespository _semesterRespository;
        private readonly ICacheService _cache;
        public GetCoursesBySemesterQueriesHandle(ISemesterRespository semesterRespository, ICacheService cache)
        {
            _semesterRespository = semesterRespository;
            _cache = cache;
        }
        public async Task<Semesters.DTOs.Respone.GetFullCouresSemester.SemesterDto> Handle(GetCourseBySemesterQueries request, CancellationToken cancellationToken)
        {
            string cacheKey = $"Semester:{request.SemesterId}";
            var cacheData = await _cache.GetAsync<Semesters.DTOs.Respone.GetFullCouresSemester.SemesterDto>(cacheKey);
            if(cacheData != null)
            {
                Console.WriteLine("Lấy dữ liệu từ Redis Cache");
                return cacheData;
            }
            var semester = await _semesterRespository.GetCourseBySemesterAsync(request.SemesterId);
            if (semester == null)
            {
                throw new Exception("không tìm thấy học kỳ");
            }
            var semesterDto = new Semesters.DTOs.Respone.GetFullCouresSemester.SemesterDto
            {
                semester_id = semester.semester_id,
                name = semester.name,
                courses = semester.courses.Select(c => new Semesters.DTOs.Respone.GetFullCouresSemester.CourseDto
                {
                    course_id = c.course_id,
                    name = c.name,
                }).ToList()
            };
            await _cache.SetAsync(cacheKey, semesterDto, TimeSpan.FromHours(1));
            return semesterDto;
        }
    }
}
