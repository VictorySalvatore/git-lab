using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext() : base("name=DefaultConnection")
        {
            this.Configuration.LazyLoadingEnabled = false; // - ленивая загр.
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Reader> Readers { get; set; }
        public DbSet<Loan> Loans { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<Loan>()
                .HasRequired(l => l.Book)
                .WithMany()
                .HasForeignKey(l => l.BookUdc)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Loan>()
                .HasRequired(l => l.Reader)
                .WithMany()
                .HasForeignKey(l => l.ReaderId)
                .WillCascadeOnDelete(false);
            base.OnModelCreating(modelBuilder);
        }
    }
}
