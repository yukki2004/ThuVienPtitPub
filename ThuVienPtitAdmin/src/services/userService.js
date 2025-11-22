import api from "./apiClient.js"
import axios from "axios";
export const loginApi = (login, password) => api.post("/User/login", {login, password});
export const logoutApi = (refresh_token) => api.post("/User/logout", {refresh_token});
export const refreshTokenApi = (refresh_token) => axios.post("http://api.thuvienptit.com/api/User/refresh-token",{refresh_token});
export const getMe = () => api.get("User/GetUser");
export const getUserByAdminApi = (userid) =>{
    return api.get(`/User/get-user-by-admin/${userid}`);
}
export const getPubDocUserByAdminApi = (userid,pageNumber) =>{
    return api.get(`/User/GetPubDocByAdmin/${userid}`, {
        params: {
            pageNumber,
        }
    });
}
export const getPenDocUserByAdminApi = (userid,pageNumber) =>{
    return api.get(`/User/GetPenDocByAdmin/${userid}`, {
        params: {
            pageNumber,
        }
    });
}
export const deleteUserByAdminApi = (userid) =>{
    return api.delete(`/User/DeleteUser/${userid}`);
}
export const getAllUserByAdminApi = (pageNumber = 1) => {
    return api.get("/User/Get-All-User", {
        params: {
            pageNumber,
        }
    })
}
