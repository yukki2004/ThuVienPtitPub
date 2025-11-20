import axios  from "axios";
import { setTokens, getAccessToken, getRefreshToken, removeTokens } from "../untils/tokenUtils";
import { refreshTokenApi } from "./userService.js";
import { getTokenExpiry } from "../untils/jwt.js";
const api = axios.create({
  baseURL: "https://localhost:7188/api",
  timeout: 80000,
});
let isRefreshing = false;
let subscribers = [];
const subscribeTokenRefresh = (cb) => subscribers.push(cb);
const notifySubscribers = (token) => {
  subscribers.forEach((cb) => cb(token)); 
  subscribers = [];
};
const refreshToken = async () => {
  const refresh = getRefreshToken();
  if (!refresh){
    window.location.href = "/login";
    throw new Error("No refresh token");
  }
  try {
    const newTokens = (await refreshTokenApi(refresh)).data;
    setTokens(newTokens.accessToken, newTokens.refreshToken);
    notifySubscribers(newTokens.accessToken);
    return newTokens.accessToken;
  } catch (err) {
    window.location.href = "/login";
    removeTokens();
    throw err;
  }
};
api.interceptors.request.use(async (config) => {
  let token = getAccessToken();
  if (!token) return config;
  const exp = getTokenExpiry(token);
  const now = Math.floor(Date.now() / 1000);
  if (exp - now < 20) {
    if (!isRefreshing) {
      isRefreshing = true;
      try {
        token = await refreshToken();
      } finally {
        isRefreshing = false;
      }
    } else {
    
      token = await new Promise((resolve) => subscribeTokenRefresh(resolve));
    }
  }
  config.headers.Authorization = `Bearer ${token}`;
  return config;
});

api.interceptors.response.use(
  (res) => res.data,
  async (error) => {
    const originalRequest = error.config;
    if (error.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;
      if (!isRefreshing) {
        isRefreshing = true;
        try {
          const newToken = await refreshToken();
          originalRequest.headers.Authorization = `Bearer ${newToken}`;
          return api(originalRequest);
        } finally {
          isRefreshing = false;
        }
      } else {
        const newToken = await new Promise((resolve) => subscribeTokenRefresh(resolve));
        originalRequest.headers.Authorization = `Bearer ${newToken}`;
        return api(originalRequest);
      }
    }
    if(error.response?.status === 401){
        window.location.href = "/login";
        removeTokens();
    }

    return Promise.reject(error);
  }
);
export default api;