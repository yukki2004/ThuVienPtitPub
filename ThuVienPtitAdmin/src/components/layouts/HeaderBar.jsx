import React from "react";
import { NavLink, useNavigate } from "react-router-dom";
import { useAuth } from "../../context/authContext";
import HeaderAdmin from "../../assets/HeaderAdmin.png";
import guest from "../../assets/guest.png"; // avatar mặc định

const Header = () => {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
   
    logout();
    navigate("/login");
  };

  return (
    <header className="flex items-center justify-between bg-gradient-to-r from-red-100 via-white to-red-100 border-b border-red-300 px-8 py-3 shadow-md">
      {/* Logo */}
      <div className="flex items-center space-x-3">
        <img
          src={HeaderAdmin}
          alt="Tài liệu Viễn thông - Khoa Viễn thông I"
          className="h-14 object-contain drop-shadow-sm"
        />
      </div>

      {/* Menu + Avatar */}
      <div className="flex items-center space-x-6">
        <NavLink
          to="/"
          className={({ isActive }) =>
            `font-semibold text-sm tracking-wide transition px-4 py-2 rounded-lg ${
              isActive
                ? "bg-red-600 text-white shadow-sm"
                : "text-red-700 hover:text-red-800 hover:bg-red-50"
            }`
          }
        >
          Trang chủ
        </NavLink>

        {/* Hiển thị avatar nếu có user */}
        {user && (
          <div className="flex items-center space-x-2">
            <img
              src={user.img || guest}
              alt="Avatar"
              className="w-8 h-8 rounded-full object-cover"
            />
            <span className="font-medium text-sm text-red-700">{user.username || "Guest"}</span>
          </div>
        )}

        <button
          onClick={handleLogout}
          className="font-semibold text-sm tracking-wide px-4 py-2 rounded-lg text-red-700 hover:text-red-800 hover:bg-red-50 transition"
        >
          Đăng xuất
        </button>
      </div>
    </header>
  );
};

export default Header;
