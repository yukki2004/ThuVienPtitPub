import React, { useEffect, useState } from "react";
import ReactPaginate from "react-paginate";
import DocumentUserInfoCard from "../card/DocumentUserInfoCard";
import {
  approveDocumentApi,
  rejectDocumentApi,
} from "../../services/documentService";
import { getPenDocUserByAdminApi } from "../../services/userService";
import { useNavigate } from "react-router-dom";

export default function PenUserDocList({ userId, onRefresh }) {
  const [documents, setDocuments] = useState([]);
  const [pageCount, setPageCount] = useState(0);
  const [currentPage, setCurrentPage] = useState(1);
  const navigate = useNavigate();

  const fetchDocuments = async (page = 1) => {
    try {
      const res = await getPenDocUserByAdminApi(userId, page);

      setDocuments(res.documents || []);
      setPageCount(res.totalPage || 0);
      setCurrentPage(res.pageNumber || 1);
    } catch (error) {
      console.error("❌ Lỗi khi tải danh sách tài liệu đang chờ duyệt:", error);
    }
  };

  useEffect(() => {
    fetchDocuments(1);
  }, [userId]);

  const handleApprove = async (id) => {
    if (!window.confirm("Bạn có chắc muốn duyệt tài liệu này?")) return;
    await approveDocumentApi(id);
    await fetchDocuments(currentPage);
    onRefresh?.();
  };

  const handleReject = async (id) => {
    if (!window.confirm("Bạn có chắc muốn từ chối tài liệu này?")) return;
    await rejectDocumentApi(id);
    await fetchDocuments(currentPage);
  };

  const handlePageClick = (selected) => {
    fetchDocuments(selected.selected + 1);
  };

  const handleView = (slug) => {
    navigate(`/admin/documents/${slug}`);
  };

  return (
    <div className="bg-white p-4 rounded-2xl shadow space-y-4">
      <h2 className="text-lg font-semibold">Bài viết đang chờ duyệt</h2>

      {documents.length === 0 ? (
        <p className="text-gray-500 italic">
          Không có bài viết nào đang chờ duyệt.
        </p>
      ) : (
        documents.map((doc) => (
          <DocumentUserInfoCard
            key={doc.document_id}
            document={doc}
            actions={[
              {
                label: "Xem chi tiết",
                type: "view",
                onClick: () => handleView(doc.slug),
              },
              {
                label: "Từ chối",
                type: "reject",
                onClick: () => handleReject(doc.document_id),
              },
              {
                label: "Duyệt",
                type: "approve",
                onClick: () => handleApprove(doc.document_id),
              },
            ]}
          />
        ))
      )}

      {pageCount > 1 && (
        <div className="flex justify-center mt-4">
          <ReactPaginate
            previousLabel={"‹"}
            nextLabel={"›"}
            breakLabel={"..."}
            pageCount={pageCount}
            marginPagesDisplayed={1}
            pageRangeDisplayed={3}
            onPageChange={handlePageClick}
            containerClassName={"flex gap-2"}
            pageClassName={
              "px-3 py-1 border rounded hover:bg-gray-100 cursor-pointer"
            }
            previousClassName={
              "px-3 py-1 border rounded hover:bg-gray-100 cursor-pointer"
            }
            nextClassName={
              "px-3 py-1 border rounded hover:bg-gray-100 cursor-pointer"
            }
            activeClassName={"bg-blue-500 text-white border-blue-500"}
          />
        </div>
      )}
    </div>
  );
}
