

using CoffeeMate.API.Model;
using CoffeeMate.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CoffeeMate.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        public DbSet<Value> Values { get; set; }
        public DbSet<User> User { get; set; }
    }
}