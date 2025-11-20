import { useState } from "react";
import { useAuth } from "../../context/authContext"
import { useNavigate } from "react-router-dom";
import { loginApi } from "../../services/userService";
import { setTokens } from "../../untils/tokenUtils";
const LoginForm = () =>{
    const [login, setLogin] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");
    const navigate = useNavigate();
    const handleSubmit = async (e) =>{
        e.preventDefault();
        setError("");
        try {
            const res = await loginApi(login, password); 
            setTokens(res.accessToken, res.refreshToken);
            navigate("/admin");
        } catch (err) {
            console.error(err);
            setError("Email hoặc mật khẩu không đúng");
        }
    };
    return(
         <form onSubmit={handleSubmit} className="space-y-5">
      {error && <div className="text-red-500">{error}</div>}

      <div>
        <label className="block mb-2 text-gray-700 font-medium">Email</label>
        <input
          type="text"
          value={login}
          onChange={(e) => setLogin(e.target.value)}
          placeholder="Nhập email..."
          className="w-full border border-gray-300 rounded-lg p-3 focus:ring-2 focus:ring-red-500 outline-none"
          required
        />
      </div>

      <div>
        <label className="block mb-2 text-gray-700 font-medium">Mật khẩu</label>
        <input
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          placeholder="Nhập mật khẩu..."
          className="w-full border border-gray-300 rounded-lg p-3 focus:ring-2 focus:ring-red-500 outline-none"
          required
        />
      </div>

      <div className="flex items-center justify-between text-sm">
        <label className="flex items-center space-x-2">
          <input type="checkbox" className="w-4 h-4 text-red-500" />
          <span>Ghi nhớ đăng nhập</span>
        </label>
        <a href="#" className="text-red-600 hover:underline">
          Quên mật khẩu?
        </a>
      </div>

      <button
        type="submit"
        className="w-full bg-red-600 hover:bg-red-700 text-white font-semibold py-3 rounded-lg transition"
      >
        Đăng nhập
      </button>
    </form>
    );
}
export default LoginForm;