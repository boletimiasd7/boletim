using boletim.Database.Entidades;
using Microsoft.EntityFrameworkCore;

namespace boletim.Database
{
    public class BoletimDb : DbContext
    {
        public BoletimDb(DbContextOptions<BoletimDb> options) : base(options)
        {
            
        }

        public DbSet<Igreja> Igreja { get; set; }
        public DbSet<IgrejaConfig> IgrejaConfig { get; set; }
    }
}
