import React, { createContext, useContext, useEffect, useState } from "react";
import { getMe, logoutApi } from "../services/userService";
import { getRefreshToken, removeTokens, setTokens } from "../untils/tokenUtils";
const authContext = createContext();
export const useAuth = () => useContext(authContext);
export const  AuthProvider = ({children}) =>{
    const [user, setUser] = useState(null);
    const [loading, setLoading] = useState(true);
    const loadUser = async () =>{
        try{
            const me = await getMe();
            setUser(me);
        } catch (err){
            console.error(err);

        } finally {
            setLoading(false);
        }
        
    }

    const logout = async () => {
        try {
            const refresh_token = getRefreshToken();
            if(refresh_token) await logoutApi(refresh_token);
        } catch{
            console.error(err);
        }
        removeTokens();
        setUser(null);
        window.location.href = "/login";
    }
    useEffect(()=>{
        loadUser();
    },[]);
    return (
        <authContext.Provider value={{user, loading, logout}}>
            {loading ? <div>Loading...</div> : children}
        </authContext.Provider>
    );
}