import api from "./apiClient";
export const dashboardApi = () => api.get("/DashBoard/statistics")