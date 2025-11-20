// src/utils/storage.js
export const setItem = (key, value) => localStorage.setItem(key, JSON.stringify(value));
export const getItem = (key) => {
  const item = localStorage.getItem(key);
  return item ? JSON.parse(item) : null;
};
export const removeItem = (key) => localStorage.removeItem(key);

export const setTokens = (access, refresh) => {
  setItem("accessToken", access);
  setItem("refreshToken", refresh);
};
export const getAccessToken = () => getItem("accessToken");
export const getRefreshToken = () => getItem("refreshToken");
export const removeTokens = () => {
  removeItem("accessToken");
  removeItem("refreshToken");
};

