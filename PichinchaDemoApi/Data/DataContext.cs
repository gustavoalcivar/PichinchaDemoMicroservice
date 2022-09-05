using PichinchaDemoApi.Models;

namespace PichinchaDemoApi.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    
    public DbSet<Cliente> Clientes { get; set; }

    public DbSet<Cuenta> Cuentas { get; set; }

    public DbSet<Movimiento> Movimientos { get; set; }
}
