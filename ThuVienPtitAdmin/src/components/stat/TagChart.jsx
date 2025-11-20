import React from "react";
import {
  PieChart,
  Pie,
  Cell,
  Tooltip,
  ResponsiveContainer,
  Legend,
} from "recharts";

export default function TagChart({ data }) {
  const chartData = [
    { name: "Đã duyệt", value: data.approved },
    { name: "Chờ duyệt", value: data.pending },
    { name: "Đã xóa", value: data.deleted },
  ];

  const COLORS = [
    "url(#approvedGradient)",
    "url(#pendingGradient)",
    "url(#deletedGradient)",
  ];

  const total = chartData.reduce((sum, item) => sum + item.value, 0);

  return (
    <div className="bg-white shadow-md rounded-2xl p-6 mt-6 transition hover:shadow-lg">
      <h3 className="text-lg font-semibold text-gray-800 mb-4">
        Tỷ lệ tài liệu theo trạng thái
      </h3>

      <div className="relative w-full h-80">
        <ResponsiveContainer>
          <PieChart>
            {/* Định nghĩa gradient cho từng màu */}
            <defs>
              <linearGradient id="approvedGradient" x1="0" y1="0" x2="1" y2="1">
                <stop offset="0%" stopColor="#34d399" />
                <stop offset="100%" stopColor="#059669" />
              </linearGradient>
              <linearGradient id="pendingGradient" x1="0" y1="0" x2="1" y2="1">
                <stop offset="0%" stopColor="#facc15" />
                <stop offset="100%" stopColor="#eab308" />
              </linearGradient>
              <linearGradient id="deletedGradient" x1="0" y1="0" x2="1" y2="1">
                <stop offset="0%" stopColor="#f87171" />
                <stop offset="100%" stopColor="#dc2626" />
              </linearGradient>
            </defs>

            <Pie
              data={chartData}
              cx="50%"
              cy="50%"
              innerRadius={70}
              outerRadius={120}
              paddingAngle={4}
              cornerRadius={5}
              dataKey="value"
              labelLine={false}
              label={({ name, percent }) =>
                `${name} ${(percent * 100).toFixed(0)}%`
              }
            >
              {chartData.map((entry, index) => (
                <Cell key={index} fill={COLORS[index % COLORS.length]} />
              ))}
            </Pie>

            <Tooltip
              formatter={(value) => [`${value} tài liệu`, "Số lượng"]}
              contentStyle={{
                backgroundColor: "#fff",
                borderRadius: "8px",
                border: "1px solid #e5e7eb",
              }}
            />
            <Legend verticalAlign="bottom" height={36} />
          </PieChart>
        </ResponsiveContainer>

        {/* Hiển thị tổng số ở giữa biểu đồ */}
        <div className="absolute inset-0 flex flex-col items-center justify-center pointer-events-none">
          <span className="text-3xl font-bold text-gray-800">{total}</span>
          <span className="text-sm text-gray-500">Tổng tài liệu</span>
        </div>
      </div>
    </div>
  );
}
