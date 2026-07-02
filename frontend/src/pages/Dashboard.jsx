import { useEffect, useState } from "react";
import { Siren, Truck, Users, Building2 } from "lucide-react";
import { Link } from "react-router-dom";
import PageHeader from "../components/PageHeader";
import StatCard from "../components/StatCard";
import Badge from "../components/Badge";
import { stationsService } from "../services/stations";
import { vehiclesService } from "../services/vehicles";
import { firefightersService } from "../services/firefighters";
import { incidentsService, INCIDENT_STATUS_LABELS } from "../services/incidents";
import { useAuth } from "../context/AuthContext";

const statusColor = { Reportado: "yellow", EnAtencion: "blue", Cerrado: "green" };

export default function Dashboard() {
  const { user } = useAuth();
  const [data, setData] = useState({
    stations: [],
    vehicles: [],
    firefighters: [],
    incidents: [],
  });
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    async function load() {
      setLoading(true);
      setError("");
      try {
        const [stations, vehicles, firefighters, incidents] = await Promise.all([
          stationsService.getAll(),
          vehiclesService.getAll(),
          firefightersService.getAll(),
          incidentsService.getAll(),
        ]);
        setData({ stations, vehicles, firefighters, incidents });
      } catch (err) {
        setError(
          err.response?.data?.message ||
            "No se pudo cargar la información del dashboard."
        );
      } finally {
        setLoading(false);
      }
    }
    load();
  }, []);

  const activeIncidents = data.incidents.filter((i) => i.status !== "Cerrado");
  const availableVehicles = data.vehicles.filter(
    (v) => v.operationalStatus === "Disponible"
  );
  const activeFirefighters = data.firefighters.filter(
    (f) => f.currentStatus === "Activo"
  );
  const recent = [...data.incidents].slice(-6).reverse();

  return (
    <div className="p-8">
      <PageHeader
        title="Dashboard"
        subtitle={`Bienvenido, ${user?.username ?? "usuario"} — resumen operativo en tiempo real.`}
      />

      {error && (
        <div className="mb-4 bg-error-container text-on-error-container text-sm font-medium px-4 py-3 rounded border-l-4 border-error">
          {error}
        </div>
      )}

      {loading ? (
        <p className="text-secondary">Cargando datos...</p>
      ) : (
        <>
          <section className="grid grid-cols-1 md:grid-cols-4 gap-4 mb-8">
            <StatCard
              label="Incidentes Activos"
              value={activeIncidents.length}
              sublabel={`${data.incidents.length} total`}
              icon={Siren}
              color="primary"
            />
            <StatCard
              label="Vehículos Disponibles"
              value={availableVehicles.length}
              sublabel={`${data.vehicles.length} en flota`}
              icon={Truck}
              color="tertiary"
            />
            <StatCard
              label="Personal Activo"
              value={activeFirefighters.length}
              sublabel={`${data.firefighters.length} registrados`}
              icon={Users}
              color="secondary"
            />
            <StatCard
              label="Estaciones"
              value={data.stations.length}
              sublabel="en operación"
              icon={Building2}
              color="outline"
            />
          </section>

          <div className="grid grid-cols-1 xl:grid-cols-3 gap-6">
            <div className="xl:col-span-2 bg-surface-container-lowest border border-outline-variant rounded-lg shadow-sm overflow-hidden">
              <div className="px-6 py-4 border-b border-outline-variant bg-surface-container-low flex justify-between items-center">
                <h3 className="font-black flex items-center gap-2">
                  <Siren size={18} className="text-primary" />
                  Incidentes Recientes
                </h3>
                <Link
                  to="/incidents"
                  className="text-xs font-bold text-primary hover:underline uppercase"
                >
                  Ver todos
                </Link>
              </div>
              <table className="w-full text-left">
                <thead>
                  <tr className="bg-surface-container-highest text-xs uppercase text-on-surface-variant">
                    <th className="px-4 py-2 font-bold">Código</th>
                    <th className="px-4 py-2 font-bold">Tipo</th>
                    <th className="px-4 py-2 font-bold">Prioridad</th>
                    <th className="px-4 py-2 font-bold">Estado</th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-outline-variant">
                  {recent.length === 0 ? (
                    <tr>
                      <td colSpan={4} className="px-4 py-6 text-center text-secondary">
                        Sin incidentes registrados.
                      </td>
                    </tr>
                  ) : (
                    recent.map((i) => (
                      <tr key={i.incidentId}>
                        <td className="px-4 py-3 font-mono font-bold text-primary">
                          {i.incidentCode}
                        </td>
                        <td className="px-4 py-3">{i.emergencyType}</td>
                        <td className="px-4 py-3">{i.priorityLevel}</td>
                        <td className="px-4 py-3">
                          <Badge color={statusColor[i.status] || "gray"}>
                            {INCIDENT_STATUS_LABELS[i.status] || i.status}
                          </Badge>
                        </td>
                      </tr>
                    ))
                  )}
                </tbody>
              </table>
            </div>

            <div className="bg-surface-container-lowest border border-outline-variant rounded-lg shadow-sm">
              <div className="px-6 py-4 border-b border-outline-variant bg-surface-container-low">
                <h3 className="font-black flex items-center gap-2">
                  <Building2 size={18} className="text-tertiary" />
                  Estaciones
                </h3>
              </div>
              <div className="p-4 space-y-3">
                {data.stations.length === 0 ? (
                  <p className="text-secondary text-sm">Sin estaciones registradas.</p>
                ) : (
                  data.stations.map((s) => {
                    const vCount = data.vehicles.filter(
                      (v) => v.stationId === s.stationId
                    ).length;
                    return (
                      <div
                        key={s.stationId}
                        className="flex items-center justify-between p-2 rounded hover:bg-surface-container-low"
                      >
                        <div>
                          <p className="font-bold text-sm">
                            Estación {s.stationNumber}
                          </p>
                          <p className="text-xs text-secondary">{s.address}</p>
                        </div>
                        <span className="font-mono text-xs font-bold">
                          {vCount}/{s.vehicleCapacity}
                        </span>
                      </div>
                    );
                  })
                )}
              </div>
            </div>
          </div>
        </>
      )}
    </div>
  );
}
