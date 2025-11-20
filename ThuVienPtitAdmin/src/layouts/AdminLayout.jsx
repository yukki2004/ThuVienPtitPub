import React from "react";
import { Outlet } from "react-router-dom";
import Header from "../components/layouts/HeaderBar.jsx";
import SlideBar from "../components/layouts/SlideBar.jsx";
const AdminLayout = () => {
  return (
    <div className="flex h-screen flex-col bg-gray-100">
        <Header />
        <div className="flex flex-1 overflow-hidden">
            <SlideBar />
            <main className="flex-1 overflow-y-auto p-6">
                <Outlet />
            </main>
        </div>
    </div>
  );
}
export default AdminLayout