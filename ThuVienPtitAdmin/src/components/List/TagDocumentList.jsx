import React from "react";
import { useNavigate } from "react-router-dom";
import {
  deleteDocumentApi,
  rejectDocumentApi,
  approveDocumentApi,
  clearDocumentApi,
  restoreDocumentApi,
} from "../../services/documentService";
import { toast } from "react-toastify";

export default function TagDocumentList({
  documents,
  pageNumber,
  totalPage,
  onPageChange,
  onRefresh,
}) {
  const navigate = useNavigate();

  const handleAction = async (type, id) => {
    try {
      if (type === "delete") await deleteDocumentApi(id);
      if (type === "reject") await rejectDocumentApi(id);
      if (type === "approve") await approveDocumentApi(id);
      if (type === "clear") await clearDocumentApi(id);
      if (type === "restore") await restoreDocumentApi(id);

      toast.success("Thao tác thành công!");
      onRefresh();
    } catch {
      toast.error("Thao tác thất bại!");
    }
  };

  return (
    <div className="bg-white shadow rounded-md p-4">
      <h2 className="text-lg font-semibold mb-4">Danh sách tài liệu</h2>

      {documents.length === 0 ? (
        <p className="text-gray-500 italic">Không có tài liệu nào.</p>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
          {documents.map((doc) => (
            <div
              key={doc.document_id}
              className="border rounded-lg overflow-hidden shadow-sm hover:shadow-md transition bg-gray-50"
            >
              <img
                src={doc.avt_document}
                alt={doc.title}
                className="w-full h-40 object-cover"
              />
              <div className="p-4 space-y-2">
                <h3 className="font-semibold text-lg">{doc.title}</h3>
                <p className="text-sm text-gray-600 line-clamp-2">
                  {doc.description || "Không có mô tả"}
                </p>
                <p className="text-xs text-gray-400">
                  Ngày tạo: {new Date(doc.created_at).toLocaleString()}
                </p>

                <div className="flex flex-wrap gap-2 mt-3">
                  {doc.status === 0 && (
                    <>
                      <button
                        onClick={() => handleAction("approve", doc.document_id)}
                        className="bg-green-500 text-white px-3 py-1 rounded text-sm hover:bg-green-600"
                      >
                        Duyệt
                      </button>
                      <button
                        onClick={() => handleAction("reject", doc.document_id)}
                        className="bg-yellow-500 text-white px-3 py-1 rounded text-sm hover:bg-yellow-600"
                      >
                        Từ chối
                      </button>
                    </>
                  )}
                  {doc.status === 1 && (
                    <button
                      onClick={() => handleAction("delete", doc.document_id)}
                      className="bg-red-500 text-white px-3 py-1 rounded text-sm hover:bg-red-600"
                    >
                      Xóa
                    </button>
                  )}
                  {doc.status === 3 && (
                    <>
                      <button
                        onClick={() => handleAction("clear", doc.document_id)}
                        className="bg-gray-600 text-white px-3 py-1 rounded text-sm hover:bg-gray-700"
                      >
                        Clear
                      </button>
                      <button
                        onClick={() => handleAction("restore", doc.document_id)}
                        className="bg-green-500 text-white px-3 py-1 rounded text-sm hover:bg-green-600"
                      >
                        Khôi phục
                      </button>
                    </>
                  )}
                  <button
                    onClick={() => navigate(`/admin/documents/${doc.slug}`)}
                    className="bg-blue-500 text-white px-3 py-1 rounded text-sm hover:bg-blue-600"
                  >
                    Xem chi tiết
                  </button>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}

      {/* Pagination */}
      <div className="flex justify-center mt-4">
        <button
          disabled={pageNumber <= 1}
          onClick={() => onPageChange(pageNumber - 1)}
          className="px-3 py-1 mx-1 border rounded disabled:opacity-50"
        >
          &lt;
        </button>
        <span className="px-3 py-1">
          Trang {pageNumber}/{totalPage}
        </span>
        <button
          disabled={pageNumber >= totalPage}
          onClick={() => onPageChange(pageNumber + 1)}
          className="px-3 py-1 mx-1 border rounded disabled:opacity-50"
        >
          &gt;
        </button>
      </div>
    </div>
  );
}
