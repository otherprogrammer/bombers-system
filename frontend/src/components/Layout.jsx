import { NavLink, Outlet } from "react-router-dom";
import {
  LayoutDashboard,
  Siren,
  Building2,
  Truck,
  Users,
  ShieldCheck,
  LogOut,
} from "lucide-react";
import { useAuth } from "../context/AuthContext";

const navItems = [
  { to: "/", label: "Dashboard", icon: LayoutDashboard, end: true },
  { to: "/incidents", label: "Incidentes", icon: Siren },
  { to: "/stations", label: "Estaciones", icon: Building2 },
  { to: "/vehicles", label: "Vehículos", icon: Truck },
  { to: "/personnel", label: "Personal", icon: Users },
];

export default function Layout() {
  const { user, logout } = useAuth();
  const isAdmin = user?.roles?.includes("Administrador");

  return (
    <div className="flex min-h-screen bg-background">
      {/* Sidebar */}
      <aside className="fixed left-0 top-0 h-screen w-60 bg-surface border-r border-outline-variant flex flex-col py-6 z-50">
        <div className="px-6 mb-8">
          <h1 className="text-2xl font-black text-primary uppercase tracking-tighter">
            FireOps Command
          </h1>
          <p className="text-[11px] font-semibold text-secondary uppercase tracking-widest opacity-70 mt-1">
            Central Dispatch
          </p>
        </div>

        <nav className="flex-1 space-y-1 px-3">
          {navItems.map(({ to, label, icon: Icon, end }) => (
            <NavLink
              key={to}
              to={to}
              end={end}
              className={({ isActive }) =>
                `flex items-center gap-3 px-4 py-3 rounded text-sm font-semibold tracking-tight transition-colors ${
                  isActive
                    ? "text-primary bg-surface-container-high border-l-4 border-primary"
                    : "text-secondary hover:bg-surface-container hover:text-primary"
                }`
              }
            >
              <Icon size={20} />
              <span>{label}</span>
            </NavLink>
          ))}

          {isAdmin && (
            <NavLink
              to="/admin"
              className={({ isActive }) =>
                `flex items-center gap-3 px-4 py-3 rounded text-sm font-semibold tracking-tight transition-colors ${
                  isActive
                    ? "text-primary bg-surface-container-high border-l-4 border-primary"
                    : "text-secondary hover:bg-surface-container hover:text-primary"
                }`
              }
            >
              <ShieldCheck size={20} />
              <span>Admin</span>
            </NavLink>
          )}
        </nav>

        <div className="px-4 mt-auto pt-4 border-t border-outline-variant">
          <div className="flex items-center gap-3 px-2 mb-3">
            <div className="w-9 h-9 rounded-full bg-primary-container flex items-center justify-center text-on-primary-container font-bold text-sm">
              {user?.username?.slice(0, 2).toUpperCase() ?? "U"}
            </div>
            <div className="overflow-hidden">
              <p className="text-sm font-bold truncate">{user?.username}</p>
              <p className="text-[11px] text-secondary truncate">
                {user?.roles?.join(", ") || "Sin rol"}
              </p>
            </div>
          </div>
          <button
            onClick={logout}
            className="w-full flex items-center justify-center gap-2 py-2 text-xs font-bold uppercase tracking-widest text-secondary border border-outline-variant rounded hover:bg-surface-container-high hover:text-primary transition-colors"
          >
            <LogOut size={16} />
            Cerrar sesión
          </button>
        </div>
      </aside>

      {/* Main content */}
      <main className="ml-60 flex-1 min-h-screen">
        <Outlet />
      </main>
    </div>
  );
}
