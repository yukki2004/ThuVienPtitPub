// Sidebar.jsx
import { 
  FaBookOpen, 
  FaFolder, 
  FaCheckSquare,
  FaTrash,
  FaUsers,
  FaTags
} from "react-icons/fa";
import { NavLink } from "react-router-dom";

const baseLinkStyle = "flex items-center gap-3 p-2 rounded transition-colors";
const activeLinkStyle = "bg-gray-700 text-white";
const inactiveLinkStyle = "hover:bg-gray-700 hover:text-white";

const Sidebar = () => {
  return (
    <div className="w-64 h-screen bg-gray-800 text-white p-4">
      <h2 className="text-lg font-semibold mb-6">Chức năng</h2>
      <nav className="space-y-4">
        <NavLink
          to="/admin/subjects" 
          className={({ isActive }) => 
            `${baseLinkStyle} ${isActive ? activeLinkStyle : inactiveLinkStyle}`
          }
        >
          <FaBookOpen className="text-pink-400" />
          <span>Quản lý môn học</span>
        </NavLink>
        
        <NavLink
          to="/admin/documents"
          className={({ isActive }) => 
            `${baseLinkStyle} ${isActive ? activeLinkStyle : inactiveLinkStyle}`
          }
        >
          <FaFolder className="text-yellow-400" />
          <span>Quản lý tài liệu</span>
        </NavLink>

        {/* ✅ Thêm mục Quản lý Tag */}
        <NavLink
          to="/admin/tag"
          className={({ isActive }) =>
            `${baseLinkStyle} ${isActive ? activeLinkStyle : inactiveLinkStyle}`
          }
        >
          <FaTags className="text-purple-400" />
          <span>Quản lý Tag</span>
        </NavLink>

        <NavLink
          to="/admin/pending-documents"
          className={({ isActive }) => 
            `${baseLinkStyle} ${isActive ? activeLinkStyle : inactiveLinkStyle}`
          }
        >
          <FaCheckSquare className="text-green-400" />
          <span>Duyệt tài liệu</span>
        </NavLink>
        
        <NavLink
          to="/admin/trash-documents"
          className={({ isActive }) => 
            `${baseLinkStyle} ${isActive ? activeLinkStyle : inactiveLinkStyle}`
          }
        >
          <FaTrash className="text-red-400" />
          <span>Xóa tài liệu</span>
        </NavLink>
        
        <NavLink
          to="/admin/users"
          className={({ isActive }) => 
            `${baseLinkStyle} ${isActive ? activeLinkStyle : inactiveLinkStyle}`
          }
        >
          <FaUsers className="text-blue-400" />
          <span>Quản lý User</span>
        </NavLink>
      </nav>
    </div>
  );
};

export default Sidebar;
