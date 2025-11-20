import React from "react";
import UserCard from "../card/UserCard";

const UserList = ({ users, onDelete }) => {
  if (!users || users.length === 0)
    return <p className="text-center text-gray-400">Không có người dùng nào.</p>;

  return (
    <div className="grid gap-4">
      {users.map((user) => (
        <UserCard key={user.user_id} user={user} onDelete={onDelete} />
      ))}
    </div>
  );
};

export default UserList;
