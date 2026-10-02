using System;
using System.Collections.Generic;
using JWTAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace JWTAPI.Context;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Clothe> Clothes { get; set; }

    public virtual DbSet<ClothesSize> ClothesSizes { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderClothe> OrderClothes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Size> Sizes { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<TypeClothe> TypeClothes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Clothes;Username=postgres;Password=postgres");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Clothe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Clothes_pkey");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.IsAvailable).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Price).HasPrecision(12, 2);
            entity.Property(e => e.TypeClothesId).HasColumnName("TypeClothesID");

            entity.HasOne(d => d.TypeClothes).WithMany(p => p.Clothes)
                .HasForeignKey(d => d.TypeClothesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Clothes_TypeClothesID_fkey");
        });

        modelBuilder.Entity<ClothesSize>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ClothesSize_pkey");

            entity.ToTable("ClothesSize");

            entity.HasIndex(e => new { e.ClothesId, e.SizeId }, "ClothesSize_ClothesID_SizeID_key").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.ClothesId).HasColumnName("ClothesID");
            entity.Property(e => e.SizeId).HasColumnName("SizeID");

            entity.HasOne(d => d.Clothes).WithMany(p => p.ClothesSizes)
                .HasForeignKey(d => d.ClothesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ClothesSize_ClothesID_fkey");

            entity.HasOne(d => d.Size).WithMany(p => p.ClothesSizes)
                .HasForeignKey(d => d.SizeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ClothesSize_SizeID_fkey");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Order_pkey");

            entity.ToTable("Order");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Status)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Order_Status_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Order_UserID_fkey");
        });

        modelBuilder.Entity<OrderClothe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("OrderClothes_pkey");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.ClothesId).HasColumnName("ClothesID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");

            entity.HasOne(d => d.Clothes).WithMany(p => p.OrderClothes)
                .HasForeignKey(d => d.ClothesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OrderClothes_ClothesID_fkey");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderClothes)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OrderClothes_OrderID_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Role_pkey");

            entity.ToTable("Role");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Role1)
                .HasMaxLength(100)
                .HasColumnName("Role");
        });

        modelBuilder.Entity<Size>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Size_pkey");

            entity.ToTable("Size");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Size1)
                .HasMaxLength(5)
                .HasColumnName("Size");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Status_pkey");

            entity.ToTable("Status");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Status1)
                .HasMaxLength(100)
                .HasColumnName("Status");
        });

        modelBuilder.Entity<TypeClothe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TypeClothes_pkey");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Type).HasMaxLength(100);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("User_pkey");

            entity.ToTable("User");

            entity.HasIndex(e => e.Login, "User_Login_key").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Login).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(20);

            entity.HasOne(d => d.RoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.Role)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("User_Role_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
