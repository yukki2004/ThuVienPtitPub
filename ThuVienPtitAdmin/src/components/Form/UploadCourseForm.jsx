import React, { useState } from "react";
import { FaBook, FaInfoCircle, FaLayerGroup, FaCalendarAlt, FaPlus } from "react-icons/fa";
import {
  createCourseApi,
  getCategoryCourseApi,
} from "../../services/courseService";
import { getAllSemesterApi } from "../../services/semesterService";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

const UploadCourseForm = ({ onUploadSuccess }) => {
  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const [credits, setCredits] = useState(0);
  const [semester, setSemester] = useState([]);
  const [category, setCategory] = useState([]);
  const [selectSemester, setSelectSemester] = useState("");
  const [selectCategory, setSelectCategory] = useState("");
  const [loadingForm, setLoadingForm] = useState(false);
  const [loadingDropdown, setLoadingDropdown] = useState(false);

  const handleOpenSemester = async () => {
    if (semester.length === 0 && !loadingDropdown) {
      setLoadingDropdown(true);
      try {
        const res = await getAllSemesterApi();
        setSemester(res);
      } catch (err) {
        toast.error("Lỗi tải danh sách học kỳ");
        console.error(err);
      } finally {
        setLoadingDropdown(false);
      }
    }
  };

  const handleOpenCategory = async () => {
    if (category.length === 0 && !loadingDropdown) {
      setLoadingDropdown(true);
      try {
        const res = await getCategoryCourseApi();
        setCategory(res);
      } catch (err) {
        toast.error("Lỗi tải danh sách loại học phần");
        console.error(err);
      } finally {
        setLoadingDropdown(false);
      }
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (loadingForm) return;
    try {
      setLoadingForm(true);
      await createCourseApi({
        name,
        description,
        credits: Number(credits),
        semester_id: selectSemester,
        category_id: selectCategory,
      });

      toast.success("Upload học phần thành công 🎉", {
        position: "bottom-right",
      });

      onUploadSuccess();
      setName("");
      setDescription("");
      setCredits(0);
      setSelectSemester("");
      setSelectCategory("");
    } catch (err) {
      console.error(err);
      const msg =
        err.response?.data?.message ||
        err.message ||
        "Đã xảy ra lỗi không xác định";
      toast.error(`❌ ${msg}`, { position: "bottom-right" });
    } finally {
      setLoadingForm(false);
    }
  };

  const inputClass =
    "border border-gray-300 rounded-lg p-3 pl-10 focus:outline-none focus:ring-2 focus:ring-blue-400 transition";

  const labelClass = "mb-1 text-gray-600 font-medium flex items-center gap-1";

  return (
    <>
      <form
        onSubmit={handleSubmit}
        className="bg-white shadow-xl rounded-2xl p-6 mb-6 max-w-3xl mx-auto border border-gray-200"
      >
        <h2 className="text-2xl font-semibold mb-6 text-gray-700 text-center flex items-center justify-center gap-2">
          <FaPlus className="text-blue-500" /> Thêm học phần mới
        </h2>

        <div className="grid grid-cols-1 md:grid-cols-2 gap-5">
          {/* Name */}
          <div className="relative flex flex-col">
            <label className={labelClass}>
              <FaBook className="text-gray-400" /> Tên học phần
            </label>
            <input
              name="name"
              placeholder="Nhập tên học phần..."
              value={name}
              onChange={(e) => setName(e.target.value)}
              className={inputClass}
              disabled={loadingForm}
              required
            />
            <FaInfoCircle className="absolute left-3 top-10 text-gray-400" title="Tên học phần" />
          </div>

          {/* Credits */}
          <div className="relative flex flex-col">
            <label className={labelClass}>
              <FaLayerGroup className="text-gray-400" /> Số tín chỉ
            </label>
            <input
              name="credits"
              type="number"
              placeholder="Nhập số tín chỉ"
              value={credits}
              onChange={(e) => setCredits(e.target.value)}
              className={inputClass}
              disabled={loadingForm}
              min={0}
              required
            />
          </div>

          {/* Description */}
          <div className="relative flex flex-col md:col-span-2">
            <label className={labelClass}>
              <FaInfoCircle className="text-gray-400" /> Mô tả
            </label>
            <textarea
              name="description"
              placeholder="Nhập mô tả học phần..."
              value={description}
              onChange={(e) => setDescription(e.target.value)}
              className={`${inputClass} resize-none h-24 pl-10`}
              disabled={loadingForm}
            />
          </div>

          {/* Semester */}
          <div className="relative flex flex-col">
            <label className={labelClass}>
              <FaCalendarAlt className="text-gray-400" /> Học kỳ
            </label>
            <select
              name="semester_id"
              value={selectSemester}
              onChange={(e) => setSelectSemester(e.target.value)}
              onClick={handleOpenSemester}
              className={inputClass}
              disabled={loadingForm}
              required
            >
              <option value="">Chọn học kỳ</option>
              {semester.map((s) => (
                <option key={s.semester_id} value={s.semester_id}>
                  {s.name} (Năm {s.year})
                </option>
              ))}
            </select>
          </div>

          {/* Category */}
          <div className="relative flex flex-col">
            <label className={labelClass}>
              <FaLayerGroup className="text-gray-400" /> Loại học phần
            </label>
            <select
              name="category_id"
              value={selectCategory}
              onChange={(e) => setSelectCategory(e.target.value)}
              onClick={handleOpenCategory}
              className={inputClass}
              disabled={loadingForm}
              required
            >
              <option value="">Chọn loại học phần</option>
              {category.map((c) => (
                <option key={c.category_id} value={c.category_id}>
                  {c.name}
                </option>
              ))}
            </select>
          </div>
        </div>

        {/* Submit Button */}
        <button
          type="submit"
          disabled={loadingForm}
          className={`mt-6 w-full py-3 rounded-xl text-white font-semibold flex justify-center items-center gap-2 ${
            loadingForm
              ? "bg-gray-400 cursor-not-allowed"
              : "bg-gradient-to-r from-blue-500 to-blue-600 hover:from-blue-600 hover:to-blue-700 transition"
          }`}
        >
          {loadingForm ? "Đang thêm..." : <><FaPlus /> Thêm học phần</>}
        </button>
      </form>

      <ToastContainer position="bottom-right" autoClose={3000} />
    </>
  );
};

export default UploadCourseForm;
