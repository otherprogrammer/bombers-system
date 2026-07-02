import { useEffect, useState } from "react";
import { Plus, Trash2, ShieldCheck, X } from "lucide-react";
import PageHeader from "../components/PageHeader";
import Modal from "../components/Modal";
import { usersService, rolesService, userRolesService } from "../services/users";

const emptyForm = { username: "", password: "", firefighterId: "" };

export default function Admin() {
  const [users, setUsers] = useState([]);
  const [roles, setRoles] = useState([]);
  const [userRoles, setUserRoles] = useState({});
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [modalOpen, setModalOpen] = useState(false);
  const [form, setForm] = useState(emptyForm);
  const [saving, setSaving] = useState(false);
  const [assignTarget, setAssignTarget] = useState(null);
  const [roleToAssign, setRoleToAssign] = useState("");

  async function loadAll() {
    setLoading(true);
    setError("");
    try {
      const [u, r] = await Promise.all([
        usersService.getAll(),
        rolesService.getAll(),
      ]);
      setUsers(u);
      setRoles(r);
      const rolesMap = {};
      await Promise.all(
        u.map(async (usr) => {
          try {
            rolesMap[usr.userId] = await userRolesService.getByUser(usr.userId);
          } catch {
            rolesMap[usr.userId] = [];
          }
        })
      );
      setUserRoles(rolesMap);
    } catch (err) {
      setError(
        err.response?.data?.message ||
          "No se pudo cargar la consola administrativa. ¿Tu usuario tiene rol Administrador?"
      );
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    loadAll();
  }, []);

  async function handleCreate(e) {
    e.preventDefault();
    setSaving(true);
    setError("");
    try {
      await usersService.create({
        username: form.username,
        password: form.password,
        firefighterId: form.firefighterId ? Number(form.firefighterId) : null,
      });
      setModalOpen(false);
      setForm(emptyForm);
      await loadAll();
    } catch (err) {
      setError(
        err.response?.data?.message || "No se pudo crear el usuario."
      );
    } finally {
      setSaving(false);
    }
  }

  async function handleDeleteUser(u) {
    if (!confirm(`¿Eliminar al usuario ${u.username}?`)) return;
    try {
      await usersService.remove(u.userId);
      await loadAll();
    } catch (err) {
      setError(
        err.response?.data?.message || "No se pudo eliminar el usuario."
      );
    }
  }

  async function handleAssignRole(e) {
    e.preventDefault();
    if (!roleToAssign) return;
    try {
      await userRolesService.assign(assignTarget.userId, Number(roleToAssign));
      setAssignTarget(null);
      setRoleToAssign("");
      await loadAll();
    } catch (err) {
      setError(
        err.response?.data?.message || "No se pudo asignar el rol."
      );
    }
  }

  async function handleRemoveRole(userId, roleId) {
    try {
      await userRolesService.remove(userId, roleId);
      await loadAll();
    } catch (err) {
      setError(
        err.response?.data?.message || "No se pudo quitar el rol."
      );
    }
  }

  return (
    <div className="p-8">
      <PageHeader
        title="Consola Administrativa"
        subtitle="Gestión de usuarios, roles y permisos del sistema."
        action={
          <button
            onClick={() => setModalOpen(true)}
            className="bg-primary text-on-primary px-5 py-2.5 rounded-lg font-bold flex items-center gap-2 hover:brightness-110 active:scale-95 transition-all shadow-sm"
          >
            <Plus size={18} />
            Nuevo Usuario
          </button>
        }
      />

      {error && (
        <div className="mb-4 bg-error-container text-on-error-container text-sm font-medium px-4 py-3 rounded border-l-4 border-error">
          {error}
        </div>
      )}

      <div className="bg-surface-container-lowest border border-outline-variant rounded-lg overflow-hidden shadow-sm">
        <table className="w-full text-left border-collapse">
          <thead>
            <tr className="bg-surface-container text-on-surface-variant text-xs uppercase tracking-widest border-b border-outline-variant">
              <th className="px-4 py-3 font-black">Usuario</th>
              <th className="px-4 py-3 font-black">Bombero vinculado</th>
              <th className="px-4 py-3 font-black">Roles</th>
              <th className="px-4 py-3 font-black text-right">Acciones</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-outline-variant">
            {loading ? (
              <tr>
                <td colSpan={4} className="px-4 py-6 text-center text-secondary">
                  Cargando...
                </td>
              </tr>
            ) : users.length === 0 ? (
              <tr>
                <td colSpan={4} className="px-4 py-6 text-center text-secondary">
                  No hay usuarios registrados.
                </td>
              </tr>
            ) : (
              users.map((u) => (
                <tr key={u.userId} className="hover:bg-surface-container-low transition-colors align-top">
                  <td className="px-4 py-3 font-bold">{u.username}</td>
                  <td className="px-4 py-3 text-sm text-secondary">
                    {u.firefighterId ?? "—"}
                  </td>
                  <td className="px-4 py-3">
                    <div className="flex flex-wrap gap-1 items-center">
                      {(userRoles[u.userId] || []).map((r) => (
                        <span
                          key={r.roleId}
                          className="inline-flex items-center gap-1 bg-secondary-container text-on-secondary-container px-2 py-0.5 rounded text-[11px] font-bold"
                        >
                          {r.roleName}
                          <button
                            onClick={() => handleRemoveRole(u.userId, r.roleId)}
                            title="Quitar rol"
                          >
                            <X size={12} />
                          </button>
                        </span>
                      ))}
                      <button
                        onClick={() => {
                          setAssignTarget(u);
                          setRoleToAssign("");
                        }}
                        className="text-xs font-bold text-primary hover:underline flex items-center gap-1"
                      >
                        <Plus size={12} /> rol
                      </button>
                    </div>
                  </td>
                  <td className="px-4 py-3 text-right">
                    <button
                      onClick={() => handleDeleteUser(u)}
                      className="p-1.5 text-secondary hover:text-error transition-colors"
                      title="Eliminar usuario"
                    >
                      <Trash2 size={16} />
                    </button>
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>

      {modalOpen && (
        <Modal title="Nuevo Usuario" onClose={() => setModalOpen(false)}>
          <form onSubmit={handleCreate} className="flex flex-col gap-4">
            <div>
              <label className="block text-xs font-bold text-on-surface-variant uppercase mb-1">
                Usuario
              </label>
              <input
                type="text"
                required
                value={form.username}
                onChange={(e) => setForm({ ...form, username: e.target.value })}
                className="w-full border border-outline-variant rounded-lg px-3 py-2 focus:ring-2 focus:ring-primary outline-none"
              />
            </div>
            <div>
              <label className="block text-xs font-bold text-on-surface-variant uppercase mb-1">
                Contraseña
              </label>
              <input
                type="password"
                required
                value={form.password}
                onChange={(e) => setForm({ ...form, password: e.target.value })}
                className="w-full border border-outline-variant rounded-lg px-3 py-2 focus:ring-2 focus:ring-primary outline-none"
              />
            </div>
            <div>
              <label className="block text-xs font-bold text-on-surface-variant uppercase mb-1">
                ID de Bombero (opcional)
              </label>
              <input
                type="number"
                value={form.firefighterId}
                onChange={(e) =>
                  setForm({ ...form, firefighterId: e.target.value })
                }
                className="w-full border border-outline-variant rounded-lg px-3 py-2 focus:ring-2 focus:ring-primary outline-none"
              />
            </div>
            <p className="text-xs text-secondary">
              El usuario se crea con el rol "Bombero" por defecto (asignable en la tabla).
            </p>
            <button
              type="submit"
              disabled={saving}
              className="mt-2 bg-primary text-on-primary py-2.5 rounded-lg font-bold hover:brightness-110 transition-all disabled:opacity-60"
            >
              {saving ? "Creando..." : "Crear Usuario"}
            </button>
          </form>
        </Modal>
      )}

      {assignTarget && (
        <Modal
          title={`Asignar rol a ${assignTarget.username}`}
          onClose={() => setAssignTarget(null)}
        >
          <form onSubmit={handleAssignRole} className="flex flex-col gap-4">
            <div>
              <label className="block text-xs font-bold text-on-surface-variant uppercase mb-1 flex items-center gap-1">
                <ShieldCheck size={14} /> Rol
              </label>
              <select
                required
                value={roleToAssign}
                onChange={(e) => setRoleToAssign(e.target.value)}
                className="w-full border border-outline-variant rounded-lg px-3 py-2 focus:ring-2 focus:ring-primary outline-none"
              >
                <option value="">Selecciona un rol</option>
                {roles.map((r) => (
                  <option key={r.roleId} value={r.roleId}>
                    {r.roleName}
                  </option>
                ))}
              </select>
            </div>
            <button
              type="submit"
              className="bg-primary text-on-primary py-2.5 rounded-lg font-bold hover:brightness-110 transition-all"
            >
              Asignar
            </button>
          </form>
        </Modal>
      )}
    </div>
  );
}
