import { useEffect, useState } from "react";
import { getAllTagApi } from "../../services/tagsService";
import { getAllSemesterApi, getCourseBySemesterApi } from "../../services/semesterService";
import { getAllCourseApi } from "../../services/courseService";
import { uploadDocumentApi } from "../../services/documentService";

const UploadDocForm = ({ onUploadSuccess }) => {
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [avtDocument, setAvtDocument] = useState(null);
  const [fileDocument, setFileDocument] = useState(null);
  const [avtPreview, setAvtPreview] = useState(null);
  const [tags, setTags] = useState([]);
  const [selectedTags, setSelectedTags] = useState([]);
  const [courses, setCourses] = useState([]);
  const [semesters, setSemesters] = useState([]);
  const [selectedCourse, setSelectedCourse] = useState("");
  const [selectedSemester, setSelectedSemester] = useState("");
  const [category, setCategory] = useState("");

  useEffect(() => {
    const fetchData = async () => {
      try {
        const tagsRes = await getAllTagApi();
        setTags(tagsRes || []);

        const semestersRes = await getAllSemesterApi();
        setSemesters(semestersRes || []);

        const coursesRes = await getAllCourseApi();
        setCourses(coursesRes || []);
      } catch (err) {
        console.error("Lỗi khi load dữ liệu:", err);
      }
    };
    fetchData();
  }, []);
  useEffect(() => {
    const fetchCourses = async () => {
      if (!selectedSemester) return;
      try {
        const res = await getCourseBySemesterApi(selectedSemester);
        if (res?.courses) {
          setCourses(res.courses);
        }
      } catch (err) {
        console.error("Lỗi khi load môn học theo học kỳ:", err);
      }
    };
    fetchCourses();
  }, [selectedSemester]);
  const handleAvtChange = (e) => {
    const file = e.target.files[0];
    setAvtDocument(file);
    if (file) {
      const previewUrl = URL.createObjectURL(file);
      setAvtPreview(previewUrl);
    } else {
      setAvtPreview(null);
    }
  };

  const toggleTag = (id) => {
    setSelectedTags((prev) =>
      prev.includes(id) ? prev.filter((t) => t !== id) : [...prev, id]
    );
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await uploadDocumentApi({
        title,
        description,
        avtDocument,
        fileDocument,
        course_id: selectedCourse,
        semester_id: selectedSemester,
        category,
        tag_ids: selectedTags,
      });

      onUploadSuccess();
      setTitle("");
      setDescription("");
      setAvtDocument(null);
      setFileDocument(null);
      setAvtPreview(null);
      setSelectedTags([]);
      setSelectedCourse("");
      setSelectedSemester("");
      setCategory("");
    } catch (err) {
      console.error("Lỗi upload:", err);
    }
  };

  return (
    <form
      onSubmit={handleSubmit}
      className="p-6 border rounded-2xl shadow-lg bg-white space-y-5"
    >
      <h2 className="text-xl font-semibold text-gray-700">📄 Upload Document</h2>

      <input
        type="text"
        placeholder="Tiêu đề tài liệu..."
        value={title}
        onChange={(e) => setTitle(e.target.value)}
        className="w-full px-3 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-400"
      />

      <textarea
        placeholder="Mô tả ngắn gọn về tài liệu..."
        value={description}
        onChange={(e) => setDescription(e.target.value)}
        className="w-full px-3 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-400"
      />

      <div className="flex flex-col sm:flex-row gap-6 items-start">
        <div className="flex flex-col items-center border rounded-xl p-4 bg-gray-50 shadow-sm hover:shadow-md transition">
          <p className="font-medium text-gray-700 mb-2">Ảnh đại diện</p>
          <label className="cursor-pointer bg-blue-500 text-white px-3 py-1 rounded-md hover:bg-blue-600 transition">
            Chọn ảnh
            <input
              type="file"
              accept="image/*"
              className="hidden"
              onChange={handleAvtChange}
            />
          </label>

          {avtPreview && (
            <img
              src={avtPreview}
              alt="preview"
              className="mt-3 w-32 h-32 object-cover rounded-lg border"
            />
          )}
        </div>

        <div className="flex flex-col border rounded-xl p-4 bg-gray-50 shadow-sm hover:shadow-md transition w-full sm:w-2/3">
          <p className="font-medium text-gray-700 mb-2">File tài liệu</p>
          <label className="cursor-pointer bg-green-500 text-white px-3 py-1 rounded-md hover:bg-green-600 transition w-fit">
            Chọn file
            <input
              type="file"
              accept=".pdf,.doc,.docx,.ppt,.pptx"
              className="hidden"
              onChange={(e) => setFileDocument(e.target.files[0])}
            />
          </label>

          {fileDocument && (
            <p className="mt-3 text-sm text-gray-600">
              📘 Đã chọn:{" "}
              <span className="font-medium text-gray-800">{fileDocument.name}</span>
            </p>
          )}
        </div>
      </div>

      <div className="flex flex-wrap gap-2 mt-2">
        {tags.map((tag) => (
          <button
            key={tag.tag_id}
            type="button"
            onClick={() => toggleTag(tag.tag_id)}
            className={`px-3 py-1 border rounded-lg text-sm transition ${
              selectedTags.includes(tag.tag_id)
                ? "bg-blue-500 text-white border-blue-500"
                : "bg-gray-100 border-gray-300 hover:bg-gray-200"
            }`}
          >
            {tag.name}
          </button>
        ))}
      </div>

      <div className="flex flex-col sm:flex-row gap-4">
        <select
          value={selectedSemester}
          onChange={(e) => setSelectedSemester(e.target.value)}
          className="flex-1 px-3 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-400"
        >
          <option value="">Chọn học kỳ</option>
          {semesters.map((s) => (
            <option key={s.semester_id} value={s.semester_id}>
              {s.name}
            </option>
          ))}
        </select>

        <select
          value={selectedCourse}
          onChange={(e) => setSelectedCourse(e.target.value)}
          className="flex-1 px-3 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-400"
        >
          <option value="">Chọn môn học</option>
          {courses.map((c) => (
            <option key={c.course_id} value={c.course_id}>
              {c.name}
            </option>
          ))}
        </select>
      </div>

      <select
        value={category}
        onChange={(e) => setCategory(e.target.value)}
        className="w-full px-3 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-400"
      >
        <option value="">Chọn loại tài liệu</option>
        <option value="Giáo trình">Giáo trình</option>
        <option value="Slide">Slide</option>
        <option value="Đề thi">Đề thi</option>
        <option value="Tài liệu khác">Tài liệu khác</option>
      </select>

      <button
        type="submit"
        className="w-full bg-blue-600 hover:bg-blue-700 text-white font-semibold py-2 px-4 rounded-lg transition"
      >
        📤 Upload tài liệu
      </button>
    </form>
  );
};

export default UploadDocForm;
