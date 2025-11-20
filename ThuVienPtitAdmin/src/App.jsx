import { BrowserRouter as Router, Routes, Route, Navigate } from "react-router-dom";
import DashboardPage from './pages/DashboardPage.jsx';
import SubjectsPage from './pages/SubjectPage.jsx';
import DocumentsPage from './pages/DocumentsPage.jsx';
import TrashDocumentsPage from './pages/TrashDocumentsPage.jsx';
import DocumentDetail from './pages/DocumentDetail.jsx';
import UserDetail from './pages/UserDetail.jsx';
import PendingDocumentsPage from './pages/PendingDocumentsPage';
import AdminLayout from './layouts/AdminLayout.jsx';
import AuthLayout from './layouts/AuthLayout.jsx';
import LoginPage from './pages/LoginPage.jsx';
import UserListPage from './pages/UserListPage.jsx';
import SubjectDetail from "./pages/SubjectDetail.jsx";
import { AuthProvider } from './context/authContext';
import './index.css';
import TagPage from "./pages/TagPage.jsx";
import TagPageDetail from "./pages/TagPageDetail.jsx";

function App() {
  return (
    <Router>
      <Routes>
        {/* Public route */}
        <Route element={<AuthLayout />}>
          <Route path="/login" element={<LoginPage />} />
        </Route>

        {/* Protected routes */}
        <Route element={<AuthProvider><AdminLayout /></AuthProvider>}>
          <Route path="/admin" element={<DashboardPage />} />
          <Route path="/admin/subjects" element={<SubjectsPage />} />
          <Route path="/admin/documents" element={<DocumentsPage />} />
          <Route path="/admin/pending-documents" element={<PendingDocumentsPage />} />
          <Route path="/admin/trash-documents" element={<TrashDocumentsPage />} />
          <Route path="/admin/users" element={<UserListPage />} />
          <Route path = "/admin/tag" element = {<TagPage/>} />
          <Route path="/admin/documents/:slug" element={<DocumentDetail />} />
          <Route path="/admin/users/:userid" element={<UserDetail />} />
          <Route path ="/admin/subjects/:slug" element={<SubjectDetail/>} />
          <Route path = "/admin/tag/:tagId" element = {<TagPageDetail/>} />
        </Route>

        {/* Redirect root to /admin */}
        <Route path="/" element={<Navigate to="/admin" />} />
        <Route path="*" element={<h1>404 Not Found</h1>} />
      </Routes>
    </Router>
  );
}

export default App;
