using MediatR;
using System.Collections.Generic;
using ThuVienPtit.Src.Application.Courses.Interface;
using ThuVienPtit.Src.Application.Interface;

namespace ThuVienPtit.Src.Application.Courses.Queries
{
    public class GetAllCategoryDocumentQueriesHandle : IRequestHandler<GetAllCategoryDocumentQueries, List<ThuVienPtit.Src.Application.Courses.DTOs.Respone.CategoryListRespone>>
    {
        private readonly ICourseRespository courseRespository;
        private readonly ICacheService cacheService;
        public GetAllCategoryDocumentQueriesHandle(ICourseRespository courseRespository, ICacheService cacheService)
        {
            this.courseRespository = courseRespository;
            this.cacheService = cacheService;
        }
        public async Task<List<ThuVienPtit.Src.Application.Courses.DTOs.Respone.CategoryListRespone>> Handle(GetAllCategoryDocumentQueries queries, CancellationToken cancellationToken)
        {
            var cacheKey = "thuvienptit:category-course:all";
            var cacheData = await cacheService.GetAsync<List<ThuVienPtit.Src.Application.Courses.DTOs.Respone.CategoryListRespone>>(cacheKey);
            if(cacheData != null)
            {
                Console.WriteLine("trúng cache");
                return cacheData;
            }
            var category = await courseRespository.GetAllCategoryDocumentAsync();
            await cacheService.SetAsync(cacheKey, category, TimeSpan.FromHours(24));
            return category;
        }
    }
}
