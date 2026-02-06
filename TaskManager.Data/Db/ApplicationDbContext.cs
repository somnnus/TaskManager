using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Data.Models;

namespace TaskManager.Data.Db
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<UserModel> Users => Set<UserModel>();
        public DbSet<TaskModel> Tasks => Set<TaskModel>();

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>(entity =>
            {
                entity.ToTable("users");
                entity.HasKey(x => x.Id);

                entity
                .Property(x => x.Id)
                .HasColumnName("id");

                entity
                .Property(x => x.Name)
                .HasColumnName("name")
                .IsRequired();

                entity
                .Property(x => x.Login)
                .HasColumnName("login")
                .IsRequired();
                entity
                .Property(x => x.Password)
                .HasColumnName("password")
                .IsRequired();

                entity
                .Property(x => x.Role)
                .HasColumnName("role")
                .HasConversion<int>()
                .IsRequired();

                entity
                .HasIndex(x => x.Login)
                .IsUnique();
            });

            modelBuilder.Entity<TaskModel>(entity =>
            {
                entity.ToTable("tasks");
                entity.HasKey(x => x.Id);

                entity
                .Property(x => x.Id)
                .HasColumnName("id");

                entity
                .Property(x => x.Title)
                .HasColumnName("title").IsRequired();

                entity
                .Property(x => x.Description)
                .HasColumnName("description");

                entity.Property(x => x.CreateDate)
                  .HasColumnName("createDate")
                  .HasDefaultValueSql("datetime('now')")
                  .ValueGeneratedOnAdd()
                  .IsRequired();

                entity.Property(x => x.Status)
                      .HasColumnName("status")
                      .HasConversion<int>()
                      .HasDefaultValueSql("0")
                      .IsRequired();

                entity.Property(x => x.AuthorId).HasColumnName("author_id").IsRequired();
                entity.Property(x => x.ExecutorId).HasColumnName("executor_id");

                entity.HasOne(x => x.Author)
                      .WithMany(u => u.AuthoredTasks)
                      .HasForeignKey(x => x.AuthorId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Executor)
                      .WithMany(u => u.ExecutedTasks)
                      .HasForeignKey(x => x.ExecutorId)
                      .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}
