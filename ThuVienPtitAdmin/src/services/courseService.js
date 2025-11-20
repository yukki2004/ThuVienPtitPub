import api from "./apiClient";

export const getAllCourseApi = () => api.get("/Course/get-courses")
export const getSemesterByCourseApi = (courseId) => api.get(`/Course/${courseId}/semester`)
export const createCourseApi = ({name,description,credits,semester_id,category_id}) => {
    return api.post("/Course/create-course", {name,description,credits,semester_id,category_id})
}
export const deleteCourseApi =(id) =>{
    return api.delete(`/Course/Delete-course/${id}`)
}
export const updateCourseApi = ({id, description, credits, category_id}) =>{
    return api.put(`/Course/update-course/${id}`, {description, credits, category_id})
}
export const getCategoryCourseApi = () => api.get("/Course/get-category-course")

export const getCourseListApi = (pageNumber = 1, semesterId = null) => {
  const params = { pageNumber };
  if (semesterId) params.semesterId = semesterId;
  return api.get("/Course/get-all-course-admin", { params });
};
export const getStatisticsCourseApi = (courseId) => {
  return api.get(`/Course/SatisticsCourse/${courseId}`);
};

export const getInfoCourseBySlugApi = (slug) => {
  return api.get(`/Course/InfoCourse/${slug}`);
};

export const getDocumentsByCourseApi = (courseId, pageNumber = 1, category = null, status = null) => {
  const params = { pageNumber };
  if (category) params.category = category;
  if (status !== null) params.status = status;
  return api.get(`/Course/get-document-admin/${courseId}`, { params });
};