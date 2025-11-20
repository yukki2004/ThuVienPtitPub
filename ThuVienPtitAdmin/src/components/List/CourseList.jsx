import React from "react";
import ReactPaginate from "react-paginate";
import CourseCard from "../card/CourseCard";
const CourseList = ({
  courses,
  loading,
  pageNumber,
  totalPage,
  onPageChange,
  onEdit,
  onDelete,
  onView,
}) => {
  if (loading) return <p>Đang tải dữ liệu...</p>;
  if (!courses || courses.length === 0) return <p>Không có học phần nào.</p>;

  const handlePageClick = (event) => onPageChange(event.selected + 1);

  return (
    <div>
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
        {courses.map((course) => (
          <CourseCard
            key={course.course_id}
            course={course}
            onEdit={onEdit}
            onDelete={onDelete}
            onView={onView}
          />
        ))}
      </div>

      <div className="mt-6 flex justify-center">
        <ReactPaginate
          previousLabel={"← Trước"}
          nextLabel={"Sau →"}
          breakLabel={"..."}
          pageCount={totalPage}
          forcePage={pageNumber - 1}
          marginPagesDisplayed={2}
          pageRangeDisplayed={3}
          onPageChange={handlePageClick}
          containerClassName="flex items-center gap-2 text-sm"
          pageClassName="border rounded-lg px-3 py-1 cursor-pointer hover:bg-blue-100"
          activeClassName="bg-blue-500 text-white"
          previousClassName="border rounded-lg px-3 py-1 cursor-pointer hover:bg-blue-100"
          nextClassName="border rounded-lg px-3 py-1 cursor-pointer hover:bg-blue-100"
          disabledClassName="opacity-50 cursor-not-allowed"
        />
      </div>
    </div>
  );
};

export default CourseList;
