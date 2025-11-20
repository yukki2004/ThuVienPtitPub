import {
  PieChart,
  Pie,
  Cell,
  BarChart,
  Bar,
  XAxis,
  YAxis,
  Tooltip,
  Legend,
  ResponsiveContainer,
  LabelList,
} from "recharts";

const PIE_COLORS = ["#4ade80", "#facc15", "#ef4444"]; 
const BAR_COLORS = ["#3b82f6", "#f59e0b", "#ef4444", "#6b7280"];

const CourseStats = ({ stats }) => {
  if (!stats) return null;

  const pieData = [
    { name: "Approved", value: stats.approved, color: PIE_COLORS[0] },
    { name: "Pending", value: stats.pending, color: PIE_COLORS[1] },
    { name: "Deleted", value: stats.deleted, color: PIE_COLORS[2] },
  ];

  const barData = Object.entries(stats.byCategory).map(([key, value], index) => ({
    name: key,
    value,
    fill: BAR_COLORS[index % BAR_COLORS.length],
  }));

  return (
    <div className="grid grid-cols-1 md:grid-cols-2 gap-6 mb-6">
      {/* Pie Chart */}
      <div className="bg-white shadow-lg rounded-xl p-6 flex flex-col items-center">
        <h3 className="text-xl font-semibold mb-4 text-gray-700">Documents by Status</h3>
        <ResponsiveContainer width="100%" height={300}>
          <PieChart>
            <Pie
              data={pieData}
              dataKey="value"
              nameKey="name"
              cx="50%"
              cy="50%"
              outerRadius={100}
              innerRadius={60}
              paddingAngle={4}
              label={({ name, percent }) => `${name}: ${(percent * 100).toFixed(0)}%`}
              isAnimationActive={true}
              animationDuration={1000}
              animationEasing="ease-out"
            >
              {pieData.map((entry, index) => (
                <Cell key={index} fill={entry.color} stroke="#fff" strokeWidth={2} />
              ))}
            </Pie>
            <Tooltip
              contentStyle={{
                backgroundColor: "#fff",
                borderRadius: 8,
                boxShadow: "0 2px 10px rgba(0,0,0,0.15)",
              }}
              formatter={(value) => [`${value} docs`, "Documents"]}
            />
            <Legend
              verticalAlign="bottom"
              height={36}
              iconType="circle"
              formatter={(value, entry) => (
                <span style={{ color: entry.color, fontWeight: "bold" }}>{value}</span>
              )}
            />
          </PieChart>
        </ResponsiveContainer>
      </div>

      {/* Bar Chart */}
      <div className="bg-white shadow-lg rounded-xl p-6 flex flex-col">
        <h3 className="text-xl font-semibold mb-4 text-gray-700">Documents by Category</h3>
        <ResponsiveContainer width="100%" height={300}>
          <BarChart data={barData} margin={{ top: 20, right: 20, left: 0, bottom: 5 }}>
            <XAxis dataKey="name" tick={{ fontSize: 12, fill: "#4b5563" }} />
            <YAxis tick={{ fontSize: 12, fill: "#4b5563" }} />
            <Tooltip
              contentStyle={{
                backgroundColor: "#fff",
                borderRadius: 8,
                boxShadow: "0 2px 10px rgba(0,0,0,0.15)",
              }}
            />
            <Legend
              verticalAlign="bottom"
              height={36}
              formatter={(value, entry) => (
                <span style={{ color: entry.color, fontWeight: "bold" }}>{value}</span>
              )}
            />
            <Bar dataKey="value" isAnimationActive={true} animationDuration={1000} animationEasing="ease-out">
              {barData.map((entry, index) => (
                <Cell key={index} fill={entry.fill} />
              ))}
              <LabelList dataKey="value" position="top" fill="#111" fontSize={12} />
            </Bar>
          </BarChart>
        </ResponsiveContainer>
      </div>
    </div>
  );
};

export default CourseStats;
