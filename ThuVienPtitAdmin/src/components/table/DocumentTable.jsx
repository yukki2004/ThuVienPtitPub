import { FaCheckCircle, FaClock, FaTrash, FaUndo } from "react-icons/fa";
import { useNavigate } from "react-router-dom";

const statusMap = {
  0: { label: "Pending", color: "bg-yellow-100 text-yellow-800", icon: <FaClock className="inline mr-1" /> },
  1: { label: "Approved", color: "bg-green-100 text-green-800", icon: <FaCheckCircle className="inline mr-1" /> },
  3: { label: "Deleted", color: "bg-red-100 text-red-800", icon: <FaTrash className="inline mr-1" /> },
};

const DocumentTable = ({
  documents,
  loading,
  onApprove,
  onReject,
  onDelete,
  onRestore,
  onClear,
}) => {
  const navigate = useNavigate();

  if (loading)
    return (
      <p className="text-center py-10 text-gray-500 font-medium">Loading documents...</p>
    );

  return (
    <div className="overflow-x-auto border rounded-lg shadow-sm">
      <table className="min-w-full divide-y divide-gray-200">
        <thead className="bg-gray-50 sticky top-0">
          <tr>
            <th className="px-4 py-3 text-left text-sm font-medium text-gray-700">#</th>
            <th className="px-4 py-3 text-left text-sm font-medium text-gray-700">Title</th>
            <th className="px-4 py-3 text-left text-sm font-medium text-gray-700">Category</th>
            <th className="px-4 py-3 text-left text-sm font-medium text-gray-700">Status</th>
            <th className="px-4 py-3 text-left text-sm font-medium text-gray-700">Created At</th>
            <th className="px-4 py-3 text-left text-sm font-medium text-gray-700">Actions</th>
          </tr>
        </thead>
        <tbody className="bg-white divide-y divide-gray-200">
          {documents.map((doc, index) => (
            <tr
              key={doc.documentId}
              className="hover:bg-gray-50 transition-colors duration-150"
            >
              <td className="px-4 py-3">{index + 1}</td>
              <td className="px-4 py-3 flex items-center gap-2 font-medium">
                {doc.avtUrl && (
                  <img
                    src={doc.avtUrl}
                    alt={doc.title}
                    className="w-8 h-8 rounded-full object-cover"
                  />
                )}
                {doc.title}
              </td>
              <td className="px-4 py-3">{doc.category}</td>
              <td className="px-4 py-3">
                <span
                  className={`inline-flex items-center px-2 py-1 rounded-full text-sm font-semibold ${statusMap[doc.status]?.color}`}
                >
                  {statusMap[doc.status]?.icon}
                  {statusMap[doc.status]?.label}
                </span>
              </td>
              <td className="px-4 py-3">{new Date(doc.createdAt).toLocaleDateString()}</td>
              <td className="px-4 py-3 flex flex-wrap gap-2">
                {/* Navigate to document detail page */}
                <button
                  className="bg-gray-200 hover:bg-gray-300 px-3 py-1 rounded text-sm transition"
                  onClick={() => navigate(`/admin/documents/${doc.slug}`)}
                >
                  View
                </button>

                {doc.status === 0 && (
                  <>
                    <button
                      className="bg-green-500 hover:bg-green-600 text-white px-3 py-1 rounded text-sm transition"
                      onClick={() => onApprove(doc)}
                    >
                      Approve
                    </button>
                    <button
                      className="bg-red-500 hover:bg-red-600 text-white px-3 py-1 rounded text-sm transition"
                      onClick={() => onReject(doc)}
                    >
                      Reject
                    </button>
                  </>
                )}

                {doc.status === 1 && (
                  <button
                    className="bg-red-500 hover:bg-red-600 text-white px-3 py-1 rounded text-sm transition"
                    onClick={() => onDelete(doc)}
                  >
                    Delete
                  </button>
                )}

                {doc.status === 3 && (
                  <>
                    <button
                      className="bg-green-500 hover:bg-green-600 text-white px-3 py-1 rounded text-sm transition flex items-center gap-1"
                      onClick={() => onRestore(doc)}
                    >
                      <FaUndo /> Restore
                    </button>
                    <button
                      className="bg-red-700 hover:bg-red-800 text-white px-3 py-1 rounded text-sm transition"
                      onClick={() => onClear(doc)}
                    >
                      Clear
                    </button>
                  </>
                )}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default DocumentTable;
