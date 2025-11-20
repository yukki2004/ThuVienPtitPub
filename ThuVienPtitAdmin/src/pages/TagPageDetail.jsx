import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { getStatTagApi, getDocumentTagFilterApi } from "../services/tagsService";
import TagStatCard from "../components/card/TagStatCard";
import TagChart from "../components/stat/TagChart";
import TagDocumentList from "../components/List/TagDocumentList";
import { toast } from "react-toastify";

export default function TagPageDetail() {
  const { tagId } = useParams();
  const [tagStat, setTagStat] = useState(null);
  const [docs, setDocs] = useState([]);
  const [pageNumber, setPageNumber] = useState(1);
  const [totalPage, setTotalPage] = useState(1);
  const [status, setStatus] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    let isMounted = true; // ✅ flag kiểm soát

    const fetchStat = async () => {
      try {
        const res = await getStatTagApi(tagId);
        if (isMounted) setTagStat(res);
      } catch {
        if (isMounted) toast.error("Không thể tải thống kê tag!");
      }
    };

    const fetchDocs = async (page = 1, st = status) => {
      try {
        const res = await getDocumentTagFilterApi(tagId, page, st);
        if (isMounted) {
          setDocs(res.documents);
          setTotalPage(res.totalPages);
          setPageNumber(res.pageNumber);
        }
      } catch {
        if (isMounted) toast.error("Không thể tải danh sách tài liệu!");
      }
    };

    fetchStat();
    fetchDocs(1, null);

    return () => {
      isMounted = false; // ✅ cleanup khi unmount
    };
  }, [tagId]);

  const handleFilterChange = (val) => {
    setStatus(val);
    // Không cần await ở đây vì user có thể bấm back ngay sau đó
    getDocumentTagFilterApi(tagId, 1, val).then(res => {
      setDocs(res.documents);
      setTotalPage(res.totalPages);
      setPageNumber(res.pageNumber);
    }).catch(() => toast.error("Không thể tải danh sách tài liệu!"));
  };

  const handlePageChange = (page) => {
    getDocumentTagFilterApi(tagId, page, status).then(res => {
      setDocs(res.documents);
      setTotalPage(res.totalPages);
      setPageNumber(res.pageNumber);
    }).catch(() => toast.error("Không thể tải danh sách tài liệu!"));
  };

  return (
    <div className="p-6 space-y-6">
      <button
        onClick={() => navigate(-1)}
        className="text-blue-600 hover:underline mb-4"
      >
        ← Quay lại danh sách tag
      </button>

      {tagStat && (
        <>
          <h1 className="text-2xl font-semibold">
            Thống kê Tag:{" "}
            <span className="text-blue-600">{tagStat.tag.name}</span>
          </h1>
          <TagStatCard data={tagStat} />
          <TagChart data={tagStat} />
        </>
      )}

      <div className="mt-8">
        <div className="flex items-center mb-4 space-x-3">
          <label className="font-medium">Lọc trạng thái:</label>
          <select
            className="border border-gray-300 rounded px-3 py-2 text-sm"
            onChange={(e) =>
              handleFilterChange(
                e.target.value === "" ? null : Number(e.target.value)
              )
            }
          >
            <option value="">Tất cả</option>
            <option value="1">Đã duyệt</option>
            <option value="0">Chờ duyệt</option>
            <option value="3">Đã xóa</option>
          </select>
        </div>

        <TagDocumentList
          documents={docs}
          pageNumber={pageNumber}
          totalPage={totalPage}
          onPageChange={handlePageChange}
          onRefresh={() => handlePageChange(pageNumber)}
        />
      </div>
    </div>
  );
}
