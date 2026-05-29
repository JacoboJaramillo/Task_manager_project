using Microsoft.EntityFrameworkCore;
using Task_manager.Core.Entities;

namespace TaskManager.API.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
  public DbSet<User> Users => Set<User>();
  public DbSet<TaskItem> Tasks => Set<TaskItem>();
  public DbSet<Category> Categories => Set<Category>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
    modelBuilder.Entity<TaskItem>()
        .HasOne(t => t.User).WithMany(u => u.Tasks)
        .HasForeignKey(t => t.UserId).OnDelete(DeleteBehavior.Cascade);
    modelBuilder.Entity<Category>(entity =>
    {
      entity.HasOne(c => c.User).WithMany(u => u.Categories)
      .HasForeignKey(t => t.userId).OnDelete(DeleteBehavior.Cascade);
      entity.HasMany(t => t.Task).WithOne(c => c.Category)
      .HasForeignKey(t => t.CategoryId).OnDelete(DeleteBehavior.SetNull);
    });
  }
}

