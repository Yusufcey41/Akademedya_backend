using akademedya_backend.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace akademedya_backend.Data.Context
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public ApplicationDbContext()
        {
        }

        public DbSet<UsersInformation> UsersInformations { get; set; }
        public DbSet<TableInformations> TableInformations { get; set; }
        public DbSet<UserTables> UserTables { get; set; }
        public DbSet<Columns> Columns { get; set; }
        public DbSet<TablesValues> TablesValues { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsbuilder)
        {
            if (!optionsbuilder.IsConfigured)
            {
                optionsbuilder.UseSqlServer("Data Source=DESKTOP-2U209NF\\SQLEXPRESS;Database=Akademedya ;Integrated Security=True;Trust Server Certificate=True");
            }
            base.OnConfiguring(optionsbuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");

            modelBuilder.Entity<UsersInformation>(entity =>
            {
                entity.ToTable("user_informations");

                entity.HasKey(i => i.UserId);

                entity.Property(i => i.UserId).HasColumnName("user_id").UseIdentityColumn().IsRequired();

                entity.Property(i => i.Firstname).HasColumnName("first_name").HasColumnType("nvarchar(50)").IsRequired();

                entity.Property(i => i.Lastname).HasColumnName("last_name").HasColumnType("nvarchar(50)").IsRequired();

                entity.Property(i => i.Email).HasColumnName("email").HasColumnType("nvarchar(255)").IsRequired();

                entity.Property(i => i.Password).HasColumnName("password").HasColumnType("nvarchar(20)").IsRequired();

                entity.Property(i => i.isActive).HasColumnName("active").HasColumnType("bit").IsRequired();

               
                entity.HasMany(i => i.UserTables).WithOne(i => i.UserInformation).HasForeignKey(i => i.UserId).HasConstraintName("fk_user_id");


            });

            modelBuilder.Entity<TableInformations>(entity =>
            {
                entity.ToTable("table_informations");

                entity.HasKey(i => i.TableId);

                entity.Property(i => i.TableId).HasColumnName("table_id").HasColumnType("int").UseIdentityColumn().IsRequired();

                entity.Property(i => i.TableName).HasColumnName("table_name").HasColumnType("nvarchar(50)").IsRequired();

                entity.Property(i => i.ImageUrl).HasColumnName("table_image").HasColumnType("nvarchar(255)");

                entity.HasOne(i => i.UserTables).WithOne(i => i.TableInformations).HasForeignKey<UserTables>(i => i.TableId).HasConstraintName("fk_table_id");

                entity.HasMany(i => i.Columns).WithOne( i => i.TableInformations).HasForeignKey(i => i.TableId).HasConstraintName("fk_columntable_id");

                entity.HasMany(i => i.TablesValue).WithOne(i => i.TableInformations).HasForeignKey(i => i.TableId).HasConstraintName("fk_valuetable_id");

            });

            modelBuilder.Entity<UserTables>(entity =>
            {
                entity.HasKey(i => new { i.TableId, i.UserId });
                

                entity.Property(i => i.UserId).HasColumnName("user_id").HasColumnType("int").IsRequired();

                entity.Property(i => i.TableId).HasColumnName("table_id").HasColumnType("int").IsRequired();
            });

            modelBuilder.Entity<Columns>(entity =>
            {
                entity.HasKey(i => new { i.TableId, i.ColumnId });
               
                entity.Property(i => i.TableId).HasColumnName("table_id").HasColumnType("int").IsRequired();

                entity.Property(i => i.ColumnId).HasColumnName ("column_id").HasColumnType("int").IsRequired();

                entity.Property(i => i.ColumnName).HasColumnName("column_name").HasColumnType("nvarchar(50)").IsRequired();


            });

            modelBuilder.Entity<TablesValues>(entity =>
            {
                entity.HasKey(i => new { i.TableId, i.InputAreaId });
                
                entity.Property(i => i.TableId).HasColumnName("table_id").HasColumnType("int").IsRequired();

                entity.Property(i => i.InputAreaId).HasColumnName("input_area_id").HasColumnType("int").IsRequired();

                entity.Property(i => i.Value).HasColumnName("value").HasColumnType("nvarchar(75)").IsRequired();

                


            });


            base.OnModelCreating(modelBuilder);
        }
    }
}
