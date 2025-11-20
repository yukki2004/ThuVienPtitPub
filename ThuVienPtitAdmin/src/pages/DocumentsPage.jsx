import UploadDocForm from "../components/Form/UploadDocForm";
import PubDocument from "../components/List/PubDocument";
import { useEffect, useState } from "react";

const DocumentsPage = () => {
   const [reloadFlag, setReloadFlag] = useState(0);

  const handleUploadSuccess = () => {
    setReloadFlag(prev => prev + 1);
  };

  return (
    <div className="space-y-6">
      <UploadDocForm onUploadSuccess={handleUploadSuccess} />
      <PubDocument reloadFlag={reloadFlag} />
    </div>
  );
}
export default DocumentsPage;