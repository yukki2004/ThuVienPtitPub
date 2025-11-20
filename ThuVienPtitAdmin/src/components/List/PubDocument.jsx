import React, { useEffect, useState } from "react";
import ReactPaginate from "react-paginate";
import DocumentCard from "../card/DocumentCard";
import {  deleteDocumentApi, getPubLishDocumentApi } from "../../services/documentService";
import { getPubDocUserByAdminApi } from "../../services/userService";
export default function PubDocument({ reloadFlag }) {
  const [documents, setDocuments] = useState([]);
  const [pageCount, setPageCount] = useState(0);
  const [currentPage, setCurrentPage] = useState(1);

  const fetchDocuments = async (page = 1) => {
    const res = await getPubLishDocumentApi(page);
    setDocuments(res.docs);
    setPageCount(res.toTalPages);
    setCurrentPage(res.pageNumber);
  };

  useEffect(() => {
    fetchDocuments(currentPage);
  }, [reloadFlag]);

  const handleDelete = async (documentId) => {
    if (!window.confirm("Bạn có chắc muốn xóa tài liệu này?")) return;
    await deleteDocumentApi(documentId);
    fetchDocuments(currentPage);
  };

  const handlePageClick = (selected) => {
    fetchDocuments(selected.selected + 1);
  };

  return (
    <div className="space-y-4">
      {documents.map((doc) => (
        <DocumentCard key={doc.document_id} document={doc} onDelete={handleDelete} />
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
