import React, { useEffect, useState } from "react";
import ReactPaginate from "react-paginate";
import { useNavigate } from "react-router-dom";
import DocumentUserInfoCard from "../card/DocumentUserInfoCard";
import {
  getPubDocUserByAdminApi,
} from "../../services/userService";
import { deleteDocumentApi } from "../../services/documentService";
const PubUserDocList = ({ userId, refresh }) => {
  const [docs, setDocs] = useState([]);
  const [page, setPage] = useState(1);
  const [totalPage, setTotalPage] = useState(0);
  const navigate = useNavigate();
  useEffect(() => {
    fetchDocs(page);
  }, [page, refresh]);

  const fetchDocs = async (page = 1) => {
    const res = await getPubDocUserByAdminApi(userId, page);
    setDocs(res.documents);
    setPage(res.pageNumber);
    setTotalPage(res.totalPage);
  };

  const handleDelete = async (id) => {
    await deleteDocumentApi(id);
    fetchDocs();
  };
  const handlePageClick = (selected) => {
    fetchDocs(selected.selected + 1);
  };
  const handleView =(slug)=>{
    navigate(`/admin/documents/${slug}`);
  }

  return (
    <div className="bg-white p-4 rounded-2xl shadow space-y-4">
      <h2 className="text-lg font-semibold">Bài viết đã duyệt</h2>

      {docs.map((doc) => (
        <DocumentUserInfoCard
          key={doc.document_id}
          document={doc}
          actions={[
            { label: "Xem chi tiết", type: "view", onClick: () =>handleView(doc.slug) },
            { label: "Xóa", type: "reject", onClick: () => handleDelete(doc.document_id) },
          ]}
        />
      ))}

      <ReactPaginate
        className="flex justify-center gap-2 mt-4"
        breakLabel="..."
        nextLabel=">"
        onPageChange={handlePageClick}
        pageRangeDisplayed={3}
        pageCount={totalPage}
        previousLabel="<"
        activeClassName="bg-blue-500 text-white px-3 py-1 rounded"
        pageClassName="px-3 py-1 border rounded"
      />
    </div>
  );
};

export default PubUserDocList;
