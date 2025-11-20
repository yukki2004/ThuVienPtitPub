import api from "./apiClient.js";
export const uploadDocumentApi = ({
  title,
  description,
  avtDocument, 
  fileDocument,
  course_id,
  semester_id,
  category,
  tag_ids = [],
}) => {
  const formData = new FormData();
  formData.append("title", title);
  formData.append("description", description);
  if (avtDocument) formData.append("avtDocument", avtDocument);
  if (fileDocument) formData.append("fileDocument", fileDocument);
  formData.append("course_id", course_id);
  formData.append("semester_id", semester_id);
  formData.append("category", category);
  tag_ids.forEach((id) => formData.append("tag_ids[]", id));

  return api.post("/Document/add-document", formData, {
    headers: {
      "Content-Type": "multipart/form-data",
    },
  });
};
export const getPubLishDocumentApi = (pageNumber = 1)=>{
    return api.get("/Document/AdminPublishDocuments",{
        params: {
            pageNumber,
        }
    })
};
export const deleteDocumentApi = (documentId) => {
  return api.delete(`/Document/DeleteDocument/${documentId}`);
};
export const getDocumentApi = (slug) => {
    return api.get(`/Document/GetDocumentBySlug/${slug}`);
}
export const getPendingDocumentApi = (pageNumber = 1)=>{
    return api.get("/Document/AdminPendingDocuments",{
        params: {
            pageNumber,
        }
    })
};
export const getDeleteDocumentApi = (pageNumber = 1)=>{
    return api.get("/Document/AdminDeleteDocuments",{
        params: {
            pageNumber,
        }
    })
};
export const rejectDocumentApi = (documentId) => {
    return api.patch(`/Document/RejectDocument/${documentId}`)
}
export const approveDocumentApi = (documentId) => {
    return api.patch(`/Document/ApproveDocument/${documentId}`)
}
export const clearDocumentApi = (documentId) => {
    return api.delete(`/Document/ClearDocument/${documentId}`)
}
export const restoreDocumentApi = (documentId) => {
    return api.patch(`/Document/RestoreDocument/${documentId}`)
}