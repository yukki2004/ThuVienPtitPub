const DocumentFilters = ({ category, status, onCategoryChange, onStatusChange }) => {
  return (
    <div className="flex flex-wrap items-center gap-6 mb-6 bg-gray-50 p-4 rounded-lg shadow-sm">
      <div className="flex flex-col">
        <label className="text-sm font-medium text-gray-700 mb-1">Category</label>
        <select
          value={category || ""}
          onChange={(e) => onCategoryChange(e.target.value)}
          className="border border-gray-300 rounded-lg px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-400"
        >
          <option value="">All Categories</option>
          <option value="Giáo trình">Giáo trình</option>
          <option value="Đề thi">Đề thi</option>
          <option value="Slide">Slide</option>
          <option value="Tài liệu khác">Tài liệu khác</option>
        </select>
      </div>
      <div className="flex flex-col">
        <label className="text-sm font-medium text-gray-700 mb-1">Status</label>
        <select
          value={status ?? ""}
          onChange={(e) => {
            const val = e.target.value;
            onStatusChange(val === "" ? null : Number(val));
          }}
          className="border border-gray-300 rounded-lg px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-400"
        >
          <option value="">All Statuses</option>
          <option value={1}>Approved</option>
          <option value={0}>Pending</option>
          <option value={3}>Deleted</option>
        </select>
      </div>
    </div>
  );
};

export default DocumentFilters;
