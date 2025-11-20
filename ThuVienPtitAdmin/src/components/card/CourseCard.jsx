import React from "react";

const CourseCard = ({ course, onEdit, onDelete, onView }) => {
  return (
    <div className="border rounded-2xl p-4 shadow-md bg-white hover:shadow-lg transition duration-200">
      <h3 className="text-lg font-semibold text-blue-600 mb-1">{course.name}</h3>
      <p className="text-gray-700 text-sm mb-2 line-clamp-2">
        {course.description || "Không có mô tả"}
      </p>

      <div className="text-sm text-gray-600 space-y-1">
        <p>
          <span className="font-medium">🎓 Tín chỉ:</span> {course.credits}
        </p>
        <p>
          <span className="font-medium">📘 Học kỳ:</span>{" "}
          {course.semesterDto?.name || "—"}
        </p>
        <p>
          <span className="font-medium">🏷️ Loại:</span>{" "}
          {course.category_Course?.name || "—"}
        </p>
      </div>

      <div className="flex gap-2 mt-4">
        <button
          onClick={() => onView(course.slug)}
          className="bg-blue-500 hover:bg-blue-600 text-white px-3 py-1 rounded-lg text-sm transition"
        >
          Xem chi tiết
        </button>
        <button
          onClick={() => onEdit(course)}
          className="bg-yellow-400 hover:bg-yellow-500 text-white px-3 py-1 rounded-lg text-sm transition"
        >
          Chỉnh sửa
        </button>
        <button
          onClick={() => onDelete(course.course_id)}
          className="bg-red-500 hover:bg-red-600 text-white px-3 py-1 rounded-lg text-sm transition"
        >
          Xóa
        </button>
      </div>
    </div>
  );
};

export default CourseCard;
