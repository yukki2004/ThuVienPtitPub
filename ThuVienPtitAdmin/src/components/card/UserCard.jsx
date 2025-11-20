import React from "react";
import { useNavigate } from "react-router-dom";
import guest from "../../assets/guest.png";
const UserCard = ({ user, onDelete }) => {
  const navigate = useNavigate();

  const handleDetail = () => {
    navigate(`/admin/users/${user.user_id}`);
  };

  return (
    <div className="p-4 bg-white rounded-2xl shadow hover:shadow-lg transition flex items-center justify-between">
      <div className="flex items-center gap-4">
        <img
          src={
            user.img && user.img.trim() !== ""
              ? user.img
              : guest
          }
          alt={user.name}
          className="w-16 h-16 object-cover rounded-full"
        />
        <div>
          <h3 className="text-lg font-semibold">{user.name}</h3>
          <p className="text-sm text-gray-500">{user.email}</p>
          <p className="text-sm text-gray-400">
            Vai trò: {user.role === 1 ? "Admin" : "User"}
          </p>
        </div>
      </div>

      <div className="flex gap-3">
        <button
          onClick={handleDetail}
          className="px-3 py-1 bg-blue-500 text-white rounded-lg hover:bg-blue-600 transition"
        >
          Xem chi tiết
        </button>
        <button
          onClick={() => onDelete(user.user_id)}
          className="px-3 py-1 bg-red-500 text-white rounded-lg hover:bg-red-600 transition"
        >
          Xóa
        </button>
      </div>
    </div>
  );
};

export default UserCard;
