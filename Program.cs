using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Entity
{
    public class ShopContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Adress> Adresses { get; set; }
        public DbSet<Customer> Customers{ get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=shop.db"); // hangi database providerını kullanmak istiyorsak onu kuruyoruz.
            // optionsBuilder.UseSqlServer(@"Data source=.\SQLEXPRESS;Initial Catalog=ShopDb;Integrated Security=SSPI;");
            // optionsBuilder.UseMySql(@"server:localhost;port=3306;database=ShopDb;user=root;password=12345678Nm.;");

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder){
            modelBuilder.Entity<ProductCategory>()
                        .HasKey(t=> new {t.ProductId,t.CategoryId}); //çoka çok ilişkide tekrar olmaması için yaptık

            modelBuilder.Entity<ProductCategory>()
                        .HasOne(pc=>pc.Product)
                        .WithMany(p=>p.ProductCategories)
                        .HasForeignKey(pc=>pc.ProductId);    

            modelBuilder.Entity<ProductCategory>()
                        .HasOne(c=>c.Category)
                        .WithMany(p=>p.ProductCategories)
                        .HasForeignKey(c=>c.CategoryId); 
        }
    }
    public class User{
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public Customer Customer{ get; set; } // Tek bir obje olarak tanımladık
        public List<Adress> Adresses { get; set; }
    }
    public class Customer{
        public int Id { get; set; }
        public string IdentyNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public User User { get; set; } // Bire bir ilişki kurduk
        public int UserId { get; set; }


    }
    public class Suplier{
        public int Id { get; set; }
        public string Name { get; set; }
        public string TaxNumber { get; set; }

    }
    public class Adress{
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public User User { get; set; }
        public int? UserId { get; set; } // Bunu yazmasakta buna ulaşcaktık.Suru işareti sayayesinde nulldeğer gelirsa hata vermez
    }
    public class Product{
        //primary key
        public int Id { get; set; }
        // [MaxLength(100)] // Max 100 karakter alıyor
        // [Required] // Zorunlu bir alan
        public string Name { get; set; }
        public decimal Price { get; set; } 
        // public int CategoryId { get; set; } // Bire çok ilişki içindi
        public List<ProductCategory> ProductCategories { get; set; }


    }
    public class Category{
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }

    }
    public class ProductCategory{ // Çoka çok ilişki çin ayrı bir sınıf oluşşturuyoruz.(Ayrı tablo)
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
    public class Order{
        public int Id { get; set; }
        public int ProductId{ get; set; }
        public DateTime DateAdded { get; set; } // Sipariş tarihi
    }
    class Program
    {
        static void Main(string[] args)
        {
            // InsertUsers();
            //InsertAdresses();
            // using (var db = new ShopContext())
            // {
            //    var user = db.Users.FirstOrDefault(i =>i.UserName =="AhmetTuran");
            //    if (user != null)
            //    {
            //        user.Adresses = new List<Adress>(); // Bu tanımlamayı yapmadan ekleyemeyiz
            //        user.Adresses.AddRange(
            //            new List<Adress>(){
            //                 new Adress(){FullName="Ahmet Turan",Title="Ev Adresi",Body="Kastamonu"},
            //                 new Adress(){FullName="Ahmet Turan",Title="İş Adresi",Body="Kastamonu"},
            //                 new Adress(){FullName="Ahmet Turan",Title="Ev Adresi",Body="Kastamonu"},
            //            }
                       
            //        );
            //        db.SaveChanges();
            //    }
            // }
            // using (var db = new ShopContext()) // Var olan user üzerine tek bir customer ekleme
            // {
            //     var customer = new Customer({
            //         IdentyNumber ="12345678",
            //         FirstName = "Sadık",
            //         LastName = "Turan",
            //         User = db.Users.FirstOrDefault(i=>i.Id==3)
            //     };
            //     db.Customers.Add(customer);
            //     db.SaveChanges();
            // }

            // using (var db = new ShopContext())
            // {
            //     var user = new User(){ // User oluştururken customer oluşturuldu ve ilişkilendirildi.
            //         UserName ="Deneme",
            //         Email = "deneme@gmail.com",
            //         Customer = new Customer(){
            //             FirstName = "sdasd",
            //             LastName = "sadasd",
            //             IdentyNumber = "5454565446645"

            //         }
            //     };
            //     db.Users.Add(user);
            //     db.SaveChanges();
            // }
           
        }
        static void InsertUsers(){
            var users = new List<User>(){
                new User(){UserName="SadikTuran",Email="sadik@gmail.com"},
                new User(){UserName="AhmetTuran",Email="ahmet@gmail.com"},
                new User(){UserName="MehmetTuran",Email="mehmet@gmail.com"},
                new User(){UserName="CanTuran",Email="can@gmail.com"},
            };
            using (var db = new ShopContext())
            {
                db.Users.AddRange(users);
                db.SaveChanges();
            }
        }
        static void InsertAdresses(){
            var adresses = new List<Adress>(){
                new Adress(){FullName="SadikTuran",Title="Ev Adresi",Body="Kastamonu",UserId=1},
                new Adress(){FullName="SadikTuran",Title="Ev Adresi",Body="Kastamonu",UserId=1},
                new Adress(){FullName="SadikTuran",Title="Ev Adresi",Body="Kastamonu",UserId=2},
                new Adress(){FullName="SadikTuran",Title="Ev Adresi",Body="Kastamonu",UserId=3},
                new Adress(){FullName="SadikTuran",Title="İş Adresi",Body="Kastamonu",UserId=4},
                new Adress(){FullName="SadikTuran",Title="İş Adresi",Body="Kastamonu",UserId=4},
            };
               
            using (var db = new ShopContext())
            {
                db.Adresses.AddRange(adresses);
                db.SaveChanges();
            }
        }
        static void DeleteProduct(int id){
            using (var db = new ShopContext())
            {
                var p = new Product(){Id=1};
                db.Products.Remove(p);     // Select sorgsu yapılmadan silinme işlemi yapıldı.
                db.SaveChanges();
                
            }
            // using (var db = new ShopContext())
            // {
            //     var p = db.Products.Where(i => i.Id == id).FirstOrDefault();
            //     if (p != null)
            //     {
            //         db.Products.Remove(p);
            //         db.SaveChanges();
            //     }
            // }
        }
        static void UpdateProduct(){
            // using (var db = new ShopContext())
            // {
            //     var p = db.Products.Where(i => i.Id == 1).FirstOrDefault();
            //     if(p != null){
            //         p.Price = 2400;
            //        db.Products.Update(p);
            //        db.SaveChanges();

            //     }
            // }
            using (var db = new ShopContext())
            {
                var entity = new Product(){Id=1};
                db.Products.Attach(entity); // En doğr güncelleme kullanımı bu.
                entity.Price = 3000;
                db.SaveChanges();

            }
            // using (var db = new ShopContext())
            // {
            //     var p = db.Products.Where(i => i.Id == 1).FirstOrDefault();
            //     if(p != null){
            //         p.Price *= 1.2m;
            //         db.SaveChanges();
            //     }
            // }

        }
        static void GetProductByName(string name){
            using (var context = new ShopContext())
            {
                var products = context
                .Products
                // Tolower ile küçük harfe çevirdik
                .Where(a => a.Name.ToLower().Contains(name.ToLower())) // içinde name varsa true değerini döndürüyor.
                .Select(a => new { // Sadece Name ve Price  kolonlarını seçtik.
                    a.Name,
                    a.Price
                })
                .ToList(); // Tek bir ürün döndüreceksek kullanabiliriz.
                foreach (var item in products)
                {
                    Console.WriteLine($"name: {item.Name} price: {item.Price}");
                }
                

            }
        }
        static void GetProductById(int id){
            using (var context = new ShopContext())
            {
                var products = context
                .Products
                .Where(a => a.Id == id) // id ye eşit olup olmadıını sorguladık
               // .Where(a => a.Price >1000) // Fiyatı 100den büyük olanlar gelir
                .Select(a => new { // Sadece Name ve Price  kolonlarını seçtik.
                    a.Name,
                    a.Price
                })
                .FirstOrDefault(); // Tek bir ürün döndüreceksek kullanabiliriz.

                Console.WriteLine($"name: {products.Name} price: {products.Price}");

            }
        }
        static void GetAllProducts(){
            using (var context = new ShopContext())
            {
                var products = context
                .Products
                .Select(a => new { // Sadece Name ve Price  kolonlarını seçtik.
                    a.Name,
                    a.Price
                })
                .ToList(); // Bütün ürünleri alır

                foreach (var item in products)
                {
                    Console.WriteLine($"name: {item.Name} price: {item.Price}");
                    
                }

            }
        }
        static void AddProduct(){
            using (var db = new ShopContext()) // using çine aldığımızda iş bittiğinde silincek
            {
                var products = new List<Product>{
                    new Product{Name ="Samsung S5", Price = 1000},
                    new Product{Name ="Samsung S6", Price = 2000},
                    new Product{Name ="Samsung S7", Price = 3000},
                    new Product{Name ="Samsung S8", Price = 5000},
                    new Product{Name ="Samsung S9", Price = 7000},
                    new Product{Name ="Samsung S10", Price = 10000},
                };
                db.Products.AddRange(products);
                db.SaveChanges(); // Bekleyen tüm sorgular aktarılır.
                Console.WriteLine("Veriler eklendi");
            } 

        }
        static void AddProducts(){
            using (var db = new ShopContext()) // using çine aldığımızda iş bittiğinde silincek
            {
                var products = new List<Product>{
                    new Product{Name ="Samsung S5", Price = 1000},
                    new Product{Name ="Samsung S6", Price = 2000},
                    new Product{Name ="Samsung S7", Price = 3000},
                    new Product{Name ="Samsung S8", Price = 5000},
                    new Product{Name ="Samsung S9", Price = 7000},
                    new Product{Name ="Samsung S10", Price = 10000},
                };
                db.Products.AddRange(products);
                db.SaveChanges(); // Bekleyen tüm sorgular aktarılır.
                Console.WriteLine("Veriler eklendi");
            } 
            
        }
    }
}
