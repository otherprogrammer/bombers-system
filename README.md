# 🚒 bombers-system

Sistema de Gestión Operativa para Cuerpos de Bomberos — REST API desarrollada con arquitectura hexagonal, principios SOLID y patrones de diseño empresariales.

## 📋 Descripción

**bombers-system** centraliza y optimiza el registro de incidentes, la asignación de unidades y personal, y el seguimiento de intervenciones en campo. Su diseño parte de una entrevista presencial con integrantes activos de un cuerpo de bomberos.

### Problema que resuelve
- Demoras en la coordinación y despacho de unidades durante emergencias
- Pérdida del historial de intervenciones y ausencia de trazabilidad operativa
- Dificultad para generar reportes post-incidente con información confiable
- Falta de control sobre la asignación de recursos a incidentes específicos

---

## 🏗️ Arquitectura

El proyecto sigue una **arquitectura hexagonal (Puertos y Adaptadores)** organizada en 4 capas:

```
bombers-system/
├── Bombers-System/                  # Web API — Controllers, Program.cs, Middleware
├── Bombers-System.Application/      # Casos de uso — Commands, Queries (CQRS con MediatR)
├── Bombers-System.Domain/           # Dominio — Entities, Ports (interfaces), DTOs, Exceptions
└── Bombers-System.Infrastructure/   # Infraestructura — Adapters (repos), Persistence, Config
```

### Flujo de dependencias
```
API → Application → Domain ← Infrastructure
```
El **Domain** no depende de ninguna otra capa.

---

## 🛠️ Stack Tecnológico

| Capa | Tecnología |
|------|-----------|
| Lenguaje | C# / .NET 8 |
| ORM | Entity Framework Core 8 |
| Base de datos | PostgreSQL (Supabase) |
| Autenticación | JWT Bearer + BCrypt |
| CQRS | MediatR 14 |
| Documentación API | Swagger / OpenAPI |
| Geoespacial | NetTopologySuite + PostGIS |

### Patrones aplicados
- **Repository Pattern** + **Generic Repository**
- **Unit of Work**
- **CQRS** (Commands y Queries separados por caso de uso)
- **Hexagonal Architecture** (Ports & Adapters)
- **Principios SOLID** en todas las capas

---

## 📦 Módulos

| Módulo | Responsable | Estado |
|--------|-------------|--------|
| Arquitectura base + Autenticación | Carlos Huamani | ✅ |
| Estaciones + Vehículos | Ricardo Quispe | ✅ |
| Personal (Bomberos) | Benique Arela | ✅ |
| Incidentes + Reportes | Angelous Nando | ✅ |
| Asignación de Recursos | Mattias Marquez | 🔄 |

---

## 🚀 Despliegue

| Componente | Plataforma |
|-----------|-----------|
| Base de datos | Supabase (PostgreSQL) |
| Backend API | Render |
| Frontend | Vercel |

---

## ⚙️ Configuración local

### Requisitos
- .NET 8 SDK
- PostgreSQL o conexión a Supabase

### Pasos

```bash
# 1. Clonar el repositorio
git clone https://github.com/otherprogrammer/bombers-system.git
cd bombers-system

# 2. Configurar la cadena de conexión en appsettings.json
# Bombers-System/appsettings.json → ConnectionStrings:DefaultConnection

# 3. Restaurar dependencias y compilar
dotnet restore Bombers-System.sln
dotnet build Bombers-System.sln

# 4. Ejecutar la API
cd Bombers-System
dotnet run
```

### Variables de configuración (`appsettings.json`)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=...;Database=postgres;Username=...;Password=...;Port=5432;SSL Mode=Require;"
  },
  "Jwt": {
    "SecretKey": "<tu-secret-key>",
    "Issuer": "bombers-api",
    "Audience": "bombers-app"
  }
}
```

> ⚠️ Nunca subas credenciales reales al repositorio. Usa variables de entorno en producción.

---

## 📖 Documentación de la API

Una vez corriendo, accede a Swagger en:

```
https://localhost:{puerto}/swagger
```

### Endpoints principales

| Método | Ruta | Descripción |
|--------|------|-------------|
| POST | `/api/auth/register` | Registro de usuario |
| POST | `/api/auth/login` | Login y obtención de JWT |
| GET | `/api/stations` | Listar estaciones |
| POST | `/api/stations` | Crear estación |
| GET | `/api/vehicles` | Listar vehículos |
| GET | `/api/firefighters` | Listar bomberos |
| POST | `/api/incidents` | Crear incidente |
| PATCH | `/api/incidents/{id}/status` | Cambiar estado del incidente |
| POST | `/api/dispatches` | Asignar recursos a incidente |

---

## 📁 Estructura detallada

```
Bombers-System.Domain/
├── Entities/          # CadIncident, Firefighter, OperationalDispatch, Station, Vehicle...
├── Ports/             # IGenericRepository, IUnitOfWork, IStationRepository...
├── DTOs/              # Objetos de transferencia por módulo
└── Exceptions/        # NotFoundException, BadRequestException...

Bombers-System.Application/
├── Configuration/     # ApplicationServicesExtensions (registro MediatR)
└── UseCases/          # AuthUseCases, StationUseCases, FirefighterUseCases, IncidentUseCases...
    ├── Commands/      # Operaciones de escritura
    └── Queries/       # Operaciones de lectura

Bombers-System.Infrastructure/
├── Adapters/          # GenericRepository, StationRepository, UnitOfWork, JwtProvider...
├── Persistence/       # ApplicationDbContext (Fluent API)
└── Configuration/     # InfrastructureServiceExtensions

Bombers-System/
├── Controllers/       # StationsController, VehiclesController, IncidentController...
├── Configuration/     # ServiceRegistrationExtensions
├── Middleware/        # ExceptionHandlingMiddleware
└── Program.cs
```

---

## 👥 Equipo

| Integrante | Módulo |
|-----------|--------|
| Carlos Huamani | Arquitectura base + JWT + Roles |
| Ricardo Quispe | Estaciones + Vehículos |
| Benique Arela | Personal (Bomberos) |
| Angelous Nando | Incidentes + Reportes de intervención |
| Mattias Marquez | Asignación de Recursos |
