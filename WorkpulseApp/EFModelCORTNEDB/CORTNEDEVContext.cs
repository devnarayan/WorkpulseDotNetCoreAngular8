using CORTNE.EFModelCORTNEDB;
using Microsoft.EntityFrameworkCore;

namespace CORTNE.Models
{
    public partial class CORTNEDEVContext : DbContext
    {
        public CORTNEDEVContext()
        {
        }

        public CORTNEDEVContext(DbContextOptions<CORTNEDEVContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AppException> AppException { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<TimeLog> TimeLog { get; set; }
        public virtual DbSet<UserTokenCach> UserTokenCach { get; set; }
        public virtual DbSet<LocationLk> LocationLk { get; set; }
        public virtual DbSet<TemplateNotice> TemplateNotice { get; set; }
        public virtual DbSet<USStates> USStates { get; set; }
        public virtual DbSet<ApplicationConfigurationLk> ApplicationConfigurationLk { get; set; }
        public virtual DbSet<UserManagementAuditHistory> UserManagementAuditHistories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("data source=DevNagarServer;initial catalog=devnagar-dev;user id=username;password=pwd;multipleactiveresultsets=True;application name=EntityFramework");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppException>(entity =>
            {
                entity.Property(e => e.Controller)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ErrorDate).HasColumnType("datetime");

                entity.Property(e => e.ExceptionMessage)
                    .HasColumnName("exceptionMessage")
                    .HasMaxLength(4000)
                    .IsUnicode(false);

                entity.Property(e => e.ExceptionType)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Method)
                    .HasColumnName("method")
                    .HasMaxLength(4000)
                    .IsUnicode(false);

                entity.Property(e => e.Stacktrace)
                    .HasColumnName("stacktrace")
                    .HasMaxLength(4000)
                    .IsUnicode(false);
            });
            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.Groups)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(128);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey, e.UserId });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId).HasMaxLength(128);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.Property(e => e.UserId).HasMaxLength(128);

                entity.Property(e => e.RoleId).HasMaxLength(128);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.LockoutEndDateUtc).HasColumnType("datetime");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<TimeLog>(entity =>
            {
                entity.Property(e => e.LogDate).HasColumnType("date");

                entity.Property(e => e.TimeIn1).HasColumnType("time(4)");

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<UserTokenCach>(entity =>
            {
                entity.HasKey(e => e.UserTokenCacheId);

                entity.Property(e => e.CacheBits).HasColumnName("cacheBits");

                entity.Property(e => e.LastWrite).HasColumnType("datetime");

                entity.Property(e => e.WebUserUniqueId).HasColumnName("webUserUniqueId");
            });

            


        
            modelBuilder.Entity<LocationLk>(entity =>
            {
                entity.HasKey(e => e.LocationId);

                entity.ToTable("Location_LK");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.Flaircode)
                    .HasColumnName("FLAIRCode")
                    .HasMaxLength(256);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.OrgCode).HasMaxLength(50);

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });
            modelBuilder.Entity<TemplateNotice>(entity =>
            {
                entity.ToTable("Template_Notice");

                entity.Property(e => e.AccManagerName)
                    .HasColumnName("Acc_Manager_Name")
                    .HasMaxLength(100);

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Emailid).HasMaxLength(50);

                entity.Property(e => e.GovernorName).HasMaxLength(50);

                entity.Property(e => e.OfficeAddress).HasColumnName("Office_Address");

                entity.Property(e => e.SurgeonName).HasMaxLength(50);

                entity.Property(e => e.Vision).HasMaxLength(100);

                entity.Property(e => e.WorkedDate).HasColumnType("datetime");

                entity.Property(e => e.WrokedBy).HasMaxLength(50);
            });
            modelBuilder.Entity<USStates>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("USStates_LK");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasColumnName("Name");

                entity.Property(e => e.AlphaCode).HasColumnName("AlphaCode");

                entity.Property(e => e.FIPSCode).HasColumnName("FIPSCode");
               
            });
            modelBuilder.Entity<ApplicationConfigurationLk>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.ToTable("ApplicationConfiguration_LK");

                entity.Property(e => e.ID).HasColumnName("ID");

                entity.Property(e => e.Area).HasColumnName("Area")
                    .HasMaxLength(10);

                entity.Property(e => e.Description).HasColumnName("Description")
                    .HasMaxLength(128);

                entity.Property(e => e.Value).HasColumnName("Value")
                    .HasMaxLength(128);

                entity.Property(e => e.UpdatedBy).HasColumnName("UpdatedBy")
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnName("UpdatedDate").HasColumnType("datetime");

                entity.Property(e => e.IsDeleted).HasColumnName("IsDeleted");

                entity.Property(e => e.InCode_Lookup).HasColumnName("InCode_Lookup")
                    .HasMaxLength(30);

            });

            modelBuilder.Entity<UserManagementAuditHistory>(entity =>
            {
                entity.ToTable("UserManagement_AuditHistory");

                entity.HasKey(e => e.RowId);

                entity.Property(e => e.UserName)
                    .HasColumnName("UserName")
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("UpdatedBy")
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.UpdatedDate)
                    .HasColumnName("UpdatedDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.RoleId)
                    .HasColumnName("RoleId")
                    .HasMaxLength(128);

                entity.Property(e => e.ActionType)
                    .HasColumnName("ActionType")
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.LocationCode)
                    .HasColumnName("LocationCode")
                    .HasMaxLength(15);

            });
        }
    }
}
