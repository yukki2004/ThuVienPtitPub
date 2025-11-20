import React, { useState } from "react";
import { createTagApi } from "../../services/tagsService";
import { toast } from "react-toastify";

const FormUploadTag = ({ onUploadSuccess }) => {
  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!name.trim()) {
      toast.warning("Tên tag không được để trống");
      return;
    }

    setLoading(true);
    try {
      await createTagApi({ name, description });
      toast.success("Tạo tag thành công!");
      setName("");
      setDescription("");
      onUploadSuccess();
    } catch {
      toast.error("Lỗi khi tạo tag!");
    } finally {
      setLoading(false);
    }
  };

  return (
    <form
      onSubmit={handleSubmit}
      className="p-4 bg-white shadow rounded-md mb-6"
    >
      <h2 className="text-lg font-semibold mb-4">Thêm tag mới</h2>
      <div className="mb-3">
        <label className="block mb-1 font-medium">Tên tag</label>
        <input
          type="text"
          className="border p-2 w-full rounded"
          value={name}
          onChange={(e) => setName(e.target.value)}
          placeholder="Nhập tên tag..."
        />
      </div>
      <div className="mb-3">
        <label className="block mb-1 font-medium">Mô tả</label>
        <textarea
          className="border p-2 w-full rounded"
          value={description}
          onChange={(e) => setDescription(e.target.value)}
          placeholder="Nhập mô tả (nếu có)..."
        />
      </div>
      <button
        type="submit"
        disabled={loading}
        className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
      >
        {loading ? "Đang lưu..." : "Thêm Tag"}
      </button>
    </form>
  );
};

export default FormUploadTag;
