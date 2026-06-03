using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Bombers_System.Domain.Entities;

namespace Bombers_System.Infrastructure.Persistence;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CadIncident> CadIncidents { get; set; }

    public virtual DbSet<DispatchCrew> DispatchCrews { get; set; }

    public virtual DbSet<DutyShift> DutyShifts { get; set; }

    public virtual DbSet<FirefighterPersonnel> FirefighterPersonnel { get; set; }

    public virtual DbSet<OperationalDispatch> OperationalDispatches { get; set; }

    public virtual DbSet<PostIncidentReport> PostIncidentReports { get; set; }

    public virtual DbSet<PpeEquipment> PpeEquipments { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Station> Stations { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<UserToken> UserTokens { get; set; }

    public virtual DbSet<Vehicle> Vehicles { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("auth", "aal_level", new[] { "aal1", "aal2", "aal3" })
            .HasPostgresEnum("auth", "code_challenge_method", new[] { "s256", "plain" })
            .HasPostgresEnum("auth", "factor_status", new[] { "unverified", "verified" })
            .HasPostgresEnum("auth", "factor_type", new[] { "totp", "webauthn", "phone" })
            .HasPostgresEnum("auth", "oauth_authorization_status", new[] { "pending", "approved", "denied", "expired" })
            .HasPostgresEnum("auth", "oauth_client_type", new[] { "public", "confidential" })
            .HasPostgresEnum("auth", "oauth_registration_type", new[] { "dynamic", "manual" })
            .HasPostgresEnum("auth", "oauth_response_type", new[] { "code" })
            .HasPostgresEnum("auth", "one_time_token_type", new[] { "confirmation_token", "reauthentication_token", "recovery_token", "email_change_token_new", "email_change_token_current", "phone_change_token" })
            .HasPostgresEnum("realtime", "action", new[] { "INSERT", "UPDATE", "DELETE", "TRUNCATE", "ERROR" })
            .HasPostgresEnum("realtime", "equality_op", new[] { "eq", "neq", "lt", "lte", "gt", "gte", "in" })
            .HasPostgresEnum("storage", "buckettype", new[] { "STANDARD", "ANALYTICS", "VECTOR" })
            .HasPostgresExtension("extensions", "pg_stat_statements")
            .HasPostgresExtension("extensions", "pgcrypto")
            .HasPostgresExtension("extensions", "uuid-ossp")
            .HasPostgresExtension("postgis")
            .HasPostgresExtension("vault", "supabase_vault");

        modelBuilder.Entity<CadIncident>(entity =>
        {
            entity.HasKey(e => e.IncidentId).HasName("cad_incidents_pkey");

            entity.ToTable("cad_incidents");

            entity.Property(e => e.IncidentId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("incident_id");
            entity.Property(e => e.Call911Time).HasColumnName("call_911_time");
            entity.Property(e => e.EmergencyType)
                .HasMaxLength(255)
                .HasColumnName("emergency_type");
            entity.Property(e => e.GpsCoordinates).HasColumnName("gps_coordinates");
            entity.Property(e => e.IncidentClosureTime).HasColumnName("incident_closure_time");
            entity.Property(e => e.IncidentCode)
                .HasMaxLength(255)
                .HasColumnName("incident_code");
            entity.Property(e => e.PriorityLevel).HasColumnName("priority_level");
        });

        modelBuilder.Entity<DispatchCrew>(entity =>
        {
            entity.HasKey(e => e.CrewId).HasName("dispatch_crews_pkey");

            entity.ToTable("dispatch_crews");

            entity.Property(e => e.CrewId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("crew_id");
            entity.Property(e => e.DispatchId).HasColumnName("dispatch_id");
            entity.Property(e => e.FirefighterId).HasColumnName("firefighter_id");
            entity.Property(e => e.VehiclePosition)
                .HasMaxLength(255)
                .HasColumnName("vehicle_position");

            entity.HasOne(d => d.Dispatch).WithMany(p => p.DispatchCrews)
                .HasForeignKey(d => d.DispatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_crew_dispatch");

            entity.HasOne(d => d.Firefighter).WithMany(p => p.DispatchCrews)
                .HasForeignKey(d => d.FirefighterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_crew_firefighter");
        });

        modelBuilder.Entity<DutyShift>(entity =>
        {
            entity.HasKey(e => e.ShiftId).HasName("duty_shifts_pkey");

            entity.ToTable("duty_shifts");

            entity.Property(e => e.ShiftId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("shift_id");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.FirefighterId).HasColumnName("firefighter_id");
            entity.Property(e => e.HoursWorked)
                .HasPrecision(5, 2)
                .HasColumnName("hours_worked");
            entity.Property(e => e.RolAssigned)
                .HasMaxLength(255)
                .HasColumnName("rol_assigned");
            entity.Property(e => e.StartDate).HasColumnName("start_date");

            entity.HasOne(d => d.Firefighter).WithMany(p => p.DutyShifts)
                .HasForeignKey(d => d.FirefighterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_shifts_firefighter");
        });

        modelBuilder.Entity<FirefighterPersonnel>(entity =>
        {
            entity.HasKey(e => e.FirefighterId).HasName("firefighter_personnel_pkey");

            entity.ToTable("firefighter_personnel");

            entity.Property(e => e.FirefighterId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("firefighter_id");
            entity.Property(e => e.CurrentStatus)
                .HasMaxLength(255)
                .HasColumnName("current_status");
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .HasColumnName("full_name");
            entity.Property(e => e.HireDate).HasColumnName("hire_date");
            entity.Property(e => e.MedicalCertification)
                .HasMaxLength(255)
                .HasColumnName("medical_certification");
            entity.Property(e => e.Rank)
                .HasMaxLength(255)
                .HasColumnName("rank");
            entity.Property(e => e.StationId).HasColumnName("station_id");

            entity.HasOne(d => d.Station).WithMany(p => p.FirefighterPersonnel)
                .HasForeignKey(d => d.StationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_firefighter_station");
        });

        modelBuilder.Entity<OperationalDispatch>(entity =>
        {
            entity.HasKey(e => e.DispatchId).HasName("operational_dispatches_pkey");

            entity.ToTable("operational_dispatches");

            entity.Property(e => e.DispatchId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("dispatch_id");
            entity.Property(e => e.IncidentId).HasColumnName("incident_id");
            entity.Property(e => e.SceneArrivalTime).HasColumnName("scene_arrival_time");
            entity.Property(e => e.StationAlertTime).HasColumnName("station_alert_time");
            entity.Property(e => e.VehicleDepartureTime).HasColumnName("vehicle_departure_time");
            entity.Property(e => e.VehicleId).HasColumnName("vehicle_id");

            entity.HasOne(d => d.Incident).WithMany(p => p.OperationalDispatches)
                .HasForeignKey(d => d.IncidentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_dispatch_incident");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.OperationalDispatches)
                .HasForeignKey(d => d.VehicleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_dispatch_vehicle");
        });

        modelBuilder.Entity<PostIncidentReport>(entity =>
        {
            entity.HasKey(e => e.ReportId).HasName("post_incident_reports_pkey");

            entity.ToTable("post_incident_reports");

            entity.Property(e => e.ReportId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("report_id");
            entity.Property(e => e.IncidentId).HasColumnName("incident_id");
            entity.Property(e => e.InjuriesReported).HasColumnName("injuries_reported");
            entity.Property(e => e.OfficerInChargeId).HasColumnName("officer_in_charge_id");
            entity.Property(e => e.StationReturnTime).HasColumnName("station_return_time");
            entity.Property(e => e.TacticalSummary).HasColumnName("tactical_summary");
            entity.Property(e => e.TotalWaterUsed).HasColumnName("total_water_used");

            entity.HasOne(d => d.Incident).WithMany(p => p.PostIncidentReports)
                .HasForeignKey(d => d.IncidentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_report_incident");

            entity.HasOne(d => d.OfficerInCharge).WithMany(p => p.PostIncidentReports)
                .HasForeignKey(d => d.OfficerInChargeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_report_officer");
        });

        modelBuilder.Entity<PpeEquipment>(entity =>
        {
            entity.HasKey(e => e.EquipmentId).HasName("ppe_equipments_pkey");

            entity.ToTable("ppe_equipments");

            entity.Property(e => e.EquipmentId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("equipment_id");
            entity.Property(e => e.DecontaminationStatus).HasColumnName("decontamination_status");
            entity.Property(e => e.ExpirationDate).HasColumnName("expiration_date");
            entity.Property(e => e.FirefighterId).HasColumnName("firefighter_id");
            entity.Property(e => e.ManufacturingDate).HasColumnName("manufacturing_date");
            entity.Property(e => e.Type)
                .HasMaxLength(255)
                .HasColumnName("type");

            entity.HasOne(d => d.Firefighter).WithMany(p => p.PpeEquipments)
                .HasForeignKey(d => d.FirefighterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ppe_firefighter");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.HasIndex(e => e.RoleName, "roles_role_name_key").IsUnique();

            entity.Property(e => e.RoleId)
                .ValueGeneratedNever()
                .HasColumnName("role_id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(255)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<Station>(entity =>
        {
            entity.HasKey(e => e.StationId).HasName("stations_pkey");

            entity.ToTable("stations");

            entity.Property(e => e.StationId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("station_id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.CoverageZonePolygon).HasColumnName("coverage_zone_polygon");
            entity.Property(e => e.StationNumber).HasColumnName("station_number");
            entity.Property(e => e.VehicleCapacity).HasColumnName("vehicle_capacity");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.FirefighterId, "users_firefighter_id_key").IsUnique();

            entity.HasIndex(e => e.Username, "users_username_key").IsUnique();

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("user_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.FirefighterId).HasColumnName("firefighter_id");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .HasColumnName("username");

            entity.HasOne(d => d.Firefighter).WithOne(p => p.User)
                .HasForeignKey<User>(d => d.FirefighterId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_users_firefighter");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoleId }).HasName("user_roles_pkey");

            entity.ToTable("user_roles");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.AssignedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("assigned_at");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("fk_userroles_role");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_userroles_user");
        });

        modelBuilder.Entity<UserToken>(entity =>
        {
            entity.HasKey(e => e.TokenId).HasName("user_tokens_pkey");

            entity.ToTable("user_tokens");

            entity.Property(e => e.TokenId)
                .ValueGeneratedNever()
                .HasColumnName("token_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");
            entity.Property(e => e.IsRevoked)
                .HasDefaultValue(false)
                .HasColumnName("is_revoked");
            entity.Property(e => e.TokenValue)
                .HasMaxLength(512)
                .HasColumnName("token_value");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.UserTokens)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_usertokens_user");
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.VehicleId).HasName("vehicles_pkey");

            entity.ToTable("vehicles");

            entity.Property(e => e.VehicleId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("vehicle_id");
            entity.Property(e => e.LastMaintenanceDate).HasColumnName("last_maintenance_date");
            entity.Property(e => e.OperationalStatus)
                .HasMaxLength(255)
                .HasColumnName("operational_status");
            entity.Property(e => e.RadioCode)
                .HasMaxLength(255)
                .HasColumnName("radio_code");
            entity.Property(e => e.StationId).HasColumnName("station_id");
            entity.Property(e => e.VehicleType)
                .HasMaxLength(255)
                .HasColumnName("vehicle_type");
            entity.Property(e => e.WaterLevelGallons).HasColumnName("water_level_gallons");

            entity.HasOne(d => d.Station).WithMany(p => p.Vehicles)
                .HasForeignKey(d => d.StationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_vehicles_station");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
