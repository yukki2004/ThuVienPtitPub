import React, { useEffect, useState } from "react";
import ReactPaginate from "react-paginate";
import {
  getAllUserByAdminApi,
  deleteUserByAdminApi,
} from "../services/userService";
import UserList from "../components/List/UserList";

const UserListPage = () => {
  const [users, setUsers] = useState([]);
  const [pageNumber, setPageNumber] = useState(1);
  const [totalPage, setTotalPage] = useState(1);
  const [loading, setLoading] = useState(false);

  const fetchUsers = async (page = 1) => {
    try {
      setLoading(true);
      const res = await getAllUserByAdminApi(page);
      setUsers(res.users);
      setTotalPage(res.totalPage);
    } catch (error) {
      console.error("Lỗi khi tải danh sách người dùng:", error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchUsers(pageNumber);
  }, [pageNumber]);

  const handleDelete = async (userId) => {
    if (!window.confirm("Bạn có chắc chắn muốn xóa người dùng này không?")) return;
    try {
      await deleteUserByAdminApi(userId);
      fetchUsers(pageNumber);
    } catch (error) {
      console.error("Xóa người dùng thất bại:", error);
    }
  };

  const handlePageClick = (data) => {
    const selectedPage = data.selected + 1;
    setPageNumber(selectedPage);
  };

  return (
    <div className="p-6">
      <h1 className="text-2xl font-bold mb-4">Quản lý người dùng</h1>

      {loading ? (
        <p className="text-gray-500 text-center">Đang tải...</p>
      ) : (
        <UserList users={users} onDelete={handleDelete} />
      )}

      <div className="flex justify-center mt-8">
        <ReactPaginate
          previousLabel="← Trước"
          nextLabel="Sau →"
          breakLabel="..."
          onPageChange={handlePageClick}
          pageCount={totalPage}
          marginPagesDisplayed={1}
          pageRangeDisplayed={3}
          containerClassName="flex items-center gap-2"
          pageClassName="px-3 py-1 border border-gray-300 rounded-lg cursor-pointer hover:bg-blue-100"
          activeClassName="bg-blue-500 text-white"
          previousClassName="px-3 py-1 border border-gray-300 rounded-lg hover:bg-gray-100"
          nextClassName="px-3 py-1 border border-gray-300 rounded-lg hover:bg-gray-100"
          disabledClassName="opacity-50 cursor-not-allowed"
        />
      </div>
    </div>
  );
};

export default UserListPage;
