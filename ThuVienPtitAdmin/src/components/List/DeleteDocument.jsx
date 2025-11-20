import React, { useEffect, useState } from "react";
import ReactPaginate from "react-paginate";
import DelDocumentCard from "../card/DelDocumentCard";
import {
  getDeleteDocumentApi,
  restoreDocumentApi,
  clearDocumentApi,
} from "../../services/documentService";

export default function DeletedDocument({ reloadFlag }) {
  const [documents, setDocuments] = useState([]);
  const [pageCount, setPageCount] = useState(0);
  const [currentPage, setCurrentPage] = useState(1);

  const fetchDocuments = async (page = 1) => {
    const res = await getDeleteDocumentApi(page);
    setDocuments(res.docs);
    setPageCount(res.toTalPages);
    setCurrentPage(res.pageNumber);
  };

  useEffect(() => {
    fetchDocuments(currentPage);
  }, [reloadFlag]);

  const handleRestore = async (documentId) => {
    if (!window.confirm("Bạn có muốn khôi phục tài liệu này không?")) return;
    await restoreDocumentApi(documentId);
    fetchDocuments(currentPage);
  };

  const handleClear = async (documentId) => {
    if (!window.confirm("Bạn có chắc muốn xóa vĩnh viễn tài liệu này không?")) return;
    await clearDocumentApi(documentId);
    fetchDocuments(currentPage);
  };

  const handlePageClick = (selected) => {
    fetchDocuments(selected.selected + 1);
  };

  return (
    <div className="space-y-4">
      {documents.map((doc) => (
        <DelDocumentCard
          key={doc.document_id}
          document={doc}
          mode="deleted"
          onRestore={handleRestore}
          onClear={handleClear}
        />
      ))}

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
            pageClassName={"px-3 py-1 border rounded hover:bg-gray-100 cursor-pointer"}
            previousClassName={"px-3 py-1 border rounded hover:bg-gray-100 cursor-pointer"}
            nextClassName={"px-3 py-1 border rounded hover:bg-gray-100 cursor-pointer"}
            activeClassName={"bg-blue-500 text-white border-blue-500"}
          />
        </div>
      )}
    </div>
  );
}
