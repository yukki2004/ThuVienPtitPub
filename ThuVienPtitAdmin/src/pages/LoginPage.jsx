import React from "react";
import LoginForm from "../components/Form/LoginForm"
const LoginPage = () => {
  return (
    <div className="w-full max-w-md mx-auto mt-20 p-6 bg-white shadow-lg rounded-lg">
      <h1 className="text-3xl font-bold text-red-600 mb-6 text-center">
        Đăng nhập hệ thống
      </h1>
      <LoginForm />
    </div>
  );
};

export default LoginPage;
