using MediatR;
using ThuVienPtit.Src.Application.Interface;
namespace ThuVienPtit.Src.Application.Semesters.Queries
{
    public class GetAllSemesterCourseCategoryQueriesHandle 
        : IRequestHandler<GetAllSemesterCourseCategoryQueries, List<Application.Semesters.DTOs.Respone.GetFullCouresSemester.semester_course_category>>
    {
        private readonly Interface.ISemesterRespository _semesterRespository;
        private readonly ICacheService _cacheService;

        public GetAllSemesterCourseCategoryQueriesHandle(
            Interface.ISemesterRespository semesterRespository,
            ICacheService cacheService)
        {
            _semesterRespository = semesterRespository;
            _cacheService = cacheService;
        }

        public async Task<List<Application.Semesters.DTOs.Respone.GetFullCouresSemester.semester_course_category>> Handle(
            GetAllSemesterCourseCategoryQueries request,
            CancellationToken cancellationToken)
        {
            string cacheKey = "semester_course_category_all";
            var cachedData = await _cacheService.GetAsync<List<Application.Semesters.DTOs.Respone.GetFullCouresSemester.semester_course_category>>(cacheKey);
            if (cachedData != null)
            {
                Console.WriteLine("Lấy dữ liệu từ Redis Cache");
                return cachedData;
            }

            var semesterCourseCategories = await _semesterRespository.GetSemester_course_category();

            await _cacheService.SetAsync(cacheKey, semesterCourseCategories, TimeSpan.FromMinutes(10));

            Console.WriteLine("Lấy dữ liệu từ DB và lưu Redis");
            return semesterCourseCategories;
        }
    }
}
