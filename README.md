# 🛡️ ThuVienPtit - Hệ thống Quản lý Tài liệu Số Repository: ```https://github.com/yukki2004/ThuVienPtitPub.git```
ThuVienPtit là nền tảng số hóa tài liệu dành cho Khoa Viễn Thông, giúp sinh viên và giảng viên dễ dàng chia sẻ, lưu trữ và tìm kiếm tài liệu học tập. Hệ thống được xây dựng trên nền tảng công nghệ hiện đại (Clean Architecture, .NET 9), đảm bảo hiệu năng cao và bảo mật chặt chẽ.
# 🌟 Chức năng & Nghiệp vụ (Key Features)
## 1. Quản lý Tài khoản & Xác thực (Identity & Auth)
- Đăng nhập đa kênh: Hỗ trợ đăng nhập Local (Email/Pass) và Google OAuth
- Cơ chế Token nâng cao:
   - Access Token: Ngắn hạn, dùng để gọi API.
   - Refresh Token: Dài hạn, lưu trữ an toàn trong Database.
- Phân quyền (RBAC): Admin (Quản trị viên) và User (Sinh viên/Giảng viên).
- Bảo mật API: Middleware bảo vệ Endpoint, chỉ cho phép user có quyền truy cập.
## 2. Các chức năng chính
- Upload tài liệu: Sinh viên đăng tải tài liệu (PDF, Ảnh...).
- Quy trình Duyệt: Tài liệu mới sẽ ở trạng thái Pending. Admin duyệt (Approve) mới được hiển thị công khai.
- Tương tác: Xem chi tiết, Tải xuống tài liệu.
- Quản lý vòng đời: Chỉnh sửa thông tin, Xóa mềm (Soft Delete) (đưa vào thùng rác), Khôi phục hoặc Xóa vĩnh viễn
- Các chức năng CORS liên quan đến khóa học, tag như thêm sửa xóa.
## 3. Nghiệp vụ Nâng cao
- Phân loại & Tìm kiếm: Hệ thống Tag, Môn học (Courses), Lọc theo học kỳ/chuyên ngành.
- Phân trang (Pagination): Tối ưu hóa hiển thị danh sách lớn.
- Thống kê (Dashboard): API báo cáo tổng số tài liệu, người dùng, tag phổ biến cho Admin.
- Quản lý User: Admin toàn quyền quản lý User có thểm xem chi tiết thông tin user xem các tài liệu đã đăng bởi user đó...
- Caching: Sử dụng Redis để cache các dữ liệu ít thay đổi (Config, Menu môn học) giúp tăng tốc độ tải trang.
# 🏛️ Kiến trúc Backend (Clean Architecture) Backend được tổ chức theo mô hình Onion Architecture (Clean Architecture), tách biệt hoàn toàn giữa nghiệp vụ và hạ tầng công nghệ.
```
ThuVienPtit/
├── 📂 Src/
│   ├── 🟡 Domain/                  # Lớp lõi: Chứa nghiệp vụ cốt lõi
│   │   ├── Entities/               # Các thực thể DB (User, Document, Subject...)
│   │   └── Enum/                   # Các định nghĩa hằng số (Role, Status...)
│   │
│   ├── 🔴 Application/             # Lớp ứng dụng: Xử lý Logic (CQRS)
│   │   ├── Behaviors/              # Pipeline (Validation, Logging, Transaction)
│   │   ├── Interface/              # Các Interface chung (IApplicationDbContext...)
│   │   │
│   │   ├── Users/                  # [Feature] Quản lý Người dùng
│   │   │   ├── Command/            # Các lệnh ghi (CreateUser, UpdateUser...)
│   │   │   ├── Queries/            # Các lệnh đọc (GetUserById, Login...)
│   │   │   ├── DTOs/               # Đối tượng chuyển đổi dữ liệu
│   │   │   └── Validators/         # Kiểm tra dữ liệu đầu vào (FluentValidation)
│   │   │
│   │   └── [Documents/Courses...]  # Các Feature khác cấu trúc tương tự Users
│   │
│   ├── 🔵 Infrastructure/          # Lớp hạ tầng: Triển khai kỹ thuật
│   │   ├── Data/                   # EF Core DbContext
│   │   ├── Respository/            # Base Repository & UnitOfWork
│   │   │
│   │   ├── Users/                  # Triển khai User Repository & Service cụ thể
│   │   └── [Documents/Courses...]  # Triển khai hạ tầng cho các Feature khác
│   │
│   └── 🟢 Presention/              # Lớp giao diện: API Endpoints
│       ├── Controller/             # Các API Controllers (UsersController...)
│       └── ExceptionMiddleware/    # Xử lý lỗi tập trung toàn hệ thống
│
├── Dockerfile                      # Cấu hình Build Docker
└── appsettings.json                # Cấu hình kết nối DB, Redis, Email...
```
# 🚀 Hướng dẫn Cài đặt & Triển khai (Installation)
- Bước 1: Clone Mã nguồn
```
git clone https://github.com/yukki2004/ThuVienPtitPub.git
cd ThuVienPtitPub
```
- Bước 2: build image Docker
```
# Vào thư mục backend chứa Dockerfile
cd ThuVienPtit
# Build Docker image với tên thuvien_backend_image
docker build -t thuvien_backend_image .
```
- Bước 3: Cấu hình Môi trường Docker
Mở file ```docker-compose.yml``` tại thư mục gốc. Đây là cấu hình chuẩn để chạy Backend kết nối với DB trên máy Host.
> ⚠️ **Lưu ý:** Host=172.17.0.1: Đây là IP để Container gọi ra máy thật (Windows/Linux Host). Đảm bảo PostgreSQL/Redis trên máy bạn đang chạy ở port tương ứng..
```
version: '3.8'
services:
  backend:
    image: thuvien_backend_image 
    container_name: thuvien_backend
    restart: always
    ports:
      - "8080:8080"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ASPNETCORE_URLS=http://+:8080
      
      # --- KẾT NỐI DATABASE (PostgreSQL) ---
      # Kết nối tới DB chạy trên máy Host qua cổng 5433
      > ⚠️ **Lưu ý:** thay Username, Password, Database đúng với tên database trong máy bạn.
      - ConnectionStrings__DefaultConnection=Host=172.17.0.1;Port=5433;Database=faculty_docs;Username=postgres;Password=yukki2004; 

      # --- CẤU HÌNH FILE STORAGE ---
      - FileStorage__RootPath=/app/files_storage
      > ⚠️ **Lưu ý:** đây là đường dẫn khi build với nginx thay bằng baseUrl của bạn.
      - FileStorage__BaseUrl=http://api.thuvienptit.com/files

      # --- CẤU HÌNH REDIS & AUTH ---
      > ⚠️ **Lưu ý:** thay Host và Post thành Redis của bạn.
      - Redis__Host=172.17.0.1
      - Redis__Port=6379
      - Jwt__Key=MyUltraSuperSecretKey_1234567890!!###
      - Jwt__Issuer=ThuVienPtitBackend
      - Jwt__Audience=ThuVienPtitUser
      
      # --- EMAIL SERVICE (SMTP Gmail) ---
      - EmailSettings__SmtpServer=smtp.gmail.com
      - EmailSettings__SmtpPort=587
      - EmailSettings__SenderName=ThuVienPTIT Support
      - EmailSettings__SenderEmail=your-email@gmail.com
      - EmailSettings__Username=your-email@gmail.com
      - EmailSettings__Password=your-app-password
    volumes:
      # Map thư mục lưu file từ máy thật vào trong Docker (đây là fileserver có vai trò chứa dữ liệu thật)
     > ⚠️ **Lưu ý:** Map đường dẫn file chuẩn với FileServer.
      - /var/www/thuvienptit/storage/ThuVienPtit:/app/files_storage


```
- Bước 3: Build & Chạy Docker Tại thư mục gốc (nơi chứa file ```docker-compose.yml```), chạy lệnh:
```
docker-compose up -d
```
Hệ thống sẽ tự động Build Backend, thiết lập môi trường và khởi chạy.
# 👨‍💻 Hướng dẫn Chạy Thủ công (Manual Dev)
1. Build Backend (.NET)
```
# Vào thư mục Backend
cd ThuVienPtit
# Khôi phục thư viện & Update Database
dotnet restore
# Chạy App
dotnet run
```
API Swagger sẽ chạy tại: ```http://localhost:8080/swagger ```

