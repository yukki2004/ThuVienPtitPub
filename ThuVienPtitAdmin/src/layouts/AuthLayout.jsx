import { Outlet } from "react-router-dom";

const AuthLayout = () => {
  
  return (
    <div className="flex h-screen items-center justify-center bg-gradient-to-br from-red-600 to-red-400">
      <div className="w-full max-w-md bg-white p-8 rounded-2xl shadow-xl border border-gray-200">
        <Outlet />
      </div>
    </div>
  );
};

export default AuthLayout;
