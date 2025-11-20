import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import CourseInfoCard from "../components/card/CourseInfoCard";
import CourseStats from "../components/stat/CourseStats";
import DocumentFilters from "../components/filter/DocumentFilters";
import DocumentTable from "../components/table/DocumentTable";
import Pagination from "../components/page/Pagination";
import {
  getInfoCourseBySlugApi,
  getStatisticsCourseApi,
  getDocumentsByCourseApi,
} from "../services/courseService";

// import thêm API xử lý document
import {
  approveDocumentApi,
  rejectDocumentApi,
  deleteDocumentApi,
  restoreDocumentApi,
  clearDocumentApi,
} from "../services/documentService"; // bạn tạo file documentService.js chứa các API này

const SubjectDetail = () => {
  const { slug } = useParams(); 
  const [course, setCourse] = useState(null);
  const [stats, setStats] = useState(null);
  const [documents, setDocuments] = useState([]);
  const [pageNumber, setPageNumber] = useState(1);
  const [totalItem, setTotalItem] = useState(0);
  const [categoryFilter, setCategoryFilter] = useState(null);
  const [statusFilter, setStatusFilter] = useState(null);
  const [loading, setLoading] = useState(false);

  const fetchCourseInfo = async () => {
    try {
      const res = await getInfoCourseBySlugApi(slug);
      setCourse(res);
    } catch (err) {
      toast.error("❌ Lỗi khi tải thông tin course");
    }
  };

  const fetchStats = async () => {
    try {
      const res = await getStatisticsCourseApi(course.courseId);
      setStats(res);
    } catch (err) {
      toast.error("❌ Lỗi khi tải thống kê course");
    }
  };

  const fetchDocuments = async () => {
    if (!course) return;
    try {
      setLoading(true);
      const res = await getDocumentsByCourseApi(
        course.courseId,
        pageNumber,
        categoryFilter,
        statusFilter
      );
      setDocuments(res.items);
      setTotalItem(res.totalItem);
    } catch (err) {
      toast.error("❌ Lỗi khi tải documents");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    if (slug) fetchCourseInfo();
  }, [slug]);

  useEffect(() => {
    if (course) {
      fetchStats();
      fetchDocuments();
    }
  }, [course, pageNumber, categoryFilter, statusFilter]);

  const handleApprove = async (doc) => {
    try {
      await approveDocumentApi(doc.documentId);
      toast.success("✅ Approved document");
      fetchDocuments();
      fetchStats();
    } catch (err) {
      toast.error("❌ Lỗi khi approve document");
    }
  };

  const handleReject = async (doc) => {
    try {
      await rejectDocumentApi(doc.documentId);
      toast.success("✅ Rejected document");
      fetchDocuments();
      fetchStats();
    } catch (err) {
      toast.error("❌ Lỗi khi reject document");
    }
  };

  const handleDelete = async (doc) => {
    try {
      await deleteDocumentApi(doc.documentId);
      toast.success("✅ Deleted document");
      fetchDocuments();
      fetchStats();
    } catch (err) {
      toast.error("❌ Lỗi khi delete document");
    }
  };

  const handleRestore = async (doc) => {
    try {
      await restoreDocumentApi(doc.documentId);
      toast.success("✅ Restored document");
      fetchDocuments();
      fetchStats();
    } catch (err) {
      toast.error("❌ Lỗi khi restore document");
    }
  };

  const handleClear = async (doc) => {
    try {
      await clearDocumentApi(doc.documentId);
      toast.success("✅ Cleared document");
      fetchDocuments();
      fetchStats();
    } catch (err) {
      toast.error("❌ Lỗi khi clear document");
    }
  };

  return (
    <div className="p-6 max-w-6xl mx-auto">
      <button onClick={() => window.history.back()} className="mb-4 text-blue-500">
        &lt; Back
      </button>

      {course && <CourseInfoCard course={course} />}
      {stats && <CourseStats stats={stats} />}

      <DocumentFilters
        category={categoryFilter}
        status={statusFilter}
        onCategoryChange={setCategoryFilter}
        onStatusChange={setStatusFilter}
      />

      <DocumentTable
        documents={documents}
        loading={loading}
        onApprove={handleApprove}
        onReject={handleReject}
        onDelete={handleDelete}
        onRestore={handleRestore}
        onClear={handleClear}
      />

      <Pagination
        totalItem={totalItem}
        pageSize={10}
        currentPage={pageNumber}
        onPageChange={setPageNumber}
      />

      <ToastContainer position="bottom-right" autoClose={3000} />
    </div>
  );
};

export default SubjectDetail;
