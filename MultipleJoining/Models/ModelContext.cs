using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace MultipleJoining.Models
{
    public partial class ModelContext : DbContext
    {
        public ModelContext()
        {
        }

        public ModelContext(DbContextOptions<ModelContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Exam> Exams { get; set; }
        public virtual DbSet<Month> Months { get; set; }
        public virtual DbSet<Student> Students { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseOracle("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=XEPDB1)));Persist Security Info=True;User Id=projectadmin1;Password=oracle;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("PROJECTADMIN1")
                .HasAnnotation("Relational:Collation", "USING_NLS_COMP");

            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(e => e.CountryId);

                entity.ToTable("COUNTRY");

                entity.Property(e => e.CountryId)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("COUNTRY_ID");

                entity.Property(e => e.CountryName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("COUNTRY_NAME");
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(e => e.Crid);

                entity.ToTable("COURSE");

                entity.Property(e => e.Crid)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("CRID");

                entity.Property(e => e.Crname)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CRNAME");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(e => e.Did);

                entity.ToTable("DEPARTMENT");

                entity.Property(e => e.Did)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("DID");

                entity.Property(e => e.Dname)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DNAME");

                entity.Property(e => e.Exid)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("EXID");
            });

            modelBuilder.Entity<Exam>(entity =>
            {
                entity.HasKey(e => e.Exid);

                entity.ToTable("EXAM");

                entity.Property(e => e.Exid)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("EXID");

                entity.Property(e => e.Exname)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("EXNAME");

                entity.Property(e => e.Mid)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("MID");
            });

            modelBuilder.Entity<Month>(entity =>
            {
                entity.HasKey(e => e.Mid);

                entity.ToTable("month");

                entity.Property(e => e.Crid)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("CRID");

                entity.Property(e => e.Mid)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("MID");

                entity.Property(e => e.Mname)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MNAME");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.Sid);

                entity.ToTable("STUDENT");

                entity.Property(e => e.Did)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("DID");

                entity.Property(e => e.Sid)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("SID");

                entity.Property(e => e.Sname)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("SNAME");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
