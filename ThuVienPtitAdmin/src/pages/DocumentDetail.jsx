import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { getDocumentApi } from "../services/documentService";
import { extractTextFromPdf } from "../services/pdfService";
import { summarizeText } from "../services/geminiService";
import { toast } from "react-toastify";

const DocumentDetail = () => {
  const { slug } = useParams();
  const [doc, setDoc] = useState(null);
  const [loading, setLoading] = useState(true);
  const [summary, setSummary] = useState("");
  const [summarizing, setSummarizing] = useState(false);

  useEffect(() => {
    const fetchDoc = async () => {
      try {
        const res = await getDocumentApi(slug);
        setDoc(res.docDto);
      } catch (error) {
        console.error("Lỗi khi tải tài liệu:", error);
      } finally {
        setLoading(false);
      }
    };
    fetchDoc();
  }, [slug]);

  const handleSummarize = async () => {
    if (!doc?.file_path?.toLowerCase().endsWith(".pdf")) {
      toast.warning("Chỉ hỗ trợ tóm tắt PDF!");
      return;
    }

    try {
      setSummarizing(true);
      toast.info("Đang đọc và tóm tắt 3 trang đầu...");

      // 1️⃣ Đọc 3 trang đầu PDF
      const text = await extractTextFromPdf(doc.file_path, 3);

      // 2️⃣ Gọi Gemini API để tóm tắt
      const result = await summarizeText(
        `Tóm tắt nội dung sau bằng tiếng Việt, ngắn gọn, dễ hiểu:\n\n${text}`
      );

      setSummary(result);
      toast.success("Đã tóm tắt xong!");
    } catch (err) {
      console.error(err);
      toast.error("Không thể tóm tắt tài liệu!");
    } finally {
      setSummarizing(false);
    }
  };

  if (loading)
    return (
      <p className="text-center mt-10 text-gray-500 animate-pulse">
        Đang tải tài liệu...
      </p>
    );

  if (!doc)
    return (
      <p className="text-center mt-10 text-red-500">
        Không tìm thấy tài liệu.
      </p>
    );

  const isPdf = doc.file_path?.toLowerCase().endsWith(".pdf");
  const isImage = /\.(jpg|jpeg|png|gif|webp|bmp)$/i.test(doc.file_path);

  return (
    <div className="max-w-5xl mx-auto px-6 py-8 bg-white rounded-xl shadow-lg mt-6 border border-gray-100">
      {/* Breadcrumb */}
      <div className="text-sm text-gray-500 mb-4">
        <span className="hover:underline cursor-pointer">Trang chủ</span> /{" "}
        <span className="hover:underline cursor-pointer">Tài liệu</span> /{" "}
        <span className="text-gray-700 font-medium">{doc.title}</span>
      </div>

      {/* Tiêu đề & mô tả */}
      <h1 className="text-3xl font-semibold text-gray-800 mb-1">{doc.title}</h1>
      <p className="text-sm text-gray-500 mb-4">
        Ngày tạo: {new Date(doc.created_at).toLocaleString("vi-VN")}
      </p>
      <p className="text-gray-700 leading-relaxed mb-6 whitespace-pre-line">
        {doc.description}
      </p>

      {/* Ảnh đại diện */}
      {doc.avt_document && (
        <div className="flex justify-center mb-6">
          <img
            src={doc.avt_document}
            alt="Ảnh đại diện tài liệu"
            className="max-h-[300px] object-contain rounded-lg border border-gray-200 shadow-sm"
          />
        </div>
      )}

      {/* Khung PDF / hình */}
      {doc.file_path && (
        <div className="border border-gray-300 rounded-lg overflow-hidden shadow mb-6 bg-gray-50">
          {isPdf ? (
            <iframe
              src={doc.file_path}
              title={doc.title}
              className="w-full h-[700px]"
            ></iframe>
          ) : isImage ? (
            <img
              src={doc.file_path}
              alt={doc.title}
              className="w-full h-auto max-h-[700px] object-contain"
            />
          ) : (
            <div className="p-6 text-center text-gray-600">
              Không thể xem trước tệp này.{" "}
              <a
                href={doc.file_path}
                target="_blank"
                rel="noopener noreferrer"
                className="text-blue-600 hover:underline"
              >
                Tải xuống tại đây
              </a>
              .
            </div>
          )}
        </div>
      )}

      {/* Nút tóm tắt PDF */}
      {isPdf && (
        <div className="text-center mb-6">
          <button
            onClick={handleSummarize}
            disabled={summarizing}
            className={`px-5 py-2.5 rounded-lg font-medium text-white transition 
              ${summarizing ? "bg-gray-400" : "bg-indigo-600 hover:bg-indigo-700"}`}
          >
            {summarizing ? "Đang tóm tắt..." : "✨ Tóm tắt 3 trang đầu"}
          </button>
        </div>
      )}

      {/* Hiển thị tóm tắt */}
      {summary && (
        <div className="bg-indigo-50 border border-indigo-200 p-5 rounded-lg shadow-sm mb-6">
          <h3 className="text-lg font-semibold text-indigo-700 mb-2">
            🧠 Tóm tắt nội dung:
          </h3>
          <p className="text-gray-800 whitespace-pre-line">{summary}</p>
        </div>
      )}

      {/* Tags */}
      {doc.tags && doc.tags.length > 0 && (
        <div className="flex flex-wrap gap-2 mb-6">
          {doc.tags.map((tag) => (
            <span
              key={tag.tag_id}
              className="px-3 py-1 text-sm bg-gradient-to-r from-blue-50 to-indigo-50 text-blue-700 rounded-md border border-blue-200 hover:bg-blue-100 transition"
            >
              #{tag.name}
            </span>
          ))}
        </div>
      )}

      {/* Thông tin khóa học */}
      <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-3 text-gray-700 border-t pt-4">
        <div>
          <p>
            <span className="font-semibold">Khóa học:</span>{" "}
            {doc.course?.name || "Không có"}
          </p>
          <p>
            <span className="font-semibold">Học kỳ:</span>{" "}
            {doc.course?.semester?.name || "Không rõ"}
          </p>
        </div>
        <div className="text-sm text-gray-500">
          <p>ID: {doc.document_id}</p>
        </div>
      </div>
    </div>
  );
};

export default DocumentDetail;
