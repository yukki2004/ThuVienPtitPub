import { useNavigate } from "react-router-dom";
import guest from "../../assets/guest.png";

const DelDocumentCard = ({ document, onApprove, onReject, onDelete, onRestore, onClear, mode }) => {
  const {
    title,
    description,
    avt_document,
    course,
    user,
    created_at,
    tags,
    slug,
    document_id,
  } = document;
  const navigate = useNavigate();
  const handleView = () => {
   navigate(`/admin/documents/${slug}`);
  };

  return (
    <div className="flex bg-white shadow-md rounded-xl overflow-hidden hover:shadow-xl transition-shadow duration-300">
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
            <p className="text-sm text-gray-600 mt-1">{description}</p>
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

        {/* --- Các nút hành động --- */}
        <div className="flex gap-2 mt-4 justify-end">
          <button
            onClick={handleView}
            className="px-3 py-1 bg-blue-500 hover:bg-blue-600 text-white rounded-lg transition-colors duration-200"
          >
            Xem chi tiết
          </button>

          {mode === "published" && (
            <button
              onClick={() => onDelete && onDelete(document_id)}
              className="px-3 py-1 bg-red-500 hover:bg-red-600 text-white rounded-lg transition-colors duration-200"
            >
              Xóa
            </button>
          )}

          {mode === "pending" && (
            <>
              <button
                onClick={() => onApprove && onApprove(document_id)}
                className="px-3 py-1 bg-green-500 hover:bg-green-600 text-white rounded-lg transition-colors duration-200"
              >
                Duyệt
              </button>
              <button
                onClick={() => onReject && onReject(document_id)}
                className="px-3 py-1 bg-red-400 hover:bg-red-500 text-white rounded-lg transition-colors duration-200"
              >
                Từ chối
              </button>
            </>
          )}

          {mode === "deleted" && (
            <>
              <button
                onClick={() => onRestore && onRestore(document_id)}
                className="px-3 py-1 bg-green-500 hover:bg-green-600 text-white rounded-lg transition-colors duration-200"
              >
                Khôi phục
              </button>
              <button
                onClick={() => onClear && onClear(document_id)}
                className="px-3 py-1 bg-red-500 hover:bg-red-600 text-white rounded-lg transition-colors duration-200"
              >
                Xóa vĩnh viễn
              </button>
            </>
          )}
        </div>
      </div>
    </div>
  );
};

export default DelDocumentCard;
