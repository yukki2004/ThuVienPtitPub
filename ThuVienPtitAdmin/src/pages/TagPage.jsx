import React, { useEffect, useState } from "react";
import { getAllTagAdminApi } from "../services/tagsService";
import FormUploadTag from "../components/Form/FormUploadTag";
import TagList from "../components/List/TagList";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

const TagPage = () => {
  const [tags, setTags] = useState([]);
  const [pageNumber, setPageNumber] = useState(1);
  const [totalPage, setTotalPage] = useState(1);

  const fetchTags = async (page = 1) => {
    try {
      const res = await getAllTagAdminApi(page);
      setTags(res.tags || []);
      setPageNumber(res.pageNumber);
      setTotalPage(res.totalPage);
    } catch (err) {
      console.error("Lỗi load danh sách tag", err);
    }
  };

  useEffect(() => {
    fetchTags(pageNumber);
  }, [pageNumber]);

  return (
    <div className="max-w-5xl mx-auto p-6">
      <ToastContainer />
      <FormUploadTag onUploadSuccess={() => fetchTags(pageNumber)} />
      <TagList
        tags={tags}
        pageNumber={pageNumber}
        totalPage={totalPage}
        onPageChange={setPageNumber}
        onRefresh={() => fetchTags(pageNumber)}
      />
    </div>
  );
};

export default TagPage;
