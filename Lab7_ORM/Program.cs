using Lab7_ORM;
using Microsoft.EntityFrameworkCore;

using var db = new AppDbContext();
db.Database.EnsureCreated();
Console.WriteLine("1. Создание категорий и продуктов");
var elics = new Category { Name = "Электроника" };
var phone = new Product { Name = "Смартфон", Price = 500m, Category = elics };
var laptop = new Product { Name = "Ноутбук", Price = 1200m, Category = elics };
db.Categories.Add(elics);
db.Products.AddRange(phone, laptop);
db.SaveChanges();
Console.WriteLine($"Создана категория: {elics.Name}, ID = {elics.Id}");
Console.WriteLine("\n2. Чтение продуктов по категориям");
var catFromDb = db.Categories
    .Include(c => c.Products) // JOIN
    .FirstOrDefault(c => c.Id == elics.Id);
if (catFromDb != null)
{
    Console.WriteLine($"Категория: {catFromDb.Name}");
    foreach (var p in catFromDb.Products)
    {
        Console.WriteLine($"  - {p.Name}, Цена: {p.Price}");
    }
}
Console.WriteLine("\n3. Обновление категории у продукта");
var prodUpdate = db.Products.FirstOrDefault(p => p.Name == "Смартфон");
if (prodUpdate != null)
{
    var gadgets = new Category { Name = "Гаджеты" };
    db.Categories.Add(gadgets);
    db.SaveChanges();
    prodUpdate.CategoryId = gadgets.Id;
    db.SaveChanges();
    Console.WriteLine("Продукт «Смартфон» перемещён в категорию «Гаджеты».");
}
Console.WriteLine("\n4. Удаление категории и всех связанных продуктов");
var catDelete = db.Categories
    .Include(c => c.Products)
    .FirstOrDefault(c => c.Name == "Электроника");
if (catDelete != null)
{
    Console.WriteLine($"Удаляем категорию: {catDelete.Name}");
    Console.WriteLine($"Количество продуктов в категории до удаления: {catDelete.Products.Count}");

    db.Categories.Remove(catDelete);
    db.SaveChanges();
    Console.WriteLine("Категория и её продукты удалены.");
}
Console.WriteLine("\n5. Проверка оставшихся продуктов");
var remProd = db.Products.ToList();
foreach (var p in remProd)
{
    Console.WriteLine($"- {p.Name} (CategoryId: {p.CategoryId})");
}