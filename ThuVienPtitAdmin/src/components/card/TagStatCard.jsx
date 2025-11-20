import React from "react";

export default function TagStatCard({ data }) {
  return (
    <div className="grid grid-cols-4 gap-4 mt-4">
      <div className="bg-blue-100 text-blue-800 p-4 rounded-lg">
        <p className="text-sm font-semibold">Tổng tài liệu</p>
        <h2 className="text-2xl font-bold">{data.toTal}</h2>
      </div>
      <div className="bg-green-100 text-green-800 p-4 rounded-lg">
        <p className="text-sm font-semibold">Đã duyệt</p>
        <h2 className="text-2xl font-bold">{data.approved}</h2>
      </div>
      <div className="bg-yellow-100 text-yellow-800 p-4 rounded-lg">
        <p className="text-sm font-semibold">Chờ duyệt</p>
        <h2 className="text-2xl font-bold">{data.pending}</h2>
      </div>
      <div className="bg-red-100 text-red-800 p-4 rounded-lg">
        <p className="text-sm font-semibold">Đã xóa</p>
        <h2 className="text-2xl font-bold">{data.deleted}</h2>
      </div>
    </div>
  );
}
