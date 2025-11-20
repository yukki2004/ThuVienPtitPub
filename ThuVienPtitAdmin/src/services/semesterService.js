import api from "./apiClient";
export const getAllSemesterApi = () => api.get("/Semester/get-all-semesters")
export const getCourseBySemesterApi = (id) => api.get(`/Semester/${id}/courses`)