using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Collections.Generic;

namespace Lab7_ORM;

[Table("categories")]
public class Category
{
    [Column("id")] // первичный ключ
    public int Id { get; set; }
    [Column("name")]
    public string Name { get; set; } = string.Empty;
    public List<Product> Products { get; set; } = new(); // навигационное свойство
}
[Table("products")]
public class Product
{
    [Column("id")]
    public int Id { get; set; }
    [Column("name")]
    public string Name { get; set; } = string.Empty;
    [Column("price")]
    public decimal Price { get; set; }
    [Column("category_id")]
    public int CategoryId { get; set; } // внешний
    public Category Category { get; set; } = null!; // навигационное свойство
}
public class AppDbContext : DbContext
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optsBuilder)
    {
        optsBuilder.UseNpgsql("Host=localhost;Database=mydb;Username=username;Password=password");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Cascade); // каскадное удаление
    }
}