2. Build Frontend (ReactJS)# Vào thư mục Frontend
```
cd ThuVienPtitAdmin
# Cài đặt thư viện Node
npm install
# Chạy môi trường Dev
npm run dev
```
Web Admin sẽ chạy tại:``` http://localhost:5173```
# 📸Demo Giao diện (Screenshots)Dưới đây là một số hình ảnh thực tế của hệ thống Admin Dashboard.
### 1. Dashboard Tổng quanThống kê trực quan số lượng tài liệu, người dùng và trạng thái hệ thống.
   ![Dashboard Admin](https://drive.google.com/uc?export=view&id=1ZeJepNvD_5XAU4IPJ4p4h5FsCJuBT2Cu)
### 2. Quản lý Môn họcDanh sách các học phần, hỗ trợ lọc theo tín chỉ và chuyên ngành.
   ![Dashboard Admin](https://drive.google.com/uc?export=view&id=14EPFS4BAH3jK51LnjM5-d5jd9aJ7UCue)
	 ![Dashboard Admin](https://drive.google.com/uc?export=view&id=1ZGUq3r9cupwwzHOyiAAHYHRhvqUYKZpD)
### 3. Hệ thống Tag & Báo cáo Quản lý nhãn (Tags) để phân loại tài liệu nhanh chóng.
   ![Tag](https://drive.google.com/uc?export=view&id=1v6Qnhnf53Xi004c3pq4M-Ky3V4GKHw3F)
### 4. Thùng rác & Khôi phụcTính năng an toàn dữ liệu, cho phép khôi phục tài liệu lỡ tay xóa.
   ![Dashboard Admin](https://drive.google.com/uc?export=view&id=1EhmlGTfhw9H34qH6lNTuTiSlotK46Pip)
# 🛠️ Tech Stack Thành phần Công nghệ

| Thành phần       | Công nghệ / Library            | Mô tả ngắn gọn                                           
|------------------|--------------------------------|---------------------------------------------------------
| 🖥️ Backend       | .NET 9 Web API                | Framework Backend hiện đại, hiệu năng cao              
| 💾 Database      | PostgreSQL                    | Cơ sở dữ liệu quan hệ mạnh mẽ, hỗ trợ JSONB & Indexing 
| 🛠️ ORM           | Entity Framework Core         | Code First, Migrations, quản lý database dễ dàng        
| ⚡ Cache         | Redis                         | Lưu cache, tăng tốc độ truy vấn & lưu OTP              
| 🌐 Frontend      | ReactJS + Vite                | SPA hiện đại, tốc độ cao                                 
| 🎨 UI            | Tailwind CSS                  | Utility-first CSS, responsive, nhanh chóng             
| 🐳 DevOps        | Docker & Nginx                | Containerization & Reverse Proxy                       
| 📧 Email Service | SMTP Gmail                    | Gửi OTP hoặc thông báo qua Email                        
| 🔑 Auth          | JWT, OAuth2                   | Token-based authentication, phân quyền RBAC            

# © 2025 ThuVienPtit by Yukki2004.
