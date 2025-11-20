import React from "react";
import { deleteTagApi } from "../../services/tagsService";
import { toast } from "react-toastify";
import { useNavigate } from "react-router-dom";

const TagList = ({ tags, pageNumber, totalPage, onPageChange, onRefresh }) => {
  const navigate = useNavigate();

  const handleDelete = async (id) => {
    if (!window.confirm("Bạn có chắc muốn xóa tag này?")) return;
    try {
      await deleteTagApi(id);
      toast.success("Xóa tag thành công!");
      onRefresh();
    } catch {
      toast.error("Lỗi khi xóa tag!");
    }
  };

  return (
    <div className="bg-white p-4 shadow rounded-md">
      <h2 className="text-lg font-semibold mb-4">Danh sách tag</h2>
      <table className="w-full border-collapse border">
        <thead className="bg-gray-100">
          <tr>
            <th className="border p-2">ID</th>
            <th className="border p-2">Tên tag</th>
            <th className="border p-2">Slug</th>
            <th className="border p-2">Ngày tạo</th>
            <th className="border p-2">Mô tả</th>
            <th className="border p-2">Hành động</th>
          </tr>
        </thead>
        <tbody>
          {tags.length > 0 ? (
            tags.map((t) => (
              <tr key={t.tag_id}>
                <td className="border p-2 text-center">{t.tag_id}</td>
                <td className="border p-2">{t.name}</td>
                <td className="border p-2">{t.slug}</td>
                <td className="border p-2">
                  {new Date(t.created_at).toLocaleString()}
                </td>
                <td className="border p-2">{t.description || "-"}</td>
                <td className="border p-2 text-center space-x-2">
                  <button
                    onClick={() => navigate(`/admin/tag/${t.tag_id}`)}
                    className="bg-green-500 text-white px-2 py-1 rounded hover:bg-green-600"
                  >
                    Xem
                  </button>
                  <button
                    onClick={() => handleDelete(t.tag_id)}
                    className="bg-red-500 text-white px-2 py-1 rounded hover:bg-red-600"
                  >
                    Xóa
                  </button>
                </td>
              </tr>
            ))
          ) : (
            <tr>
              <td colSpan="6" className="text-center p-4">
                Không có tag nào
              </td>
            </tr>
          )}
        </tbody>
      </table>

      {/* Pagination */}
      <div className="flex justify-center mt-4">
        <button
          disabled={pageNumber <= 1}
          onClick={() => onPageChange(pageNumber - 1)}
          className="px-3 py-1 mx-1 border rounded disabled:opacity-50"
        >
          &lt;
        </button>
        <span className="px-3 py-1">
          Trang {pageNumber}/{totalPage}
        </span>
        <button
          disabled={pageNumber >= totalPage}
          onClick={() => onPageChange(pageNumber + 1)}
          className="px-3 py-1 mx-1 border rounded disabled:opacity-50"
        >
          &gt;
        </button>
      </div>
    </div>
  );
};

export default TagList;
