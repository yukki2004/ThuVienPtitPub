import DeletedDocument from "../components/List/DeleteDocument";

const TrashDocumentsPage = () => {
    return (
    <div className="space-y-6">
      <h2 className="text-2xl font-semibold text-gray-800">Xóa tài liệu</h2>
      <DeletedDocument />
    </div>
   )
}
export default TrashDocumentsPage;