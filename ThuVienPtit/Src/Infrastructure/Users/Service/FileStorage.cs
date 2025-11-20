using ThuVienPtit.Src.Application.Users.Interface;

namespace ThuVienPtit.Src.Infrastructure.Users.Service
{
    public class FileStorage : IFileStorage
    {
        public string _rootPath;
        public FileStorage(IConfiguration configuration)
        {
            _rootPath = configuration["FileStorage:RootPath"] ?? throw new ArgumentNullException(nameof(configuration), "FileStorage:RootPath is not configured.");
        }
        public async Task<string> SaveUserAvtAsync(Stream? stream, string? filename, Guid userId)
        {
            if(stream == null || filename == null)
            {
                return null;
            }
            var userFolder = Path.Combine(_rootPath,"User", $"User_{userId}");
            if(!Directory.Exists(userFolder))
            {
                Directory.CreateDirectory(userFolder);
            }
            foreach(var item in Directory.GetFiles(userFolder))
            {
                File.Delete(item);
            }
            var avtFileName = Path.Combine(userFolder, filename);
            using (var fileStream = new FileStream(avtFileName, FileMode.Create, FileAccess.Write))
            {
                await stream.CopyToAsync(fileStream);
            }
            var relativePath = Path.Combine("User", $"User_{userId}", filename);
            return relativePath.Replace("\\", "/");

        }
        public async Task<string> SaveGoogleAvtAsync(string imageUrl, Guid userId)
        {
            var userFolder = Path.Combine(_rootPath, "User", $"User_{userId}");
            if(!Directory.Exists(userFolder))
            {
                Directory.CreateDirectory(userFolder);
            }
            var fileName = $"{Guid.NewGuid()}.jpg";
            var filePath = Path.Combine(userFolder, fileName);
            using var client = new HttpClient();
            var bytes = await client.GetByteArrayAsync(imageUrl);
            await File.WriteAllBytesAsync(filePath, bytes);
            var relativePath = Path.Combine("User", $"User_{userId}", fileName);
            return relativePath.Replace("\\", "/");
        }
        public async Task DeleteAvtUser(Guid userId)
        {
            var userFolder = Path.Combine(_rootPath, "User", $"User_{userId}");
            if (Directory.Exists(userFolder))
            {
                Directory.Delete(userFolder, true);
            }
            await Task.CompletedTask;
        }
    }
}
