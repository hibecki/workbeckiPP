using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using PPcore.Models;

namespace PPcore.Models
{
    public partial class PalangPanyaDBContext : DbContext
    {
        public PalangPanyaDBContext(DbContextOptions<PalangPanyaDBContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<product>(entity =>
            {
                entity.HasKey(e => e.product_code)
                    .HasName("pk_product");

                entity.Property(e => e.product_code).HasColumnType("varchar(30)");
                //entity.Property(e => e.product_code).HasColumnType("char(3)");
                //entity.Property(e => e.product_code).UseSqlServerIdentityColumn();
                //.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

                entity.Property(e => e.id).HasDefaultValueSql("newid()");

                entity.Property(e => e.product_desc)
                    .IsRequired().HasColumnType("nvarchar(100)");

                entity.Property(e => e.product_group_code).HasColumnType("varchar(30)");

                entity.Property(e => e.product_type_code).HasColumnType("varchar(30)");

                entity.Property(e => e.rowversion)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.x_log).HasColumnType("nvarchar(500)");

                entity.Property(e => e.x_note).HasColumnType("nvarchar(50)");

                entity.Property(e => e.x_status).HasColumnType("char(1)");
            });

            modelBuilder.Entity<product_group>(entity =>
            {
                entity.HasKey(e => e.product_group_code)
                    .HasName("pk_product_group");

                entity.Property(e => e.product_group_code).HasColumnType("varchar(30)");

                entity.Property(e => e.id).HasDefaultValueSql("newid()");

                entity.Property(e => e.product_group_desc)
                    .IsRequired().HasColumnType("nvarchar(100)");

                entity.Property(e => e.rowversion)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate().IsConcurrencyToken();

                entity.Property(e => e.x_log).HasColumnType("nvarchar(500)");

                entity.Property(e => e.x_note).HasColumnType("nvarchar(50)");

                entity.Property(e => e.x_status).HasColumnType("char(1)");
            });

            modelBuilder.Entity<product_type>(entity =>
            {
                entity.HasKey(e => new { e.product_type_code, e.product_group_code })
                    .HasName("pk_product_type");

                entity.Property(e => e.product_type_code).HasColumnType("varchar(30)");

                entity.Property(e => e.product_group_code).HasColumnType("varchar(30)");

                entity.Property(e => e.id).HasDefaultValueSql("newid()");

                entity.Property(e => e.product_type_desc)
                    .IsRequired().HasColumnType("nvarchar(100)");

                entity.Property(e => e.rowversion)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate().IsConcurrencyToken();

                entity.Property(e => e.x_log).HasColumnType("nvarchar(500)");

                entity.Property(e => e.x_note).HasColumnType("nvarchar(50)");

                entity.Property(e => e.x_status).HasColumnType("char(1)");
            });

            modelBuilder.Entity<mem_product>(entity =>
            {
                entity.HasKey(e => new { e.product_code, e.member_code })
                    .HasName("pk_mem_product");

                entity.Property(e => e.product_code).HasColumnType("varchar(30)");

                entity.Property(e => e.member_code).HasColumnType("varchar(30)");

                entity.Property(e => e.grow_area).HasColumnType("decimal");

                entity.Property(e => e.id).HasDefaultValueSql("newid()");

                entity.Property(e => e.rowversion)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.x_log).HasColumnType("nvarchar(500)");

                entity.Property(e => e.x_note).HasColumnType("nvarchar(50)");

                entity.Property(e => e.x_status).HasColumnType("char(1)");
            });

