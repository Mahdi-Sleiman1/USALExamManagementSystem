using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace USALExamManagementSystem.Infrastructure.Models;

public class USALDbContext : IdentityDbContext<IdentityUser>
{
    public USALDbContext(DbContextOptions<USALDbContext> options)
        : base(options) { }

    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Exam> Exams => Set<Exam>();
    public DbSet<Major> Majors => Set<Major>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<User> UsersProfile => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("UsersProfile"); // 🔴 THIS IS THE FIX

            entity.HasKey(e => e.UserId);

            entity.Property(e => e.UserId)
                  .HasMaxLength(450);

            entity.Property(e => e.Email)
                  .HasMaxLength(100)
                  .IsRequired();

            entity.Property(e => e.FullName)
                  .HasMaxLength(100)
                  .IsRequired();

            entity.HasOne(e => e.Role)
                  .WithMany(r => r.Users)
                  .HasForeignKey(e => e.RoleId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Major)
                  .WithMany()
                  .HasForeignKey(e => e.MajorId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }


}


