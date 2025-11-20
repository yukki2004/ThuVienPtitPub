import React, { useEffect, useState, useCallback } from "react";
import { PieChart, Pie, Cell, BarChart, Bar, XAxis, YAxis, Tooltip, ResponsiveContainer, Legend, LineChart, Line } from "recharts";
import { dashboardApi } from "../services/dashboardService";

const COLORS = ["#10B981", "#FACC15", "#EF4444"]; // Approved, Pending, Deleted

const DashboardPage = () => {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(true);

  const fetchDashboard = useCallback(async () => {
    try {
      setLoading(true);
      const res = await dashboardApi();
      setData(res);
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchDashboard();
    const interval = setInterval(fetchDashboard, 60000); // auto-refresh 60s
    return () => clearInterval(interval);
  }, [fetchDashboard]);

  if (loading) return <div className="text-center py-20">Loading...</div>;
  if (!data) return <div className="text-center py-20">No data</div>;

  const pieData = [
    { name: "Approved", value: data.documentStatus.approved },
    { name: "Pending", value: data.documentStatus.pending },
    { name: "Deleted", value: data.documentStatus.deleted }
  ];

  const cardValues = [
    { label: "📄 Tổng tài liệu", value: data.documentTotal, bg: "from-green-400 to-green-600", delta: "+12%" },
    { label: "🧾 Khóa học", value: data.courseTotal, bg: "from-blue-400 to-blue-600", delta: "+8%" },
    { label: "👥 Người dùng", value: data.userTotal, bg: "from-purple-400 to-purple-600", delta: "+18%" },
    { label: "🏷️ Tag", value: data.tagTotal, bg: "from-yellow-400 to-yellow-600", delta: "+5%" },
  ];

  return (
    <div className="p-6 font-sans">
      {/* Header */}
      <div className="flex justify-between items-center mb-6">
        <h2 className="text-2xl font-bold text-gray-700">Admin Dashboard</h2>
        <button
          onClick={fetchDashboard}
          className="px-4 py-2 bg-blue-500 hover:bg-blue-600 text-white rounded-lg font-semibold shadow-md transition"
        >
          Refresh 🔄
        </button>
      </div>

      {/* Cards */}
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6 mb-8">
        {cardValues.map((card, index) => (
          <div
            key={index}
            className={`p-6 rounded-xl bg-gradient-to-r ${card.bg} shadow-lg text-white flex flex-col justify-between hover:scale-105 transform transition`}
          >
            <div className="text-lg font-medium">{card.label}</div>
            <div className="text-3xl font-bold mt-2">{card.value}</div>
            <div className="text-sm mt-1 flex items-center space-x-1">
              <span className="font-semibold">{card.delta}</span>
              <span className="text-xs">so tháng trước</span>
            </div>
          </div>
        ))}
      </div>

      {/* Charts */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        <ChartCard title="✅ Trạng thái tài liệu">
          <ResponsiveContainer width="100%" height={280}>
            <PieChart>
              <Pie data={pieData} dataKey="value" nameKey="name" outerRadius={100} label>
                {pieData.map((entry, index) => (
                  <Cell key={index} fill={COLORS[index % COLORS.length]} />
                ))}
              </Pie>
              <Legend verticalAlign="bottom" height={36} />
              <Tooltip />
            </PieChart>
          </ResponsiveContainer>
        </ChartCard>

        <ChartCard title="📊 Tài liệu theo tháng">
          <ResponsiveContainer width="100%" height={280}>
            <BarChart data={data.documentMonthly}>
              <XAxis dataKey="month" />
              <YAxis />
              <Tooltip />
              <Bar dataKey="count" fill="#3B82F6" radius={[5,5,0,0]} />
            </BarChart>
          </ResponsiveContainer>
        </ChartCard>

        <ChartCard title="📊 Số tài liệu theo Tag">
          <ResponsiveContainer width="100%" height={280}>
            <BarChart layout="vertical" data={data.documentByTag}>
              <XAxis type="number" />
              <YAxis type="category" dataKey="tag" width={120} />
              <Tooltip />
              <Bar dataKey="count" fill="#F59E0B" radius={[5,5,5,5]} />
            </BarChart>
          </ResponsiveContainer>
        </ChartCard>

        <ChartCard title="👥 Người dùng đăng ký theo tháng">
          <ResponsiveContainer width="100%" height={280}>
            <LineChart data={data.userMonthly}>
              <XAxis dataKey="month" />
              <YAxis />
              <Tooltip />
              <Line type="monotone" dataKey="count" stroke="#EF4444" strokeWidth={3} dot={{ r: 5 }} />
            </LineChart>
          </ResponsiveContainer>
        </ChartCard>
      </div>
    </div>
  );
};

const ChartCard = ({ title, children }) => (
  <div className="bg-white rounded-xl p-6 shadow-lg flex flex-col hover:shadow-2xl transition">
    <h3 className="text-lg font-bold mb-4 text-gray-700">{title}</h3>
    {children}
  </div>
);

export default DashboardPage;
