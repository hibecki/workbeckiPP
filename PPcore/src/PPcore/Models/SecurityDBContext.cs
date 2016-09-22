using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using PPcore.Models;

namespace PPcore.Models
{
    public partial class SecurityDBContext : DbContext
    {
        public SecurityDBContext(DbContextOptions<SecurityDBContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SecurityMemberRoles>(entity =>
            {
                entity.HasKey(e => e.MemberId).HasName("PK_SecurityMemberRoles");
                entity.Property(e => e.MemberId).HasColumnType("uniqueidentifier");

                entity.Property(e => e.CreatedBy).HasColumnType("uniqueidentifier");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.EditedBy).HasColumnType("uniqueidentifier");
                entity.Property(e => e.EditedDate).HasColumnType("datetime");

                entity.Property(e => e.LoggedInDate).HasColumnType("datetime");
                entity.Property(e => e.LoggedOutDate).HasColumnType("datetime");

                entity.Property(e => e.x_log).HasColumnType("nvarchar(500)");
                entity.Property(e => e.x_note).HasColumnType("nvarchar(50)");
                entity.Property(e => e.x_status).HasColumnType("char(1)");
            });

            modelBuilder.Entity<SecurityRoles>(entity =>
            {
                entity.HasKey(e => new { e.RoleId }).HasName("PK_SecurityRoles");
                entity.Property(e => e.RoleId).HasDefaultValueSql("newid()");
                entity.Property(e => e.RoleName).HasColumnType("nvarchar(100)");

                entity.Property(e => e.CreatedBy).HasColumnType("uniqueidentifier");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime").HasDefaultValueSql("getdate()");
                entity.Property(e => e.EditedBy).HasColumnType("uniqueidentifier");
                entity.Property(e => e.EditedDate).HasColumnType("datetime").HasDefaultValueSql("getdate()");

                entity.Property(e => e.x_log).HasColumnType("nvarchar(500)");
                entity.Property(e => e.x_note).HasColumnType("nvarchar(50)");
                entity.Property(e => e.x_status).HasColumnType("char(1)");
            });

            modelBuilder.Entity<SecurityRoleMenus>(entity =>
            {
                entity.HasKey(e => new { e.MenuId, e.RoleId }).HasName("PK_SecurityRoleMenus");
                entity.Property(e => e.RoleId).HasColumnType("uniqueidentifier");
                entity.Property(e => e.MenuId).HasColumnType("int");

                entity.Property(e => e.EditedBy).HasColumnType("uniqueidentifier");
                entity.Property(e => e.EditedDate).HasColumnType("datetime").HasDefaultValueSql("getdate()");
            });

            modelBuilder.Entity<SecurityMenus>(entity =>
            {
                entity.HasKey(e => new { e.MenuId }).HasName("PK_SecurityMenus");
                entity.Property(e => e.MenuId).HasColumnType("int");

                entity.Property(e => e.Level).HasColumnType("int");
                entity.Property(e => e.HaveChild).HasColumnType("int");
                entity.Property(e => e.IsRightAlign).HasColumnType("int");


                entity.Property(e => e.MenuName).HasColumnType("nvarchar(100)");
                entity.Property(e => e.MenuDisplay).HasColumnType("nvarchar(100)");

                entity.Property(e => e.MenuController).HasColumnType("varchar(100)");
                entity.Property(e => e.MenuAction).HasColumnType("varchar(100)");

                entity.Property(e => e.MenuUrl).HasColumnType("varchar(200)");
            });
        }
        public virtual DbSet<SecurityMemberRoles> SecurityMemberRoles { get; set; }
        public virtual DbSet<SecurityRoles> SecurityRoles { get; set; }
        public virtual DbSet<SecurityRoleMenus> SecurityRoleMenus { get; set; }
        public virtual DbSet<SecurityMenus> SecurityMenus { get; set; }
    }
}