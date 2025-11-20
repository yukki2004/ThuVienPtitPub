export const decodeToken = (token) => {
  try {
    return JSON.parse(atob(token.split(".")[1]));
  } catch {
    return null;
  }
};
export const isTokenExpired = (token) => {
  const payload = decodeToken(token);
  if (!payload) return true;
  const now = Math.floor(Date.now() / 1000);
  return payload.exp <= now;
};

export const getTokenExpiry = (token) => {
  const payload = decodeToken(token);
  return payload ? payload.exp : 0;
};

export const getRoleFromToken = (token) => {
  const payload = decodeToken(token);
  return payload?.role || null;
};
