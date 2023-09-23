using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using UserAuthentication.Entities;
using UserAuthentication.Models;

namespace UserAuthentication.Data
{
    public partial class UserAuthenticationContext : DbContext
    {      
            public UserAuthenticationContext()
            {
            }

            public UserAuthenticationContext(DbContextOptions<UserAuthenticationContext> options)
                : base(options)
            {
            }

            public virtual DbSet<LoginTable> LoginTables { get; set; } = null!;
            public virtual DbSet<RegistrationTable> RegistrationTables { get; set; } = null!;

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                if (!optionsBuilder.IsConfigured)
                {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                    optionsBuilder.UseSqlServer("Server=DESKTOP-KBIRKVA;User=Assessment;password=123456;Database=UserAuthentication;Trusted_Connection=True;");
                }
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<LoginTable>(entity =>
                {
                    entity.ToTable("LoginTable");

                    entity.Property(e => e.LoginExpireTime)
                        .HasColumnType("datetime")
                        .HasColumnName("Login_ExpireTime");

                    entity.Property(e => e.LoginMethod)
                        .HasMaxLength(50)
                        .HasColumnName("Login_Method");

                    entity.Property(e => e.LoginTime)
                        .HasColumnType("datetime")
                        .HasColumnName("Login_Time");

                    entity.Property(e => e.Status).HasMaxLength(50);

                    entity.Property(e => e.StatusCode).HasColumnName("Status_Code");
                });

                modelBuilder.Entity<RegistrationTable>(entity =>
                {
                    entity.ToTable("RegistrationTable");

                    entity.Property(e => e.DateCreated).HasColumnType("datetime");

                    entity.Property(e => e.UserName).HasMaxLength(100);
                });

                OnModelCreatingPartial(modelBuilder);
            }

            partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
        }
    
    }
