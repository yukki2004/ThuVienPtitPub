import { FaRegCalendarAlt, FaBook, FaLayerGroup, FaStar } from "react-icons/fa";

const CourseInfoCard = ({ course }) => {
  return (
    <div className="bg-white border border-gray-200 rounded-xl shadow-md p-6 mb-6 hover:shadow-xl transform hover:-translate-y-1 transition-all duration-200">
      {/* Header */}
      <div className="flex justify-between items-start mb-4">
        <div>
          <h2 className="text-2xl font-bold text-gray-800">{course.name}</h2>
          {course.description && (
            <p className="text-sm text-gray-500 mt-1">{course.description}</p>
          )}
        </div>
        <span className="text-sm text-gray-500 flex items-center gap-1 mt-1">
          <FaRegCalendarAlt /> {course.semester.name}
        </span>
      </div>

      {/* Info Grid */}
      <div className="grid grid-cols-2 md:grid-cols-4 gap-6 text-gray-700 mt-4">
        <div className="flex items-center gap-2">
          <FaStar className="text-yellow-400" />
          <div>
            <p className="text-sm font-medium">ID</p>
            <p className="text-xs text-gray-500 truncate">{course.courseId}</p>
          </div>
        </div>

        <div className="flex items-center gap-2">
          <FaLayerGroup className="text-blue-400" />
          <div>
            <p className="text-sm font-medium">Category</p>
            <p className="text-xs text-gray-500">{course.category.name}</p>
          </div>
        </div>

        <div className="flex items-center gap-2">
          <FaBook className="text-green-400" />
          <div>
            <p className="text-sm font-medium">Credits</p>
            <p className="text-xs text-gray-500">{course.credits}</p>
          </div>
        </div>

        <div className="flex items-center gap-2">
          <FaRegCalendarAlt className="text-purple-400" />
          <div>
            <p className="text-sm font-medium">Created At</p>
            <p className="text-xs text-gray-500">{new Date(course.createdAt).toLocaleDateString()}</p>
          </div>
        </div>
      </div>
    </div>
  );
};

export default CourseInfoCard;
