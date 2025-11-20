import React from "react";
import guest from "../../assets/guest.png";

const PenDocumentCard = ({ document, onApprove, onReject, onView }) => {
  const {
    title,
    description,
    avt_document,
    course,
    user,
    created_at,
    tags,
    document_id,
  } = document;

  return (
    <div className="flex bg-white shadow-md rounded-xl overflow-hidden hover:shadow-lg transition-shadow duration-300 border border-gray-100">
      <div className="w-32 h-32 flex-shrink-0">
        <img
          src={avt_document || "/default-avt.png"}
          alt="Document"
          className="w-full h-full object-cover"
        />
      </div>
      <div className="flex-1 p-4 flex flex-col justify-between">
        <div>
          <h3 className="text-lg font-semibold text-gray-800">{title}</h3>
          {description && (
            <p className="text-sm text-gray-600 mt-1 line-clamp-2">{description}</p>
          )}
          <div className="flex items-center gap-2 mt-2">
            <img
              src={user?.avt || guest}
              alt={user?.name}
              className="w-6 h-6 rounded-full object-cover"
            />
            <span className="text-xs text-gray-500">
              {user?.name || "Unknown"}
            </span>
          </div>
          <div className="flex flex-wrap gap-2 mt-2">
            {tags?.map((tag) => (
              <span
                key={tag.tag_id}
                className="text-xs bg-gray-200 px-2 py-1 rounded-full"
              >
                {tag.name}
              </span>
            ))}
          </div>

          <div className="text-xs text-gray-400 mt-2">
            {course?.name} | {course?.semester?.name} |{" "}
            {new Date(created_at).toLocaleDateString()}
          </div>
        </div>

        <div className="flex gap-2 mt-4 justify-end">
          <button
            onClick={() => onView(document)}
            className="px-3 py-1 bg-blue-500 hover:bg-blue-600 text-white rounded-lg transition-colors duration-200"
          >
            Xem chi tiết
          </button>
          <button
            onClick={() => onReject(document_id)}
            className="px-3 py-1 bg-red-100 text-red-600 border border-red-300 hover:bg-red-200 rounded-lg transition-colors duration-200"
          >
            Từ chối
          </button>
          <button
            onClick={() => onApprove(document_id)}
            className="px-3 py-1 bg-green-500 hover:bg-green-600 text-white rounded-lg transition-colors duration-200"
          >
            Duyệt
          </button>
        </div>
      </div>
    </div>
  );
};

export default PenDocumentCard;
