namespace ThuVienPtit.Src.Application.Users.DTOs.DtoRequest
{
    public class ChangePasswordDto
    {
        public string confirmPassword { get; set; } = null!;
        public string newPassword { get; set; } = null!;
        public string oldPassword { get; set; } = null!;
    }
}
