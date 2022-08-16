using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IV.Modelos;

namespace IV.AccesoDatos.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Bodega> Bodegas { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<UsuarioAplicacion> UsuarioAplicaciones { get; set; }
        public DbSet<BodegaProducto> BodegaProducto { get; set; }
        public DbSet<Inventario> Inventario { get; set; }
        public DbSet<InventarioDetalle> InventarioDetalle { get; set; }
        public DbSet<Compania> Compania { get; set; }
        public DbSet<CarroCompras> CarroCompras { get; set; }
        public DbSet<Orden> Orden { get; set; }
        public DbSet<OrdenDetalle> OrdenDetalle { get; set; }
    }
}