            modelBuilder.Entity<mem_testcenter>(entity =>
            {
                entity.HasKey(e => new { e.mem_testcenter_code })
                    .HasName("pk_mem_testcenter");

                entity.Property(e => e.mem_testcenter_code).HasColumnType("varchar(30)");

                entity.Property(e => e.mem_testcenter_desc).IsRequired().HasColumnType("nvarchar(100)");

                entity.Property(e => e.CreatedBy).HasColumnType("uniqueidentifier");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime").HasDefaultValueSql("getdate()");

                entity.Property(e => e.id).HasDefaultValueSql("newid()");

                entity.Property(e => e.rowversion)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.x_log).HasColumnType("nvarchar(500)");

                entity.Property(e => e.x_note).HasColumnType("nvarchar(50)");

                entity.Property(e => e.x_status).HasColumnType("char(1)");
            });

            modelBuilder.Entity<album>(entity =>
            {
                entity.HasKey(e => e.album_code).HasName("pk_album");

                entity.Property(e => e.album_code).HasColumnType("varchar(30)");

                entity.Property(e => e.album_name).IsRequired().HasColumnType("nvarchar(100)");

                entity.Property(e => e.album_desc).HasColumnType("nvarchar(200)");

                entity.Property(e => e.album_type).HasColumnType("char(1)");

                entity.Property(e => e.created_by).IsRequired().HasColumnType("varchar(30)");

                entity.Property(e => e.album_date)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.id).HasDefaultValueSql("newid()");

                entity.Property(e => e.rowversion)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate().IsConcurrencyToken();

                entity.Property(e => e.x_log).HasColumnType("nvarchar(500)");

                entity.Property(e => e.x_note).HasColumnType("nvarchar(50)");

                entity.Property(e => e.x_status).HasColumnType("char(1)");
            });

            modelBuilder.Entity<course_grade>(entity =>
            {
                entity.HasKey(e => e.cgrade_code)
                    .HasName("pk_course_grade");

                entity.Property(e => e.cgrade_code).HasColumnType("char(1)");

                entity.Property(e => e.cgrade_desc)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.id).HasDefaultValueSql("newid()");

                entity.Property(e => e.rowversion)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate().IsConcurrencyToken();

                entity.Property(e => e.x_log).HasMaxLength(500);

                entity.Property(e => e.x_note).HasMaxLength(50);

                entity.Property(e => e.x_status).HasColumnType("char(1)");
            });

            modelBuilder.Entity<course_group>(entity =>
            {
                //entity.HasKey(e => e.cgroup_code).HasName("pk_course_group");

                entity.Property(e => e.cgroup_code).IsRequired().HasColumnType("varchar(30)");

                entity.Property(e => e.cgroup_desc)
                    .IsRequired()
                    .HasColumnType("nvarchar(100)");

                entity.Property(e => e.id).HasDefaultValueSql("newid()");

                entity.Property(e => e.rowversion)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate().IsConcurrencyToken();

                entity.Property(e => e.x_log).HasMaxLength(500);

                entity.Property(e => e.x_note).HasMaxLength(50);

                entity.Property(e => e.x_status).HasColumnType("char(1)");
            });

            modelBuilder.Entity<course_instructor>(entity =>
            {
                entity.HasKey(e => new { e.instructor_code, e.course_code })
                    .HasName("pk_course_instructor");

                entity.Property(e => e.instructor_code).HasColumnType("varchar(30)");

                entity.Property(e => e.course_code).HasColumnType("varchar(30)");

                entity.Property(e => e.confirm_date).HasColumnType("datetime");

                entity.Property(e => e.id).HasDefaultValueSql("newid()");

                entity.Property(e => e.instructor_cost).HasColumnType("money");

                entity.Property(e => e.ref_doc).HasColumnType("varchar(30)");

                entity.Property(e => e.x_log).HasMaxLength(500);

                entity.Property(e => e.x_note).HasMaxLength(50);

                entity.Property(e => e.x_status).HasColumnType("char(1)");
            });

            modelBuilder.Entity<course_train_place>(entity =>
            {
                entity.HasKey(e => new { e.place_code, e.course_code })
                    .HasName("pk_course_train_place");

                entity.Property(e => e.place_code).HasColumnType("varchar(30)");

                entity.Property(e => e.course_code).HasColumnType("varchar(30)");

                entity.Property(e => e.confirm_date).HasColumnType("datetime");

                entity.Property(e => e.id).HasDefaultValueSql("newid()");

                entity.Property(e => e.place_cost).HasColumnType("money");

                entity.Property(e => e.ref_doc).HasColumnType("varchar(30)");

                entity.Property(e => e.x_log).HasMaxLength(500);

                entity.Property(e => e.x_note).HasMaxLength(50);

                entity.Property(e => e.x_status).HasColumnType("char(1)");
            });

            modelBuilder.Entity<course_type>(entity =>
            {
                entity.HasKey(e => new { e.ctype_code, e.cgroup_code })
                    .HasName("pk_course_type");

                entity.Property(e => e.ctype_code).HasColumnType("varchar(30)");

                entity.Property(e => e.cgroup_code).HasColumnType("varchar(30)");

                entity.Property(e => e.ctype_desc)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.id).HasDefaultValueSql("newid()");

                entity.Property(e => e.rowversion)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate().IsConcurrencyToken();

                entity.Property(e => e.x_log).HasMaxLength(500);

                entity.Property(e => e.x_note).HasMaxLength(50);

                entity.Property(e => e.x_status).HasColumnType("char(1)");
            });

            modelBuilder.Entity<ini_config>(entity =>
            {
                entity.HasKey(e => new { e.client_code, e.system, e.module, e.cnfig_item })
                    .HasName("pk_ini_config");

                entity.Property(e => e.client_code).HasColumnType("varchar(30)");

                entity.Property(e => e.system).HasMaxLength(50);

                entity.Property(e => e.module).HasMaxLength(50);

                entity.Property(e => e.cnfig_item).HasMaxLength(50);

                entity.Property(e => e.id).HasDefaultValueSql("newid()");

                entity.Property(e => e.rowversion)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.text_value)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.x_log).HasMaxLength(500);

                entity.Property(e => e.x_note).HasMaxLength(50);

                entity.Property(e => e.x_status).HasColumnType("char(1)");
            });

            modelBuilder.Entity<ini_country>(entity =>
            {
                entity.HasKey(e => e.country_code)
                    .HasName("pk_ini_country");

                entity.Property(e => e.country_code).ValueGeneratedNever();

                entity.Property(e => e.area_part).HasColumnType("varchar(30)");

                entity.Property(e => e.country_desc)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.id).HasDefaultValueSql("newid()");

                entity.Property(e => e.rowversion)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.x_log).HasMaxLength(500);

                entity.Property(e => e.x_note).HasMaxLength(50);

                entity.Property(e => e.x_status).HasColumnType("char(1)");
            });

            modelBuilder.Entity<ini_district>(entity =>
            {
                entity.HasKey(e => new { e.district_code, e.province_code, e.country_code })
                    .HasName("pk_ini_district");

                entity.Property(e => e.district_code).HasColumnType("char(8)");

                entity.Property(e => e.province_code).HasColumnType("char(8)");

                entity.Property(e => e.area_part).HasColumnType("varchar(30)");

                entity.Property(e => e.dist_desc)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.id).HasDefaultValueSql("newid()");

                entity.Property(e => e.rowversion)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.x_log).HasMaxLength(500);

                entity.Property(e => e.x_note).HasMaxLength(50);

                entity.Property(e => e.x_status).HasColumnType("char(1)");
            });

            modelBuilder.Entity<ini_list_zip>(entity =>
            {
                entity.HasKey(e => new { e.province_code, e.country_code, e.district_code, e.subdistrict_code, e.zip_code })
                    .HasName("pk_ini_list_zip");

                entity.Property(e => e.province_code).HasColumnType("char(8)");

                entity.Property(e => e.district_code).HasColumnType("char(8)");

                entity.Property(e => e.subdistrict_code).HasColumnType("char(8)");

                entity.Property(e => e.zip_code).HasColumnType("char(5)");

                entity.Property(e => e.id).HasDefaultValueSql("newid()");

                entity.Property(e => e.rowversion)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.x_log).HasMaxLength(500);

                entity.Property(e => e.x_note).HasMaxLength(50);

                entity.Property(e => e.x_status).HasColumnType("char(1)");
            });

            modelBuilder.Entity<ini_province>(entity =>
            {
                entity.HasKey(e => new { e.province_code, e.country_code })
                    .HasName("pk_ini_province");

                entity.Property(e => e.province_code).HasColumnType("char(8)");

                entity.Property(e => e.area_part).HasColumnType("varchar(30)");

                entity.Property(e => e.id).HasDefaultValueSql("newid()");

                entity.Property(e => e.pro_desc)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.rowversion)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.x_log).HasMaxLength(500);

                entity.Property(e => e.x_note).HasMaxLength(50);

                entity.Property(e => e.x_status).HasColumnType("char(1)");
            });

            modelBuilder.Entity<ini_subdistrict>(entity =>
            {
                entity.HasKey(e => new { e.province_code, e.country_code, e.district_code, e.subdistrict_code })
                    .HasName("pk_ini_subdistrict");

                entity.Property(e => e.province_code).HasColumnType("char(8)");

                entity.Property(e => e.district_code).HasColumnType("char(8)");

                entity.Property(e => e.subdistrict_code).HasColumnType("char(8)");

                entity.Property(e => e.area_part).HasColumnType("varchar(30)");

                entity.Property(e => e.dist_desc)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.id).HasDefaultValueSql("newid()");

                entity.Property(e => e.rowversion)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.x_log).HasMaxLength(500);

                entity.Property(e => e.x_note).HasMaxLength(50);

                entity.Property(e => e.x_status).HasColumnType("char(1)");
            });

            modelBuilder.Entity<instructor>(entity =>
            {
                entity.HasKey(e => e.instructor_code)
                    .HasName("pk_instructor");

                entity.Property(e => e.instructor_code).HasColumnType("varchar(30)");

                entity.Property(e => e.confirm_date).HasColumnType("datetime");

                entity.Property(e => e.contactor).HasMaxLength(100);

                entity.Property(e => e.contactor_detail).HasMaxLength(500);

                entity.Property(e => e.id).HasDefaultValueSql("newid()");

                entity.Property(e => e.instructor_desc)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ref_doc).HasColumnType("varchar(30)");

                entity.Property(e => e.x_log).HasMaxLength(500);

                entity.Property(e => e.x_note).HasMaxLength(50);

                entity.Property(e => e.x_status).HasColumnType("char(1)");
            });

            modelBuilder.Entity<mem_education>(entity =>
            {
                entity.HasKey(e => new { e.rec_no, e.member_code })
                    .HasName("pk_mem_education");

                entity.Property(e => e.member_code).HasColumnType("varchar(30)");

                entity.Property(e => e.colledge_name).HasMaxLength(500);

                entity.Property(e => e.degree)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.faculty).HasMaxLength(500);

                entity.Property(e => e.id).HasDefaultValueSql("newid()");

                entity.Property(e => e.rowversion)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate().IsConcurrencyToken();

                entity.Property(e => e.x_log).HasMaxLength(500);

                entity.Property(e => e.x_note).HasMaxLength(50);

                entity.Property(e => e.x_status).HasColumnType("char(1)");
            });

            modelBuilder.Entity<mem_group>(entity =>
            {
                entity.HasKey(e => e.mem_group_code)
                    .HasName("pk_mem_group");

                entity.Property(e => e.mem_group_code).HasColumnType("char(3)");

                entity.Property(e => e.id).HasDefaultValueSql("newid()");

                entity.Property(e => e.mem_group_desc)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.rowversion)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.x_log).HasMaxLength(500);

                entity.Property(e => e.x_note).HasMaxLength(50);

                entity.Property(e => e.x_status).HasColumnType("char(1)");
            });

            modelBuilder.Entity<mem_health>(entity =>
            {
                entity.HasKey(e => e.member_code)
                    .HasName("pk_mem_health");

                entity.Property(e => e.member_code).HasColumnType("varchar(30)");

                entity.Property(e => e.blood_group).HasColumnType("char(1)");

                entity.Property(e => e.hobby).HasColumnType("nvarchar(500)");

                entity.Property(e => e.id).HasDefaultValueSql("newid()");

                entity.Property(e => e.medical_history).HasColumnType("nvarchar(500)");

                entity.Property(e => e.restrict_food).HasColumnType("nvarchar(500)");
                entity.Property(e => e.special_food).HasColumnType("nvarchar(500)");

                entity.Property(e => e.rowversion)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate().IsConcurrencyToken();

                entity.Property(e => e.special_skill).HasColumnType("nvarchar(500)");

                entity.Property(e => e.x_log).HasMaxLength(500);

                entity.Property(e => e.x_note).HasMaxLength(50);

                entity.Property(e => e.x_status).HasColumnType("char(1)");
            });

            modelBuilder.Entity<mem_level>(entity =>
            {
                entity.HasKey(e => e.mlevel_code)
                    .HasName("pk_mem_level");

                entity.Property(e => e.mlevel_code).HasColumnType("varchar(30)");

                entity.Property(e => e.id).HasDefaultValueSql("newid()");

                entity.Property(e => e.mlevel_desc)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.rowversion)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.x_log).HasMaxLength(500);

                entity.Property(e => e.x_note).HasMaxLength(50);

                entity.Property(e => e.x_status).HasColumnType("char(1)");
            });

            modelBuilder.Entity<mem_reward>(entity =>
            {
                entity.HasKey(e => new { e.rec_no, e.member_code })
                    .HasName("pk_mem_reward");

                entity.Property(e => e.member_code).HasColumnType("varchar(30)");

                entity.Property(e => e.id).HasDefaultValueSql("newid()");

                entity.Property(e => e.reward_desc)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.rowversion)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate().IsConcurrencyToken();

                entity.Property(e => e.x_log).HasMaxLength(500);

                entity.Property(e => e.x_note).HasMaxLength(50);

                entity.Property(e => e.x_status).HasColumnType("char(1)");
            });

            modelBuilder.Entity<mem_site_visit>(entity =>
            {
                entity.HasKey(e => new { e.rec_no, e.member_code })
                    .HasName("pk_mem_site_visit");

                entity.Property(e => e.member_code).HasColumnType("varchar(30)");

                entity.Property(e => e.id).HasDefaultValueSql("newid()");

                entity.Property(e => e.rowversion)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate().IsConcurrencyToken();

                entity.Property(e => e.site_visit_desc)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.x_log).HasMaxLength(500);

                entity.Property(e => e.x_note).HasMaxLength(50);

                entity.Property(e => e.x_status).HasColumnType("char(1)");
            });

            modelBuilder.Entity<mem_social>(entity =>
            {
                entity.HasKey(e => new { e.rec_no, e.member_code })
                    .HasName("pk_mem_social");

                entity.Property(e => e.member_code).HasColumnType("varchar(30)");

                entity.Property(e => e.id).HasDefaultValueSql("newid()");

                entity.Property(e => e.rowversion)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate().IsConcurrencyToken();

                entity.Property(e => e.social_desc)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.x_log).HasMaxLength(500);

                entity.Property(e => e.x_note).HasMaxLength(50);

                entity.Property(e => e.x_status).HasColumnType("char(1)");
            });

            modelBuilder.Entity<mem_status>(entity =>
            {
                entity.HasKey(e => e.mstatus_code)
                    .HasName("pk_mem_status");

                entity.Property(e => e.mstatus_code).HasColumnType("char(3)");

                entity.Property(e => e.id).HasDefaultValueSql("newid()");

                entity.Property(e => e.mstatus_desc)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.rowversion)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.x_log).HasMaxLength(500);

                entity.Property(e => e.x_note).HasMaxLength(50);

                entity.Property(e => e.x_status).HasColumnType("char(1)");
            });

            modelBuilder.Entity<mem_train_record>(entity =>
            {
                entity.HasKey(e => new { e.course_code, e.member_code }).HasName("pk_mem_train_record");

                entity.Property(e => e.course_code).HasColumnType("varchar(30)");

                entity.Property(e => e.member_code).HasColumnType("varchar(30)");

                entity.Property(e => e.course_grade).HasColumnType("char(1)");
                entity.Property(e => e.course_desc).HasColumnType("nvarchar(100)");

                entity.Property(e => e.id).HasDefaultValueSql("newid()");

                entity.Property(e => e.x_log).HasColumnType("nvarchar(500)");

                entity.Property(e => e.x_note).HasColumnType("nvarchar(50)");

                entity.Property(e => e.x_status).HasColumnType("char(1)");
            });

            modelBuilder.Entity<mem_type>(entity =>
            {
                entity.HasKey(e => new { e.mem_type_code, e.mem_group_code })
                    .HasName("pk_mem_type");

                entity.Property(e => e.mem_type_code).HasColumnType("char(3)");

                entity.Property(e => e.mem_group_code).HasColumnType("char(3)");

                entity.Property(e => e.id).HasDefaultValueSql("newid()");

                entity.Property(e => e.mem_type_desc)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.rowversion)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.x_log).HasMaxLength(500);

                entity.Property(e => e.x_note).HasMaxLength(50);

                entity.Property(e => e.x_status).HasColumnType("char(1)");
            });

            modelBuilder.Entity<mem_worklist>(entity =>
            {
                entity.HasKey(e => e.rec_no)
                    .HasName("pk_mem_worklist");

                entity.Property(e => e.rec_no).ValueGeneratedNever();

                entity.Property(e => e.company_name_eng).HasMaxLength(100);

                entity.Property(e => e.company_name_th).HasMaxLength(100);

                entity.Property(e => e.id).HasDefaultValueSql("newid()");

                entity.Property(e => e.member_code).HasColumnType("varchar(30)");

                entity.Property(e => e.office_address).HasMaxLength(500);

                entity.Property(e => e.position_name_eng).HasMaxLength(100);

                entity.Property(e => e.position_name_th).HasMaxLength(100);

                entity.Property(e => e.rowversion)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate().IsConcurrencyToken();

                entity.Property(e => e.work_year).HasColumnType("varchar(9)");

                entity.Property(e => e.x_log).HasMaxLength(500);

                entity.Property(e => e.x_note).HasMaxLength(50);

                entity.Property(e => e.x_status).HasColumnType("char(1)");
            });

            modelBuilder.Entity<member>(entity =>
            {
                entity.Property(e => e.mem_password).HasColumnType("varchar(40)");
                entity.Property(e => e.mem_username).HasColumnType("varchar(50)");

                entity.HasKey(e => e.member_code)
                    .HasName("pk_member");

                entity.Property(e => e.member_code).HasColumnType("varchar(30)");

                entity.Property(e => e.birthdate).HasColumnType("datetime");

                entity.Property(e => e.building).HasColumnType("nvarchar(50)");

                entity.Property(e => e.cid_card).HasColumnType("varchar(30)");

                entity.Property(e => e.cid_card_pic).HasColumnType("varchar(30)");

                entity.Property(e => e.income).HasColumnType("char(1)");

                entity.Property(e => e.district_code).HasColumnType("char(8)");

                entity.Property(e => e.email).HasColumnType("nvarchar(100)");

                entity.Property(e => e.fax).HasColumnType("nvarchar(50)");

                entity.Property(e => e.floor).HasColumnType("nvarchar(20)");

                entity.Property(e => e.title).HasColumnType("nvarchar(100)");
                entity.Property(e => e.fname).HasColumnType("nvarchar(100)");
                entity.Property(e => e.lname).HasColumnType("nvarchar(100)");
                entity.Property(e => e.occupation).HasColumnType("nvarchar(100)");

                entity.Property(e => e.h_no).HasColumnType("nvarchar(20)");

                entity.Property(e => e.id).HasDefaultValueSql("newid()");

                entity.Property(e => e.lane).HasColumnType("nvarchar(50)");

                entity.Property(e => e.latitude).HasColumnType("decimal");

                

                entity.Property(e => e.longitude).HasColumnType("decimal");

                entity.Property(e => e.lot_no).HasColumnType("nvarchar(20)");

                entity.Property(e => e.marry_status).HasColumnType("char(1)");

                entity.Property(e => e.mem_group_code).HasColumnType("char(3)");

                entity.Property(e => e.mem_photo).HasColumnType("varchar(30)");

                entity.Property(e => e.mem_type_code).HasColumnType("char(3)");

                entity.Property(e => e.mlevel_code).HasColumnType("varchar(30)");

                entity.Property(e => e.mobile).HasColumnType("nvarchar(50)");

                entity.Property(e => e.mstatus_code).HasColumnType("char(3)");

                entity.Property(e => e.nationality).HasColumnType("nvarchar(30)");

                entity.Property(e => e.parent_code).HasColumnType("varchar(30)");

                entity.Property(e => e.place_name).HasColumnType("nvarchar(50)");

                entity.Property(e => e.province_code).HasColumnType("char(8)");

                entity.Property(e => e.religion).HasColumnType("nvarchar(30)");

                entity.Property(e => e.room).HasColumnType("nvarchar(20)");

                entity.Property(e => e.rowversion)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate().IsConcurrencyToken();

                entity.Property(e => e.sex).HasColumnType("char(1)");
                
                entity.Property(e => e.facebook).HasColumnType("nvarchar(500)");
                entity.Property(e => e.line).HasColumnType("nvarchar(500)");

                entity.Property(e => e.street).HasColumnType("nvarchar(50)");

                entity.Property(e => e.subdistrict_code).HasColumnType("char(8)");

                entity.Property(e => e.tel).HasColumnType("nvarchar(50)");

                entity.Property(e => e.texta_address).HasColumnType("nvarchar(200)");

                entity.Property(e => e.textb_address).HasColumnType("nvarchar(200)");

                entity.Property(e => e.textc_address).HasColumnType("nvarchar(200)");
                entity.Property(e => e.mail_address).HasColumnType("nvarchar(600)");

                entity.Property(e => e.village).HasColumnType("nvarchar(50)");

                entity.Property(e => e.x_log).HasColumnType("nvarchar(500)");

                entity.Property(e => e.x_note).HasColumnType("nvarchar(50)");

                entity.Property(e => e.x_status).HasColumnType("char(1)");

                entity.Property(e => e.zip_code).HasColumnType("char(5)");

                entity.Property(e => e.zone).HasColumnType("nvarchar(30)");
            });

            modelBuilder.Entity<pic_image>(entity =>
            {
                entity.HasKey(e => e.image_code)
                    .HasName("pk_pic_image");

                entity.Property(e => e.image_code).HasColumnType("varchar(30)");

                entity.Property(e => e.image_file).HasColumnType("text");

                entity.Property(e => e.image_name).HasColumnType("nvarchar(50)");
                entity.Property(e => e.image_desc).HasColumnType("nvarchar(200)");

                entity.Property(e => e.ref_doc_code).HasColumnType("varchar(30)");

                entity.Property(e => e.ref_doc_type).HasColumnType("varchar(30)");

                entity.Property(e => e.x_log).HasColumnType("nvarchar(500)");

                entity.Property(e => e.x_note).HasColumnType("nvarchar(50)");

                entity.Property(e => e.x_status).HasColumnType("char(1)");
            });

            modelBuilder.Entity<project>(entity =>
            {
                entity.HasKey(e => e.project_code)
                    .HasName("pk_project");

                entity.Property(e => e.project_code).HasColumnType("varchar(30)");

                entity.Property(e => e.budget).HasColumnType("money");

                entity.Property(e => e.id).HasDefaultValueSql("newid()");

                entity.Property(e => e.project_approve_date).HasColumnType("datetime");

                entity.Property(e => e.project_date).HasColumnType("datetime");

                entity.Property(e => e.project_desc)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.project_manager).HasMaxLength(100);

                entity.Property(e => e.ref_doc).HasColumnType("varchar(30)");

                entity.Property(e => e.x_log).HasMaxLength(500);

                entity.Property(e => e.x_note).HasMaxLength(50);

                entity.Property(e => e.x_status).HasColumnType("char(1)");
            });

            modelBuilder.Entity<project_course>(entity =>
            {
                entity.HasKey(e => e.course_code)
                    .HasName("pk_project_course");

                entity.Property(e => e.course_code).HasColumnType("varchar(30)");

                entity.Property(e => e.budget).HasColumnType("money");

                entity.Property(e => e.cgroup_code).HasColumnType("varchar(30)");

                entity.Property(e => e.charge_head).HasColumnType("money");

                entity.Property(e => e.course_approve_date).HasColumnType("datetime");

                entity.Property(e => e.course_begin).HasColumnType("datetime");

                entity.Property(e => e.course_date).HasColumnType("datetime");

                entity.Property(e => e.course_desc)
                    .IsRequired()
                    .HasColumnType("nvarchar(100)");

                entity.Property(e => e.course_end).HasColumnType("datetime");

                entity.Property(e => e.ctype_code).HasColumnType("varchar(30)");

                entity.Property(e => e.id).HasDefaultValueSql("newid()");

                entity.Property(e => e.project_code).HasColumnType("varchar(30)");

                entity.Property(e => e.project_manager).HasColumnType("nvarchar(100)");

                entity.Property(e => e.ref_doc).HasColumnType("varchar(30)");

                entity.Property(e => e.support_head).HasColumnType("money");

                entity.Property(e => e.x_log).HasColumnType("nvarchar(500)");

                entity.Property(e => e.x_note).HasColumnType("nvarchar(50)");

                entity.Property(e => e.x_status).HasColumnType("char(1)");
            });

            modelBuilder.Entity<project_course_register>(entity =>
            {
                entity.HasKey(e => new { e.course_code, e.member_code })
                    .HasName("pk_project_course_register");

                entity.Property(e => e.course_code).HasColumnType("varchar(30)");

                entity.Property(e => e.member_code).HasColumnType("varchar(30)");

                entity.Property(e => e.id).HasDefaultValueSql("newid()");

                entity.Property(e => e.x_log).HasMaxLength(500);

                entity.Property(e => e.x_note).HasMaxLength(50);

                entity.Property(e => e.x_status).HasColumnType("char(1)");
            });

            modelBuilder.Entity<project_daily_checklist>(entity =>
            {
                entity.HasKey(e => new { e.course_date, e.course_code, e.member_code })
                    .HasName("pk_project_daily_checklist");

                entity.Property(e => e.course_date).HasColumnType("datetime");

                entity.Property(e => e.course_code).HasColumnType("varchar(30)");

                entity.Property(e => e.member_code).HasColumnType("varchar(30)");

                entity.Property(e => e.id).HasDefaultValueSql("newid()");

                entity.Property(e => e.x_log).HasMaxLength(500);

                entity.Property(e => e.x_note).HasMaxLength(50);

                entity.Property(e => e.x_status).HasColumnType("char(1)");
            });

            modelBuilder.Entity<project_sponsor>(entity =>
            {
                entity.HasKey(e => e.spon_code)
                    .HasName("pk_project_sponsor");

                entity.Property(e => e.spon_code).HasColumnType("varchar(30)");

                entity.Property(e => e.confirm_date).HasColumnType("datetime");

                entity.Property(e => e.contactor).HasMaxLength(100);

                entity.Property(e => e.contactor_detail).HasMaxLength(500);

                entity.Property(e => e.id).HasDefaultValueSql("newid()");

                entity.Property(e => e.ref_doc).HasColumnType("varchar(30)");

                entity.Property(e => e.spon_desc)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.x_log).HasMaxLength(500);

                entity.Property(e => e.x_note).HasMaxLength(50);

                entity.Property(e => e.x_status).HasColumnType("char(1)");
            });

            modelBuilder.Entity<project_supporter>(entity =>
            {
                entity.HasKey(e => new { e.spon_code, e.project_code })
                    .HasName("pk_project_supporter");

                entity.Property(e => e.spon_code).HasColumnType("varchar(30)");

                entity.Property(e => e.project_code).HasColumnType("varchar(30)");

                entity.Property(e => e.contactor).HasMaxLength(100);

                entity.Property(e => e.contactor_detail).HasMaxLength(500);

                entity.Property(e => e.id).HasDefaultValueSql("newid()");

                entity.Property(e => e.ref_doc).HasColumnType("varchar(30)");

                entity.Property(e => e.support_budget).HasColumnType("money");

                entity.Property(e => e.x_log).HasMaxLength(500);

                entity.Property(e => e.x_note).HasMaxLength(50);

                entity.Property(e => e.x_status).HasColumnType("char(1)");
            });

            modelBuilder.Entity<train_place>(entity =>
            {
                entity.HasKey(e => e.place_code)
                    .HasName("pk_train_place");

                entity.Property(e => e.place_code).HasColumnType("varchar(30)");

                entity.Property(e => e.confirm_date).HasColumnType("datetime");

                entity.Property(e => e.contactor).HasMaxLength(100);

                entity.Property(e => e.contactor_detail).HasMaxLength(500);

                entity.Property(e => e.id).HasDefaultValueSql("newid()");

                entity.Property(e => e.place_desc)
                    .IsRequired()
                    .HasColumnType("nvarchar(100)");

                entity.Property(e => e.ref_doc).HasColumnType("varchar(30)");

                entity.Property(e => e.x_log).HasMaxLength(500);

                entity.Property(e => e.x_note).HasMaxLength(50);

                entity.Property(e => e.x_status).HasColumnType("char(1)");
            });
        }
        public virtual DbSet<product> product { get; set; }
        public virtual DbSet<product_group> product_group { get; set; }
        public virtual DbSet<product_type> product_type { get; set; }
        public virtual DbSet<mem_product> mem_product { get; set; }

        public virtual DbSet<course_grade> course_grade { get; set; }
        public virtual DbSet<course_group> course_group { get; set; }
        public virtual DbSet<course_instructor> course_instructor { get; set; }
        public virtual DbSet<course_train_place> course_train_place { get; set; }
        public virtual DbSet<course_type> course_type { get; set; }
        public virtual DbSet<ini_config> ini_config { get; set; }
        public virtual DbSet<ini_country> ini_country { get; set; }
        public virtual DbSet<ini_district> ini_district { get; set; }
        public virtual DbSet<ini_list_zip> ini_list_zip { get; set; }
        public virtual DbSet<ini_province> ini_province { get; set; }
        public virtual DbSet<ini_subdistrict> ini_subdistrict { get; set; }
        public virtual DbSet<instructor> instructor { get; set; }
        public virtual DbSet<mem_education> mem_education { get; set; }
        public virtual DbSet<mem_group> mem_group { get; set; }
        public virtual DbSet<mem_health> mem_health { get; set; }
        public virtual DbSet<mem_level> mem_level { get; set; }
        public virtual DbSet<mem_reward> mem_reward { get; set; }
        public virtual DbSet<mem_site_visit> mem_site_visit { get; set; }
        public virtual DbSet<mem_social> mem_social { get; set; }
        public virtual DbSet<mem_status> mem_status { get; set; }
        public virtual DbSet<mem_train_record> mem_train_record { get; set; }
        public virtual DbSet<mem_type> mem_type { get; set; }
        public virtual DbSet<mem_worklist> mem_worklist { get; set; }
        public virtual DbSet<member> member { get; set; }
        public virtual DbSet<pic_image> pic_image { get; set; }
        public virtual DbSet<project> project { get; set; }
        public virtual DbSet<project_course> project_course { get; set; }
        public virtual DbSet<project_course_register> project_course_register { get; set; }
        public virtual DbSet<project_daily_checklist> project_daily_checklist { get; set; }
        public virtual DbSet<project_sponsor> project_sponsor { get; set; }
        public virtual DbSet<project_supporter> project_supporter { get; set; }
        public virtual DbSet<train_place> train_place { get; set; }
        public virtual DbSet<album> album { get; set; }
        public DbSet<mem_testcenter> mem_testcenter { get; set; }
    }
}