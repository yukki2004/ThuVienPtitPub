using Microsoft.EntityFrameworkCore;
using ThuVienPtit.Src.Application.DashBoard.DTOs;
using ThuVienPtit.Src.Application.DashBoard.Interface;
using ThuVienPtit.Src.Domain.Enum;
using ThuVienPtit.Src.Infrastructure.Data;

namespace ThuVienPtit.Src.Infrastructure.DashBoard.Repository
{
    public class DashBoardRepository : IDashBoardRespository
    {
        private readonly AppDataContext _context;

        public DashBoardRepository(AppDataContext context)
        {
            _context = context;
        }

        public async Task<DashboardDto> GetStatDashboard()
        {
            var dto = new DashboardDto();

            // ✅ Chạy tuần tự, tránh concurrent trên cùng DbContext
            dto.DocumentTotal = await _context.documents.CountAsync();
            dto.CourseTotal = await _context.courses.CountAsync();
            dto.UserTotal = await _context.users.CountAsync();
            dto.TagTotal = await _context.tags.CountAsync();

            // Document status
            var documentStatusCounts = await _context.documents
                .GroupBy(d => d.status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync();

            dto.DocumentStatus = new DocumentStatusDto
            {
                Approved = documentStatusCounts.FirstOrDefault(x => x.Status == status_document.approved)?.Count ?? 0,
                Pending = documentStatusCounts.FirstOrDefault(x => x.Status == status_document.pending)?.Count ?? 0,
                Deleted = documentStatusCounts.FirstOrDefault(x => x.Status == status_document.deleted)?.Count ?? 0
            };

            // Document monthly
            var docMonthlyRaw = await _context.documents
                .AsNoTracking()
                .GroupBy(d => d.created_at.Month)
                .Select(g => new { Month = g.Key, Count = g.Count() })
                .OrderBy(x => x.Month)
                .ToListAsync();

            dto.DocumentMonthly = docMonthlyRaw
                .Select(x => new MonthlyCountDto
                {
                    Month = x.Month.ToString("D2"),
                    Count = x.Count
                })
                .ToList();

            // Top 10 document by tag
            dto.DocumentByTag = await _context.document_tags
                .Include(dt => dt.Tag)
                .GroupBy(dt => dt.Tag.name)
                .Select(g => new TagDocumentCountDto
                {
                    Tag = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .Take(10)
                .ToListAsync();

            // User monthly
            var userMonthlyRaw = await _context.users
                .AsNoTracking()
                .GroupBy(u => u.created_at.Month)
                .Select(g => new { Month = g.Key, Count = g.Count() })
                .OrderBy(x => x.Month)
                .ToListAsync();

            dto.UserMonthly = userMonthlyRaw
                .Select(x => new MonthlyCountDto
                {
                    Month = x.Month.ToString("D2"),
                    Count = x.Count
                })
                .ToList();

            return dto;
        }
    }
}
