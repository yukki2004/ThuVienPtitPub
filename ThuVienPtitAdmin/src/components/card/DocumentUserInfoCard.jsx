import React from "react";
import guest from "../../assets/guest.png";

const DocumentUserInfoCard = ({ document, actions = [] }) => {
  const {
    title,
    description,
    avt_document,
    course,
    userInfoDto,
    created_at,
    tags,
  } = document;

  return (
    <div className="flex bg-white shadow-md rounded-xl overflow-hidden hover:shadow-lg transition border border-gray-100">
      <div className="w-32 h-32 flex-shrink-0">
        <img
          src={avt_document || guest}
          alt="Document"
          className="w-full h-full object-cover"
        />
      </div>
      <div className="flex-1 p-4 flex flex-col justify-between">
        <div>
          <h3 className="text-lg font-semibold text-gray-800 line-clamp-1">{title}</h3>
          {description && (
            <p className="text-sm text-gray-600 mt-1 line-clamp-2">{description}</p>
          )}

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

        <div className="flex gap-2 mt-3 justify-end">
          {actions.map((act) => (
            <button
              key={act.label}
              onClick={act.onClick}
              className={`px-3 py-1 rounded-lg text-sm transition ${
                act.type === "view"
                  ? "bg-blue-500 text-white hover:bg-blue-600"
                  : act.type === "approve"
                  ? "bg-green-500 text-white hover:bg-green-600"
                  : act.type === "reject"
                  ? "bg-red-100 text-red-600 border border-red-300 hover:bg-red-200"
                  : "bg-gray-100 text-gray-600 hover:bg-gray-200"
              }`}
            >
              {act.label}
            </button>
          ))}
        </div>
      </div>
    </div>
  );
};

export default DocumentUserInfoCard;
