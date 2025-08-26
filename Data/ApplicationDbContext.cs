using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Stock1.Models;
namespace Stock1.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Producto> productos { get; set; }
        
        public DbSet<Comprador> compradores { get; set; }
        public DbSet<Categoria> categorias { get; set; }
        public DbSet<Stock> stocks { get; set; }

    }
}
