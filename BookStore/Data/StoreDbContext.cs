using BookStore.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace BookStore.Data
{
    public class StoreDbContext : DbContext
    {

        public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
    }
}
