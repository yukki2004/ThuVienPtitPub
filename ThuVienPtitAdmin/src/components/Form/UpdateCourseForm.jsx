import React, { useEffect, useState } from "react";
import { motion, AnimatePresence } from "framer-motion";
import { getCategoryCourseApi } from "../../services/courseService";
import { updateCourseApi } from "../../services/courseService";
import { toast } from "react-toastify";

const EditCourseModal = ({ isOpen, onClose, course, onSuccess }) => {
  const [form, setForm] = useState({
    description: "",
    credits: 0,
    category_id: "",
  });
  const [categories, setCategories] = useState([]);
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    if (course) {
      setForm({
        description: course.description || "",
        credits: course.credits || 0,
        category_id: course.category_Course?.category_id || "",
      });
    }
  }, [course]);

  useEffect(() => {
    if (isOpen) {
      getCategoryCourseApi()
        .then((res) => setCategories(res))
        .catch(() => toast.error("❌ Lỗi khi tải danh mục học phần!"));
    }
  }, [isOpen]);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setSaving(true);
    try {
      await updateCourseApi({
        id: course.course_id,
        description: form.description,
        credits: form.credits,
        category_id: form.category_id,
      });
      toast.success("✅ Cập nhật học phần thành công!");
      onSuccess();
      onClose();
    } catch {
      toast.error("❌ Lỗi khi cập nhật học phần!");
    } finally {
      setSaving(false);
    }
  };

  return (
    <AnimatePresence>
      {isOpen && (
        <motion.div
          initial={{ opacity: 0 }}
          animate={{ opacity: 1 }}
          exit={{ opacity: 0 }}
          className="fixed inset-0 bg-black/50 flex items-center justify-center z-50"
        >
          <motion.div
            initial={{ scale: 0.9, opacity: 0 }}
            animate={{ scale: 1, opacity: 1 }}
            exit={{ scale: 0.9, opacity: 0 }}
            transition={{ duration: 0.25 }}
            className="bg-white p-6 rounded-2xl shadow-2xl w-[420px] relative"
          >
            <button
              onClick={onClose}
              className="absolute top-3 right-3 text-gray-500 hover:text-gray-800"
            >
              ✕
            </button>

            <h3 className="text-lg font-semibold mb-4 text-center text-blue-600">
              ✏️ Chỉnh sửa học phần
            </h3>

            <form onSubmit={handleSubmit}>
              <label className="block mb-2 text-sm font-medium">Mô tả</label>
              <textarea
                value={form.description}
                onChange={(e) => setForm({ ...form, description: e.target.value })}
                className="w-full border rounded-lg p-2 mb-3 focus:ring-2 focus:ring-blue-400"
                rows="3"
              />

              <label className="block mb-2 text-sm font-medium">Tín chỉ</label>
              <input
                type="number"
                value={form.credits}
                onChange={(e) => setForm({ ...form, credits: parseInt(e.target.value) })}
                className="w-full border rounded-lg p-2 mb-3 focus:ring-2 focus:ring-blue-400"
              />

              <label className="block mb-2 text-sm font-medium">Loại học phần</label>
              <select
                value={form.category_id}
                onChange={(e) => setForm({ ...form, category_id: e.target.value })}
                className="w-full border rounded-lg p-2 mb-4 focus:ring-2 focus:ring-blue-400"
              >
                <option value="">-- Chọn loại học phần --</option>
                {categories.map((c) => (
                  <option key={c.category_id} value={c.category_id}>
                    {c.name}
                  </option>
                ))}
              </select>

              <div className="flex justify-end gap-3">
                <button
                  type="button"
                  onClick={onClose}
                  className="px-4 py-2 rounded-lg bg-gray-300 hover:bg-gray-400 transition"
                >
                  Hủy
                </button>
                <button
                  type="submit"
                  disabled={saving}
                  className={`px-4 py-2 rounded-lg text-white transition ${
                    saving ? "bg-blue-300" : "bg-blue-500 hover:bg-blue-600"
                  }`}
                >
                  {saving ? "Đang lưu..." : "Lưu thay đổi"}
                </button>
              </div>
            </form>
          </motion.div>
        </motion.div>
      )}
    </AnimatePresence>
  );
};

export default EditCourseModal;
