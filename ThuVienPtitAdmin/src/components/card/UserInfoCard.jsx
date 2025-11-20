import React from "react";
import guest from "../../assets/guest.png";
const UserInfoCard = ({ user }) => {
  return (
    <div className="bg-white shadow-md rounded-2xl p-6 flex items-center gap-6">
      <img
        src={user.img && user.img.trim() !== "" ? user.img : guest}
        alt={user.name}
        className="w-24 h-24 rounded-full object-cover"
      />
      <div>
        <h2 className="text-2xl font-semibold text-gray-800">{user.name}</h2>
        <p className="text-gray-600">{user.email}</p>
        <p className="text-sm text-gray-500 mt-1">Username: {user.username}</p>
        <p className="text-sm text-gray-500">
          Vai trò: {user.role === 1 ? "Admin" : "User"}
        </p>
        <p className="text-sm text-gray-400 mt-1">
          Ngày tạo: {new Date(user.created_at).toLocaleDateString()}
        </p>
      </div>
    </div>
  );
};

export default UserInfoCard;
