using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace MeterAPI.DAL.Entities
{
    public partial class MeterDBContext : DbContext
    {
        public string DbPath { get; private set; }
        public MeterDBContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);

            DbPath = $"./MeterDB.db";
        }

        public MeterDBContext(DbContextOptions<MeterDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<MeterReadingEntity> MeterReadings { get; set; }
        public virtual DbSet<TestAccountEntity> TestAccounts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite($"DataSource={DbPath}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MeterReadingEntity>(entity =>
            {
                entity.Property(e => e.MeterReadingId).HasColumnName("meter_reading_id");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.Reading).HasColumnName("reading");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.MeterReadings)
                    .HasForeignKey(d => d.AccountId);
            });

            modelBuilder.Entity<TestAccountEntity>(entity =>
            {
                entity.HasKey(e => e.AccountId);

                entity.ToTable("test_accounts");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.Email).HasColumnName("email");

                entity.Property(e => e.FirstName).HasColumnName("first_name");

                entity.Property(e => e.Surname).HasColumnName("surname");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
