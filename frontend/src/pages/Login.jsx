import { useState } from "react";
import { useNavigate, useLocation, Navigate } from "react-router-dom";
import { Flame, User, Lock, LogIn } from "lucide-react";
import { useAuth } from "../context/AuthContext";

export default function Login() {
  const { user, login } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const [submitting, setSubmitting] = useState(false);

  if (user) {
    return <Navigate to={location.state?.from?.pathname || "/"} replace />;
  }

  async function handleSubmit(e) {
    e.preventDefault();
    setError("");
    setSubmitting(true);
    try {
      await login(username, password);
      navigate(location.state?.from?.pathname || "/", { replace: true });
    } catch (err) {
      const msg =
        err.response?.data?.message ||
        err.response?.data?.title ||
        "Usuario o contraseña incorrectos.";
      setError(msg);
    } finally {
      setSubmitting(false);
    }
  }

  return (
    <div className="min-h-screen flex flex-col items-center justify-center bg-background relative overflow-hidden px-4">
      <main className="relative z-10 w-full max-w-[400px]">
        <div className="text-center mb-8">
          <div className="inline-flex items-center justify-center w-16 h-16 bg-primary-container rounded-lg mb-4 shadow-lg border border-primary">
            <Flame className="text-on-primary" size={32} />
          </div>
          <h1 className="text-2xl font-black text-primary tracking-tight">
            FireOps Command
          </h1>
          <p className="text-sm text-on-surface-variant mt-1">
            Sistema de Gestión Operativa — bombers-system
          </p>
        </div>

        <div className="bg-white border border-outline-variant p-8 rounded-lg shadow-sm">
          <form onSubmit={handleSubmit} className="flex flex-col gap-4">
            {error && (
              <div className="bg-error-container text-on-error-container text-sm font-medium px-3 py-2 rounded border-l-4 border-error">
                {error}
              </div>
            )}

            <div className="flex flex-col gap-1">
              <label
                className="text-[11px] font-semibold text-on-surface-variant uppercase tracking-wider"
                htmlFor="username"
              >
                Usuario
              </label>
              <div className="relative">
                <User
                  className="absolute left-3 top-1/2 -translate-y-1/2 text-on-surface-variant"
                  size={18}
                />
                <input
                  id="username"
                  type="text"
                  required
                  value={username}
                  onChange={(e) => setUsername(e.target.value)}
                  placeholder="Ingresa tu usuario"
                  className="w-full pl-10 pr-4 py-3 bg-surface-bright border border-outline-variant focus:border-primary focus:ring-1 focus:ring-primary rounded-lg text-sm outline-none transition-all"
                />
              </div>
            </div>

            <div className="flex flex-col gap-1">
              <label
                className="text-[11px] font-semibold text-on-surface-variant uppercase tracking-wider"
                htmlFor="password"
              >
                Contraseña
              </label>
              <div className="relative">
                <Lock
                  className="absolute left-3 top-1/2 -translate-y-1/2 text-on-surface-variant"
                  size={18}
                />
                <input
                  id="password"
                  type="password"
                  required
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                  placeholder="••••••••"
                  className="w-full pl-10 pr-4 py-3 bg-surface-bright border border-outline-variant focus:border-primary focus:ring-1 focus:ring-primary rounded-lg text-sm outline-none transition-all"
                />
              </div>
            </div>

            <button
              type="submit"
              disabled={submitting}
              className="mt-2 w-full bg-primary py-3 px-6 text-on-primary font-bold rounded-lg shadow-md hover:brightness-110 active:scale-[0.98] transition-all flex items-center justify-center gap-2 disabled:opacity-60"
            >
              {submitting ? "Ingresando..." : "Acceder"}
              <LogIn size={18} />
            </button>
          </form>
        </div>
      </main>
    </div>
  );
}
