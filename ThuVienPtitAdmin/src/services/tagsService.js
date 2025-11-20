import api from "./apiClient";

export const getAllTagApi = () => api.get("/Tag/get-all-tags")
export const getAllTagAdminApi = (pageNumber = 1) =>{
    return api.get("/Tag/get-all-list-tag-admin",{
        params: {
            pageNumber,
        }
    })
}
export const createTagApi = ({name, description})=>{
    return api.post("/Tag/create-tag", {name, description})
}
export const deleteTagApi = (id) =>{
    return api.delete(`/Tag/delete-tag/${id}`)
}
export const getStatTagApi = (tagId) =>{
    return api.get(`/Tag/get-stat-tag-admin/${tagId}`)
}
export const getDocumentTagFilterApi =(tagId, pageNumber = 1, status = null)=>{
    const params = {pageNumber};
    if (status !== null) params.status = status;
    return api.get(`/Tag/get-document-filter-admin/${tagId}`,{params})
}