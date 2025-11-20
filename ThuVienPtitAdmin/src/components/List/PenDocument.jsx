import React, { useEffect, useState } from "react";
import ReactPaginate from "react-paginate";
import PenDocumentCard from "../card/PenDocumentCard";
import {
  getPendingDocumentApi,
  approveDocumentApi,
  rejectDocumentApi,
} from "../../services/documentService";
import { useNavigate } from "react-router-dom";

export default function PenDocument() {
  const [documents, setDocuments] = useState([]);
  const [pageCount, setPageCount] = useState(0);
  const [currentPage, setCurrentPage] = useState(1);
  const Navigate = useNavigate();
  const fetchDocuments = async (page = 1) => {
    try {
      const res = await getPendingDocumentApi(page);
      setDocuments(res.docs);
      setPageCount(res.toTalPages);
      setCurrentPage(res.pageNumber);
    } catch (error) {
      console.error("Lỗi khi tải danh sách pending:", error);
    }
  };

  useEffect(() => {
    fetchDocuments(currentPage);
  }, []);

  const handleApprove = async (documentId) => {
    if (!window.confirm("Duyệt tài liệu này?")) return;
    await approveDocumentApi(documentId);
    fetchDocuments(currentPage);
  };

  const handleReject = async (documentId) => {
    if (!window.confirm("Từ chối tài liệu này?")) return;
    await rejectDocumentApi(documentId);
    fetchDocuments(currentPage);
  };

  const handleView = (doc) => {
    Navigate(`/admin/documents/${doc.slug}`);
  };

  const handlePageClick = (selected) => {
    fetchDocuments(selected.selected + 1);
  };

  return (
    <div className="space-y-4">
      {documents.length === 0 ? (
        <p className="text-center text-gray-500 mt-10">
          Không có tài liệu chờ duyệt nào.
        </p>
      ) : (
        documents.map((doc) => (
          <PenDocumentCard
            key={doc.document_id}
            document={doc}
            onApprove={handleApprove}
            onReject={handleReject}
            onView={handleView}
          />
        ))
      )}

      {pageCount > 1 && (
        <div className="flex justify-center mt-6">
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
