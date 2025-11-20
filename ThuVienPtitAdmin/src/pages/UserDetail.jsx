import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { getUserByAdminApi } from "../services/userService";
import UserInfoCard from "../components/card/UserInfoCard";
import PenUserDocList from "../components/List/PenUserDocList";
import PubUserDocList from "../components/List/PubUserDocList";

const UserDetail = () => {
  const { userid } = useParams();
  const [user, setUser] = useState(null);
  const [refresh, setRefresh] = useState(false); 
  useEffect(() => {
    fetchUser();
  }, [userid]);

  const fetchUser = async () => {
    const res = await getUserByAdminApi(userid);
    setUser(res);
  };

  return (
    <div className="p-6 space-y-6">
      {user && <UserInfoCard user={user} />}

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        <PenUserDocList userId={userid} onRefresh={() => setRefresh(!refresh)} />
        <PubUserDocList userId={userid} refresh={refresh} />
      </div>
    </div>
  );
};

export default UserDetail;
