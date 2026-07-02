import { createContext, useContext, useEffect, useState } from "react";
import api from "../lib/api";
import { getUserFromToken, isTokenExpired } from "../lib/jwt";

const AuthContext = createContext(null);

export function AuthProvider({ children }) {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const token = localStorage.getItem("bombers_token");
    if (token && !isTokenExpired(token)) {
      setUser(getUserFromToken(token));
    } else {
      localStorage.removeItem("bombers_token");
    }
    setLoading(false);
  }, []);

  async function login(username, password) {
    const { data } = await api.post("/auth/login", { username, password });
    localStorage.setItem("bombers_token", data.accessToken);
    if (data.refreshToken) {
      localStorage.setItem("bombers_refresh_token", data.refreshToken);
    }
    const decoded = getUserFromToken(data.accessToken);
    setUser(decoded);
    return decoded;
  }

  function logout() {
    localStorage.removeItem("bombers_token");
    localStorage.removeItem("bombers_refresh_token");
    setUser(null);
  }

  return (
    <AuthContext.Provider value={{ user, loading, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error("useAuth must be used within an AuthProvider");
  return ctx;
}
