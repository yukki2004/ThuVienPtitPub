import React, { useEffect, useState } from "react";
import { getCourseListApi } from "../services/courseService";
import { getAllSemesterApi } from "../services/semesterService";
import UploadCourseForm from "../components/Form/UploadCourseForm";
import CourseList from "../components/List/CourseList";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { deleteCourseApi } from "../services/courseService";
import EditCourseModal from "../components/Form/UpdateCourseForm";
import { useNavigate } from "react-router-dom";
const SubjectPage = () => {
  const [courses, setCourses] = useState([]);
  const [pageNumber, setPageNumber] = useState(1);
  const [totalPage, setTotalPage] = useState(1);
  const [loading, setLoading] = useState(false);
  const [semesters, setSemesters] = useState([]);
  const [selectedSemester, setSelectedSemester] = useState("");
  const [editingCourse, setEditingCourse] = useState(null);
  const [isEditOpen, setIsEditOpen] = useState(false);
  const navigate = useNavigate()
  const handleDeleteCourse = async (id) => {
  if (window.confirm("Bạn có chắc muốn xóa học phần này không?")) {
    try {
      await deleteCourseApi(id);
      toast.success("✅ Xóa học phần thành công!");
      fetchCourses();
    } catch {
      toast.error("❌ Lỗi khi xóa học phần!");
    }
  }
};
  const fetchSemesters = async () => {
    try {
      const res = await getAllSemesterApi();
      setSemesters(res);
    } catch (err) {
      console.error(err);
      toast.error("❌ Lỗi khi tải danh sách học kỳ!");
    }
  };

  const fetchCourses = async () => {
    try {
      setLoading(true);
      const res = await getCourseListApi(pageNumber, selectedSemester || null);
      setCourses(res.coursePage || []);
      setTotalPage(res.tolTalPage || 1);
    } catch (err) {
      toast.error(err?.response?.data?.message || "❌ Lỗi khi tải danh sách học phần!");
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchCourses();
  }, [pageNumber, selectedSemester]);

  useEffect(() => {
    fetchSemesters();
  }, []);

  return (
    <div className="p-6 max-w-6xl mx-auto">
      <h1 className="text-2xl font-bold mb-6">Quản lý học phần</h1>

      <UploadCourseForm
        onUploadSuccess={() => {
          toast.success("✅ Thêm học phần thành công!", { position: "bottom-right" });
          fetchCourses();
        }}
      />

      <div className="mb-6 flex items-center gap-3">
        <label className="font-medium text-gray-700">Lọc theo học kỳ:</label>
        <select
          value={selectedSemester}
          onChange={(e) => {
            setPageNumber(1);
            setSelectedSemester(e.target.value);
          }}
          className="border p-2 rounded min-w-[200px]"
        >
          <option value="">Tất cả học kỳ</option>
          {semesters.map((s) => (
            <option key={s.semester_id} value={s.semester_id}>
              {s.name} (Năm {s.year})
            </option>
          ))}
        </select>
      </div>
      <CourseList
        courses={courses}
        loading={loading}
        pageNumber={pageNumber}
        totalPage={totalPage}
        onPageChange={setPageNumber}
        onEdit={(course) => {
          setEditingCourse(course);
          setIsEditOpen(true);
        }}
        onDelete={handleDeleteCourse}
        onView={(slug) => navigate(`/admin/subjects/${slug}`)}
      />
      <EditCourseModal
        isOpen={isEditOpen}
        onClose={() => setIsEditOpen(false)}
        course={editingCourse}
        onSuccess={fetchCourses}
      />
      <ToastContainer position="bottom-right" autoClose={3000} />
    </div>
  );
};

export default SubjectPage;
