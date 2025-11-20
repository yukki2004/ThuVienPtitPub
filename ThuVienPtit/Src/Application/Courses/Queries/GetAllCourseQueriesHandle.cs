using MediatR;
using ThuVienPtit.Src.Application.Courses.DTOs.Entity;
using ThuVienPtit.Src.Application.Courses.Interface;
using ThuVienPtit.Src.Application.Interface;

namespace ThuVienPtit.Src.Application.Courses.Queries
{
    public class GetAllCourseQueriesHandle : IRequestHandler<GetAllCourseQueries, List<ThuVienPtit.Src.Application.Courses.DTOs.Respone.GetAllCourseRespone>>
    {
        private readonly ICourseRespository _courseRepository;
        private readonly ICacheService _cacheService;
        public GetAllCourseQueriesHandle(ICourseRespository courseRepository, ICacheService cacheService)
        {
            _courseRepository = courseRepository;
            _cacheService = cacheService;
        }
        public async Task<List<ThuVienPtit.Src.Application.Courses.DTOs.Respone.GetAllCourseRespone>> Handle(GetAllCourseQueries request, CancellationToken cancellationToken)
        {
            var cacheKey = "thuvienptit:course:all";
            var cacheData = await _cacheService.GetAsync<List<ThuVienPtit.Src.Application.Courses.DTOs.Respone.GetAllCourseRespone>>(cacheKey);
            if(cacheData !=  null)
            {
                Console.WriteLine("trúng cache");
                return cacheData;
            }
            var courses = await _courseRepository.GetAllCoursesAsync();
            var courseDtos = courses.Select(c => new ThuVienPtit.Src.Application.Courses.DTOs.Respone.GetAllCourseRespone
            {
                course_id = c.course_id,
                name = c.name,
                slug = c.slug,
            }).ToList();
            await _cacheService.SetAsync(cacheKey, courseDtos, TimeSpan.FromHours(12));
            return courseDtos;
        }

    }
}
