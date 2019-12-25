using Microsoft.EntityFrameworkCore;

namespace CORTNE.Models
{
    public partial class CORTNEDBContext : DbContext
    {
        public CORTNEDBContext()
        {
        }

        public CORTNEDBContext(DbContextOptions<CORTNEDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblUser> TblUser { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DIT00MLT045313\\MSSQLSERVER02;Database=WorkpulseDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblUser>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(50);
            });
        }
    }
}
