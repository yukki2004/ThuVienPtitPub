namespace ThuVienPtit.Src.Application.Users.DTOs.DtoRespone
{
    public class ListUserDto
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int totalItems { get; set; }
        public int totalPage { get; set; }
        public List<UserInfoDto> users { get; set; } = new List<UserInfoDto>();
    }
}
