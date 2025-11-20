namespace ThuVienPtit.Src.Application.Users.DTOs.DtoRequest
{
    public class UpdateUserRequestDto
    {
        public string userName { get; set; } = null!;
        public string name { get; set; } = null!;
        public string? major { get; set; }
        public IFormFile? img {  get; set; }

    }
}
